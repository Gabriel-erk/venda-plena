using VendinhaPlena.Database;
using VendinhaPlena.Models;
using VendinhaPlena.Services;

var database = new DatabaseConfig();

database.InicializarBanco();

var clienteService = new ClienteService();

var dividaService = new DividaService();

while (true)
{
    Console.Clear();

    Console.WriteLine("=== VENDINHA PLENA ===");
    Console.WriteLine("1 - Cadastrar cliente");
    Console.WriteLine("2 - Listar clientes");
    Console.WriteLine("3 - Buscar cliente");
    Console.WriteLine("4 - Atualizar cliente");
    Console.WriteLine("5 - Remover cliente");
    Console.WriteLine("6 - Criar dívida");
    Console.WriteLine("7 - Listar dívidas do cliente");
    Console.WriteLine("8 - Marcar dívida como paga");
    Console.WriteLine("9 - Remover dívida");
    Console.WriteLine("0 - Sair");

    Console.Write("\nOpção: ");

    var opcao = Console.ReadLine();

    Console.Clear();

    // CADASTRAR CLIENTE
    if (opcao == "1")
    {
        var cliente = new Cliente();

        Console.Write("Nome: ");
        cliente.Nome = Console.ReadLine();

        Console.Write("CPF: ");
        cliente.Cpf = Console.ReadLine();

        Console.Write("Data nascimento: ");
        cliente.DataNascimento =
            DateTime.Parse(Console.ReadLine());

        Console.Write("Email: ");
        cliente.Email = Console.ReadLine();

        var sucesso = clienteService.Criar(cliente);

        if (sucesso)
        {
            Console.WriteLine("\nCliente criado!");
        }
    }

    // LISTAR CLIENTES
    else if (opcao == "2")
    {
        Console.Write("Página: ");

        int page =
            int.Parse(Console.ReadLine());

        Console.Write("Busca: ");

        var busca = Console.ReadLine();

        clienteService.Listar(page, 10, busca);
    }

    // BUSCAR CLIENTE
    else if (opcao == "3")
    {
        Console.Write("Id cliente: ");

        int id =
            int.Parse(Console.ReadLine());

        var cliente =
            clienteService.Buscar(id);

        if (cliente == null)
        {
            Console.WriteLine("Cliente não encontrado.");
        }
        else
        {
            cliente.PrintDados();
        }
    }

    // ATUALIZAR CLIENTE
    else if (opcao == "4")
    {
        Console.Write("Id cliente: ");

        int id =
            int.Parse(Console.ReadLine());

        var cliente =
            clienteService.Buscar(id);

        if (cliente == null)
        {
            Console.WriteLine("Cliente não encontrado.");
        }
        else
        {
            Console.Write("Novo nome: ");
            cliente.Nome = Console.ReadLine();

            Console.Write("Novo CPF: ");
            cliente.Cpf = Console.ReadLine();

            Console.Write("Nova data nascimento: ");
            cliente.DataNascimento =
                DateTime.Parse(Console.ReadLine());

            Console.Write("Novo email: ");
            cliente.Email = Console.ReadLine();

            var sucesso =
                clienteService.Atualizar(cliente);

            if (sucesso)
            {
                Console.WriteLine("Cliente atualizado!");
            }
        }
    }

    // REMOVER CLIENTE
    else if (opcao == "5")
    {
        Console.Write("Id cliente: ");

        int id =
            int.Parse(Console.ReadLine());

        var sucesso =
            clienteService.Remover(id);

        if (sucesso)
        {
            Console.WriteLine("Cliente removido!");
        }
        else
        {
            Console.WriteLine("Cliente não encontrado.");
        }
    }

    // CRIAR DIVIDA
    else if (opcao == "6")
    {
        var divida = new Divida();

        Console.Write("Id cliente: ");

        divida.ClienteId =
            int.Parse(Console.ReadLine());

        Console.Write("Valor dívida: ");

        divida.Valor =
            decimal.Parse(Console.ReadLine());

        var sucesso =
            dividaService.Criar(divida);

        if (sucesso)
        {
            Console.WriteLine("Dívida criada!");
        }
    }

    // LISTAR DIVIDAS
    else if (opcao == "7")
    {
        Console.Write("Id cliente: ");

        int clienteId =
            int.Parse(Console.ReadLine());

        var lista =
            dividaService.ListarPorCliente(clienteId);

        foreach (var item in lista)
        {
            Console.WriteLine("-------------------");

            item.PrintDados();
        }
    }

    // PAGAR DIVIDA
    else if (opcao == "8")
    {
        Console.Write("Id dívida: ");

        int id =
            int.Parse(Console.ReadLine());

        var sucesso =
            dividaService.MarcarComoPaga(id);

        if (sucesso)
        {
            Console.WriteLine("Dívida paga!");
        }
    }

    // REMOVER DIVIDA
    else if (opcao == "9")
    {
        Console.Write("Id dívida: ");

        int id =
            int.Parse(Console.ReadLine());

        var sucesso =
            dividaService.Remover(id);

        if (sucesso)
        {
            Console.WriteLine("Dívida removida!");
        }
    }

    // SAIR
    else if (opcao == "0")
    {
        break;
    }

    else
    {
        Console.WriteLine("Opção inválida.");
    }

    Console.WriteLine("\nPressione qualquer tecla...");

    Console.ReadKey();
}