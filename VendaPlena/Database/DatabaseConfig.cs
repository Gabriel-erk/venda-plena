using Microsoft.Data.Sqlite;

namespace VendinhaPlena.Database;

public class DatabaseConfig
{
    private string connectionString = "Data Source=vendinha.db";

    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    }

    public void InicializarBanco()
    {
        using var connection = GetConnection();

        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS clientes (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                nome TEXT NOT NULL,
                cpf TEXT NOT NULL UNIQUE,
                data_nascimento TEXT NOT NULL,
                email TEXT
            );

            CREATE TABLE IF NOT EXISTS dividas (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                cliente_id INTEGER NOT NULL,
                valor REAL NOT NULL,
                situacao INTEGER NOT NULL,
                data_criacao TEXT NOT NULL,
                data_pagamento TEXT,

                FOREIGN KEY(cliente_id)
                REFERENCES clientes(id)
            );
        ";

        command.ExecuteNonQuery();
    }
}