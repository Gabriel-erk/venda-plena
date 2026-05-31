# Vendinha Plena

## Descrição

O Vendinha Plena é um sistema desenvolvido em C# utilizando .NET e SQLite para auxiliar no controle de clientes e dívidas de uma pequena venda.

O sistema permite cadastrar clientes, registrar dívidas, controlar pagamentos e consultar informações de forma rápida e organizada.

---

## Funcionalidades

### Clientes

- Cadastrar cliente
- Listar clientes
- Buscar cliente por ID
- Atualizar cliente
- Remover cliente

### Dívidas

- Cadastrar dívida
- Listar dívidas de um cliente
- Marcar dívida como paga
- Remover dívida

---

## Regras de Negócio

- O CPF do cliente deve ser único.
- Um cliente não pode possuir mais de uma dívida em aberto.
- A idade do cliente é calculada automaticamente a partir da data de nascimento.
- O e-mail é validado quando informado.
- As dívidas possuem situação:
  - Pendente
  - Paga
- Ao marcar uma dívida como paga, a data de pagamento é registrada automaticamente.

---

## Tecnologias Utilizadas

- C#
- .NET 8
- SQLite
- Programação Orientada a Objetos (POO)
- DataAnnotations para validações

---

## Estrutura do Projeto

```text
VendinhaPlena/
│
├── Database/
│   └── DatabaseConfig.cs
│
├── Enums/
│   └── SituacaoDivida.cs
│
├── Models/
│   ├── Cliente.cs
│   └── Divida.cs
│
├── Services/
│   ├── ClienteService.cs
│   └── DividaService.cs
│
├── Program.cs
├── schema.sql
└── vendinha.db
```

## Banco de Dados

O sistema utiliza SQLite para armazenamento dos dados.

Tabelas:

### clientes

| Campo | Tipo |
|---------|---------|
| id | INTEGER |
| nome | TEXT |
| cpf | TEXT |
| data_nascimento | TEXT |
| email | TEXT |

### dividas

| Campo | Tipo |
|---------|---------|
| id | INTEGER |
| cliente_id | INTEGER |
| valor | REAL |
| situacao | INTEGER |
| data_criacao | TEXT |
| data_pagamento | TEXT |

---

## Como Executar

### Clonar o projeto

```bash
git clone URL_DO_REPOSITORIO
```

### Entrar na pasta

```bash
cd VendinhaPlena
```

### Restaurar dependências

```bash
dotnet restore
```

### Executar o projeto

```bash
dotnet run
```

---

## Dependências

Instalar o pacote SQLite:

```bash
dotnet add package Microsoft.Data.Sqlite
```

---

## Conceitos Aplicados

- Classes e Objetos
- Encapsulamento
- Enumerações
- Serviços
- Validação de dados
- CRUD
- Banco de Dados
- Relacionamento entre entidades
- Programação Orientada a Objetos
- Persistência de Dados

---

## Autor

Projeto desenvolvido para a disciplina de Programação Orientada a Objetos utilizando C# e .NET.
