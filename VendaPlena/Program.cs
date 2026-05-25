using VendinhaPlena.Database;
using VendinhaPlena.Models;
using VendinhaPlena.Services;

var database = new DatabaseConfig();

database.InicializarBanco();

var clienteService = new ClienteService();

var dividaService = new DividaService();

var cliente = new Cliente
{
    Nome = "Gabriel Ferreira",
    Cpf = "12345678910",
    DataNascimento = new DateTime(2004, 1, 1),
    Email = "gabriel@gmail.com"
};

clienteService.Criar(cliente);

var clientes = clienteService.Listar();

var primeiroCliente = clientes.First();

var divida = new Divida
{
    ClienteId = primeiroCliente.Id,
    Valor = 150
};

dividaService.Criar(divida);

var dividas = dividaService.ListarPorCliente(
    primeiroCliente.Id
);

foreach (var item in dividas)
{
    item.PrintDados();
}