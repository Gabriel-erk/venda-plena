// arquivo de configuraćão e criaćão do banco SQLite

// imporata a biblioteca do SQLite, pois sem isso o c# não reconheceria comandos como: SqliteConnection, comandos SQL nem a conexão com o banco
using Microsoft.Data.Sqlite;

namespace VendinhaPlena.Database;

// classe que vai cuidar do banco (DatabaseConfig)
public class DatabaseConfig
{
    // "Data Source=vendinha.db" que define qual banco vai ser usado, fazendo referencia ao meu arquivo na raiz chamado: vendinha.db, que é o arquivo SQLite
    private string connectionString = "Data Source=vendinha.db";

    // método publico
    // tipo de retorno: SqliteConnection 
    // nomé do método: GetConnection
    // inuito do método: retorna uma conexáo com o banco, onde o retorno dele é o retorno da execućão método SqliteConnection onde passamos por parâmetro qual banco vai ser usado (valor de connectionString), new == trazer uma instância de uma classe específica, nesse caso, retornarmos uma INSTÂNCIA de SqliteConnection (que é uma conexão com o nosso banco sqlite: vendinha.db)
    // quando alguém chamar o nosso método abaixo: GetConnection receberá uma conexão com nosso banco (vendinha.db) pronta para usar
    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    }

    // método responsável por criar as tabelas do banco
    /* FLUXO COMPLETO
    * conecta no SQLite (vendinha.db com using var connection = GetConnection() onde GetConnection retorna uma conexão com o banco de dados SQLite: vendinha.db)
    * abre conexão (connection.Open)
    * monta SQL (var command = connection.CreateCommand() e command.CommandText = "comando sql aqui como: CREATE, INSERT, UPDATE ou DELETE)
    * monta tabelas (dentro de command.CommandText, está nosso código SQL de montagem de tabelas (clientes e dividas))
    * fecha conexão automaticamente ao chegar no final do método graćas ao "using" presente em using var connection = GetConnection();
    */
    public void InicializarBanco()
    {
        // objetivo: criar um objeto de conexão com o banco SQLite "vendinha.db" através do método GetConnection() que retorna uma conexão com este banco
        // using ==  quando terminar, feche e descarte automaticante, pois, sem isso, eu poderia deixar conexões abertas pois, sem ele, caso eu não fechasse MANUALMENTE as conexões elas consumiriam memória e etc, logo, com o using, no final do escopo atual, automaticamente será rodado connection.Dispose() para fechar a conexão e evitar problemas
        // por padrão essa conexão vem fechada, e nesse caso, não consigo executar nenhum código sql enquanto ela estiver dessa forma
        using var connection = GetConnection();

        // abbrindo a nossa conexão (connection == nosso objeto que contém nossa conexão com o banco de dados SQLite), agora, podemos executar comandos SQL em nosso banco
        connection.Open();

        // comand = connection.CreateCommand() == cria um comando SQl
        // basicamente o método (CreateCommand) do nosso objeto de conexão com o banco de dados (connection) permite que realizemos a criaćão de um comando SQL, o conteúdo de command é a mensagem SQL que iremos enviar ao nosso banco de dados SQLite (vendinha.db) através do nosso objeto de conexão de com o banco (connection) 
        var command = connection.CreateCommand();

        // já declaramos na linha acima (var command = connection.CreateCommand()) que command irá receber comandos SQL, logo: o atributo CommandText (dentro de command, ficando == command.CommandText) definirá QUAL SQL será executado 
        command.CommandText =
        // @ como na linha abaixo, permite escrever texto em múltiplas linhas
        // REAL == números decimais, só que na versão do SQLITE
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

        // executa o SQL dentro de "command" (já que ele que está chamando o método ExecuteNonQuery) no banco vendinha.db (pois command recebeu o comando: CreateCommand a partir do objeto de conexão connection(e objeto de conexão possui vínculo com o banco SQLite: vendinha.db, logo, a exeucão será no banco de dados que atribuiu a permissão de command possuir instrućões SQL == connection que possui vínculo com meu banco de dados: vendinha.sb)
        // NonQuery == não retorna dados, apenas EXECUTA COMANDOS como: (CREATE, INSERT, UPDATE e DELELTE)
        command.ExecuteNonQuery();
    }
}