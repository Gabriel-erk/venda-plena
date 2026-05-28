using System.ComponentModel.DataAnnotations;

namespace VendinhaPlena.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")] // Anotação de validação para indicar que o nome é obrigatório, onde entregará uma msg de erro caso ele não seja passado, mensagem de erro terá o conteúdo: "O nome é obrigatório"
    [StringLength(100)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "CPF obrigatório")]
    [StringLength(11, MinimumLength = 11)] // serve para validar o tamanho do texto, nesse caso, o primeiro parâmetro dita o máximo (11 caracteres) e o segundo parâmetro dita o minímo de caracteres para aquele campo ser "válido" (no caso, no minimo, 11 também, logo, máximo 11 e minimo 11, qualquer quantia fora dessas não será aceita)
    [RegularExpression("^[0-9]+$")] // expressão regular regex que aceita apenas números
    public string Cpf { get; set; }

    [Required(ErrorMessage = "Data de nascimento obrigatória")]
    public DateTime DataNascimento { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; } // "?" diz que a propriedade "Email" PODE ser nula

    // toda vez que consultarem o atributo Idade ele será calculado novamente por causa da forma que o método get dele está
    public int Idade
    {
        get
        {
            // pega a data de hoje
            var hoje = DateTime.Today;

            // subtrai a nossa data atual pelo ANO que a propriedade DataNascimento possui (porém a pessoa pode ainda não ter feito aniversário esse ano, logo, precisa outro calculo para saber se ela já fez aniversário ou não, pois, no meu exemplo, no dia de hoje (26/5/26), se eu me registrasse aqui, ele me entregaria que eu tenho 20 anos, (sem a verificacao abaixo, claro) mas eu ainda não fiz aniversário
            var idade = hoje.Year - DataNascimento.Year;

            // DataNascimento.Date acessa diretamente a data que ela possui
            // hoje.AddYears(-idade), faz com que, ao invés de adicionar os anos (como o nome AddyYears sugere), como parâmetro passamos -idade, ou seja, estamos dizendo para subtrair o valor de idade (idade atual da pessoa dps do calculo do ano atual - ano que ela nasceu), assim ficando o ano que a pessoa nasceu
            // logo, se a de nascimento da pessoa (DataNascimento.Date) for MAIOR que a DATA ATUAL (ex aqui, pessoa colocou que nasceu dia 10/12/2000, logo, o valor de DataNascimento.Date abaixo seria == 10/12/2000 (até o momento valor de idade seria 26 pois ele ainda não sabe que a pessoa não fez aniversárioa ainda) e hoje.AddYears(-idade) == hoje.AddYears(-26), logo a comparacao correta seria: if(10/12/2000 > 26/12/2000), se retornar TRUE, significa que pessoa ainda não fez aniversário (como sei que é true e que uma data é maior que a outra? como os anos são iguais, vai usar mes e dia para comparar, como o mes (12) é maior que o outro mês (5) já vai considerar que a primeira data é menor, logo, nesse caso a pessoa ainda não fez aniversário, logo, abaixa 1, deixando a idade correta)) 
            if (DataNascimento.Date > hoje.AddYears(-idade))
            {
                idade--;
            }

            return idade;
        }
    }

    public void PrintDados()
    {
        Console.WriteLine($"Id: {Id}");
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"CPF: {Cpf}");
        Console.WriteLine($"Nascimento: {DataNascimento:dd/MM/yyyy}");
        Console.WriteLine($"Idade: {Idade}");
        Console.WriteLine($"Email: {Email}");
    }
}