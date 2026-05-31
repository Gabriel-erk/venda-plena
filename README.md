# Vendinha Plena

## Descrição

Sistema desenvolvido em C# para controle de clientes e dívidas de uma vendinha.

O projeto permite cadastrar clientes, registrar dívidas, controlar pagamentos e consultar informações armazenadas em banco de dados SQLite.

## Funcionalidades

### Clientes
- Cadastrar cliente
- Listar clientes
- Buscar cliente
- Atualizar cliente
- Remover cliente

### Dívidas
- Cadastrar dívida
- Listar dívidas de um cliente
- Marcar dívida como paga
- Remover dívida

## Regras de Negócio

- CPF não pode ser duplicado.
- Um cliente pode possuir apenas uma dívida em aberto.
- A idade é calculada automaticamente a partir da data de nascimento.
- O e-mail é validado quando informado.

## Tecnologias Utilizadas

- C#
- .NET 8
- SQLite
- Programação Orientada a Objetos (POO)

## Como Executar

Instalar as dependências:

```bash
dotnet restore
dotnet add package Microsoft.Data.Sqlite
```

Executar o projeto:

```bash
dotnet run
```

## Estrutura do Projeto

```text
Models/
Services/
Database/
Enums/
Program.cs
```

## Autor

Projeto desenvolvido para fins acadêmicos na disciplina de Programação Orientada a Objetos.
