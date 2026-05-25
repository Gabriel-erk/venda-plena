using Microsoft.Data.Sqlite;
using VendinhaPlena.Database;
using VendinhaPlena.Enums;
using VendinhaPlena.Models;

namespace VendinhaPlena.Services;

public class DividaService
{
    private DatabaseConfig database;

    public DividaService()
    {
        database = new DatabaseConfig();
    }

    public bool Criar(Divida divida)
    {
        using var connection = database.GetConnection();

        connection.Open();

        // verifica se já existe dívida pendente
        var verificarCommand = connection.CreateCommand();

        verificarCommand.CommandText =
        @"
            SELECT COUNT(*)

            FROM dividas

            WHERE cliente_id = @clienteId
            AND situacao = 0
        ";

        verificarCommand.Parameters.AddWithValue(
            "@clienteId",
            divida.ClienteId
        );

        var quantidade =
            Convert.ToInt32(
                verificarCommand.ExecuteScalar()
            );

        if (quantidade > 0)
        {
            Console.WriteLine(
                "Cliente já possui dívida em aberto."
            );

            return false;
        }

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            INSERT INTO dividas
            (
                cliente_id,
                valor,
                situacao,
                data_criacao
            )

            VALUES
            (
                @clienteId,
                @valor,
                @situacao,
                @dataCriacao
            )
        ";

        command.Parameters.AddWithValue(
            "@clienteId",
            divida.ClienteId
        );

        command.Parameters.AddWithValue(
            "@valor",
            divida.Valor
        );

        command.Parameters.AddWithValue(
            "@situacao",
            (int)SituacaoDivida.Pendente
        );

        command.Parameters.AddWithValue(
            "@dataCriacao",
            DateTime.Now
        );

        command.ExecuteNonQuery();

        return true;
    }

    public List<Divida> ListarPorCliente(int clienteId)
    {
        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            SELECT
                id,
                cliente_id,
                valor,
                situacao,
                data_criacao,
                data_pagamento

            FROM dividas

            WHERE cliente_id = @clienteId
        ";

        command.Parameters.AddWithValue(
            "@clienteId",
            clienteId
        );

        using var reader = command.ExecuteReader();

        var lista = new List<Divida>();

        while (reader.Read())
        {
            var divida = new Divida
            {
                Id = reader.GetInt32(0),

                ClienteId = reader.GetInt32(1),

                Valor = reader.GetDecimal(2),

                Situacao =
                    (SituacaoDivida)
                    reader.GetInt32(3),

                DataCriacao =
                    reader.GetDateTime(4),

                DataPagamento =
                    reader.IsDBNull(5)
                    ? null
                    : reader.GetDateTime(5)
            };

            lista.Add(divida);
        }

        return lista;
    }

    public bool MarcarComoPaga(int id)
    {
        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            UPDATE dividas

            SET
                situacao = @situacao,
                data_pagamento = @dataPagamento

            WHERE id = @id
        ";

        command.Parameters.AddWithValue(
            "@situacao",
            (int)SituacaoDivida.Paga
        );

        command.Parameters.AddWithValue(
            "@dataPagamento",
            DateTime.Now
        );

        command.Parameters.AddWithValue(
            "@id",
            id
        );

        var linhasAfetadas =
            command.ExecuteNonQuery();

        return linhasAfetadas > 0;
    }

    public bool Remover(int id)
    {
        using var connection = database.GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            DELETE FROM dividas
            WHERE id = @id
        ";

        command.Parameters.AddWithValue(
            "@id",
            id
        );

        var linhasAfetadas =
            command.ExecuteNonQuery();

        return linhasAfetadas > 0;
    }
}