using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;
using VendinhaPlena.Database;
using VendinhaPlena.Models;

namespace VendinhaPlena.Services;

public class ClienteService
{
    private DatabaseConfig database;

    public ClienteService()
    {
        database = new DatabaseConfig();
    }

    // out List<ValidationResult> erros == presente em validaćão de objetos, geralmente com: Validator.TryValidateObject
    // onde a ideia principal é: o método vai preenhcer essa váriavel para mim
    // out == retornar valores adicionais por parâmetro geralmente um método c# retorna só UMA coisa, mas ás vezes você quer retornar: sucesso/falha, lista de erros, resultado calculado...
    // este método abaixo vai devolver/preencher uma lista de erros de validaćão, logo, graćas ao out, eu posso simplesmente chamar o método passando uma lista de erros de validaćao para o out e não vou ter a necessidade de fazer: errosTest = Validar(clienteUm, out erros), posso apenas fazer Validar(clienteUm, out erros) que minha váriavel/objeto "erros" terá seu valor atualizado pelo retorno de out, graćas ao out, o método vai retornar o valor de dentro dele para a váriavel presente no out (que eu passei de parâmetro, obviamente)
    public bool Validar(Cliente cliente, out List<ValidationResult> erros)
    {   
        // validationContext == objeto que guarda: qual objeto está sendo validado (cliente, assim como passamos por parâmetro), info sobre o objeto que será validado e metadados de valićão, onde basicamente o que queremos dizer é: o objeto que quero validar é o: cliente (Ele será validado a partir das DataAnnotations que deixamos no MODEL do objeto que está sendo passado como parâmetro de: ValidationContext, como por ex: Required, MinimumLenght e etc, ele sabe as regras que vai usar para validar pois eu DESCREVI elas na definićão do model)
        var contexto = new ValidationContext(cliente); // prepara o ambiente de validaćão
        // criando uma lista vazia que armazenará os erros encontrados com o tipo: VALIDATIONRESULT que representa: erro de validaćão, uma mensagem, qual campo deu problema (um exemplo de erro guardado nela: "Data de nascimento obrigatória", pois quando tentar validar a data de nascimento do cliente e der erro, ele pegará a mensagem de erro que deixamos lá na nossa DataAnnotation de Required )
        erros = new List<ValidationResult>(); // cria um lugar para guardar os erros, aqui estamos inicializando o 2 parâmetro que recebemos (erros), com uma lista vazia
        // pega o objeto (cliente), olha as validaćões dele (DataAnnotations como: required no campo nome, regex no campo cpf, minimo e maximo de caracteres em cpf....), testa tudo (contexto), pode retornar: true ou false e preenche a lista de erros (erros) 
        var valido = Validator.TryValidateObject( // analisa o objeto e retorna se é valido e quais erros existem
            cliente, // objeto que será validado
            contexto, // contexto que será validado (usando informaćões daquel objeto (tipo, DataAnnotations == required, minimo e máximo de caracteres etc))
            erros, // lista onde os erros serão colocados
            true // esse parâmetro diz: valide toda as propriedades do objeto, se fosse false, ele só validaria as propriedades obrigatórias (com Required), ou seja, se fosse false, ele não validaria o campo Email, pois ele não tem a anotação de Required, logo, se fosse false, eu poderia colocar um email com formato errado e ele não reclamaria, pois ele não estaria validando o campo email, mas como eu quero validar o campo email também (pois ele tem a anotação de EmailAddress), eu passo true para validar todas as propriedades do objeto cliente
        ); // no fim da execućão deste método Validator.TryValidateObject(), o valor de valido será true ou false, caso qualquer validaćão no objeto cliente a partir do contexto passado (objeto que será validado, onde contem seu tipo e regras (required, min, max...) onde caso QUALQUER um não seja atendido, ele retornará false para "valido" e preenchera o 3 parâmetro (erros) com todos os erros presentes)

        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        // query sql abaixo verifica se existe outro cliente com o mesmo cpf (cpf que passo para @cpf nas linhas abaixo: command.Parameters.AddWithValue("@cpf", cliente.cpf)), ignorando um id especifico (passo o id que quero ignorar para @id nas linhas abaixo: command.Parameters.AddWithValue("@id", cliente.Id))
        command.CommandText =
        // select count(*) == conte quantos registros existem, ou seja: conte quantos clientes existem com o mesmo cpf (que passo como parâmetro nas linhas abaixo) IGNORANDO um id especifico (que passo nas linhas abaixo também)
        @"
            SELECT COUNT(*)
            FROM clientes
            WHERE cpf = @cpf
            AND id != @id
        ";

        command.Parameters.AddWithValue("@cpf", cliente.Cpf);
        command.Parameters.AddWithValue("@id", cliente.Id);

        // command.ExecuteScalar == executa uma querySQL e retorna UM único valor (a query que ele executa é a query dentro de command == query nas linhas acima que verifica se existe um cpf igual ao informado excluindo um id especifico de cliente), porém o retorno do método command.ExecuteScalar é um objeto, logo, usamos: Convert.ToInt32 para converter do objeto retornado para em um tipo "int", logo, "quantidade" é do tipo int (vai ter valores tipo 0, 1, 2 etc)
        var quantidade = Convert.ToInt32(command.ExecuteScalar());
        // se existe pelo menos UM CLIENTE (quantdade == 1 ou +) com o CPF informado em: command.Parameters.AddWithValue("@cpf", cliente.Cpf), significa que o cpf que estou tentando inserir no banco já tem dono, logo, vou retornar um erro
        if (quantidade > 0)
        {
            // adiciono um erro na lista de erros (erros) com erros.Add
            erros.Add(
                new ValidationResult( // erro adicionado na lista: erros é um objeto do tipo: ValidationResult (mesmo tipo da lista "erros") com a mensagem: "Já existe cliente com esse CPF"
                    "Já existe cliente com esse CPF"
                )
            );
            // caso tenha entrado nesse if, sei que meu objeto (cliente) NÃO é válido, logo, vou atribuir manualmene este valor a ele
            valido = false;
        }
        return valido;
    }

