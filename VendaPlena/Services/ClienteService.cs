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

    public bool Validar(Cliente cliente, out List<ValidationResult> erros)
    {
        var contexto = new ValidationContext(cliente);

        erros = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            cliente,
            contexto,
            erros,
            true
        );

        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT COUNT(*)
            FROM clientes
            WHERE cpf = @cpf
            AND id != @id
        ";

        command.Parameters.AddWithValue("@cpf", cliente.Cpf);
        command.Parameters.AddWithValue("@id", cliente.Id);

        var quantidade = Convert.ToInt32(command.ExecuteScalar());

        if (quantidade > 0)
        {
            erros.Add(
                new ValidationResult(
                    "Já existe cliente com esse CPF"
                )
            );

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