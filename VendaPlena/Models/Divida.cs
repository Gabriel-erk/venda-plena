using VendinhaPlena.Enums;

namespace VendinhaPlena.Models;

public class Divida
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public decimal Valor { get; set; }

    public SituacaoDivida Situacao { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataPagamento { get; set; }

    public void PrintDados()
    {
        Console.WriteLine($"Id: {Id}");
        Console.WriteLine($"Cliente Id: {ClienteId}");
        Console.WriteLine($"Valor: R$ {Valor}");
        Console.WriteLine($"Situação: {Situacao}");
        Console.WriteLine($"Data Criação: {DataCriacao:dd/MM/yyyy}");

        if (DataPagamento != null)
        {
            Console.WriteLine($"Data Pagamento: {DataPagamento:dd/MM/yyyy}");
        }
    }
}