    public bool Criar(Cliente cliente)
    {
        if (!Validar(cliente, out var erros))
        {
            foreach (var erro in erros)
            {
                Console.WriteLine(erro.ErrorMessage);
            }

            return false;
        }

        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            INSERT INTO clientes
            (nome, cpf, data_nascimento, email)

            VALUES
            (@nome, @cpf, @dataNascimento, @email)
        ";

        command.Parameters.AddWithValue("@nome", cliente.Nome);
        command.Parameters.AddWithValue("@cpf", cliente.Cpf);
        command.Parameters.AddWithValue("@dataNascimento", cliente.DataNascimento);
        command.Parameters.AddWithValue("@email", cliente.Email);

        command.ExecuteNonQuery();

        return true;
    }

    public List<Cliente> Listar(
        int page = 1,
        int pageSize = 10,
        string busca = ""
    )
    {
        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        var skip = (page - 1) * pageSize;

        command.CommandText =
        @"
            SELECT
                c.id,
                c.nome,
                c.cpf,
                c.data_nascimento,
                c.email,

                COALESCE(SUM(
                    CASE
                        WHEN d.situacao = 0
                        THEN d.valor
                        ELSE 0
                    END
                ), 0) as total_dividas

            FROM clientes c

            LEFT JOIN dividas d
            ON d.cliente_id = c.id

            WHERE c.nome LIKE @busca

            GROUP BY c.id

            ORDER BY total_dividas DESC

            LIMIT @limit
            OFFSET @offset
        ";

        command.Parameters.AddWithValue("@busca", $"%{busca}%");
        command.Parameters.AddWithValue("@limit", pageSize);
        command.Parameters.AddWithValue("@offset", skip);

        using var reader = command.ExecuteReader();

        var lista = new List<Cliente>();

        while (reader.Read())
        {
            var cliente = new Cliente
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Cpf = reader.GetString(2),
                DataNascimento = reader.GetDateTime(3),
                Email = reader.IsDBNull(4)
                    ? null
                    : reader.GetString(4)
            };

            lista.Add(cliente);

            Console.WriteLine("--------------------------------");
            cliente.PrintDados();
            Console.WriteLine($"Total Dívidas: R$ {reader.GetDecimal(5)}");
        }

        return lista;
    }

    public Cliente Buscar(int id)
    {
        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT
                id,
                nome,
                cpf,
                data_nascimento,
                email

            FROM clientes

            WHERE id = @id
        ";

        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Cliente
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Cpf = reader.GetString(2),
                DataNascimento = reader.GetDateTime(3),
                Email = reader.IsDBNull(4)
                    ? null
                    : reader.GetString(4)
            };
        }

        return null;
    }

    public bool Atualizar(Cliente cliente)
    {
        if (!Validar(cliente, out var erros))
        {
            foreach (var erro in erros)
            {
                Console.WriteLine(erro.ErrorMessage);
            }

            return false;
        }

        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            UPDATE clientes

            SET
                nome = @nome,
                cpf = @cpf,
                data_nascimento = @dataNascimento,
                email = @email

            WHERE id = @id
        ";

        command.Parameters.AddWithValue("@nome", cliente.Nome);
        command.Parameters.AddWithValue("@cpf", cliente.Cpf);
        command.Parameters.AddWithValue("@dataNascimento", cliente.DataNascimento);
        command.Parameters.AddWithValue("@email", cliente.Email);
        command.Parameters.AddWithValue("@id", cliente.Id);

        var linhasAfetadas = command.ExecuteNonQuery();

        return linhasAfetadas > 0;
    }

    public bool Remover(int id)
    {
        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            DELETE FROM clientes
            WHERE id = @id
        ";

        command.Parameters.AddWithValue("@id", id);

        var linhasAfetadas = command.ExecuteNonQuery();

        return linhasAfetadas > 0;
    }
}