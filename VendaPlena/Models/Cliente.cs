using System.ComponentModel.DataAnnotations;

namespace VendinhaPlena.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "CPF obrigatório")]
    [StringLength(11, MinimumLength = 11)]
    [RegularExpression("^[0-9]+$")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "Data de nascimento obrigatória")]
    public DateTime DataNascimento { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }

    public int Idade
    {
        get
        {
            var hoje = DateTime.Today;

            var idade = hoje.Year - DataNascimento.Year;

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