# README

## Arquitetura

- documentação swagger
- versionamento de api
- testes unitários
- banco de dados
- cloud based

## Requisitos

- **Consultar** com filtros o registro de pessoas
- **Cadastrar** registro de pessoa
- **Alterar** registro de pessoa
- **Remover** registro de pessoa

Sobre o versionamento de api

- v1: sem endereço
- v2: com endereço obrigatório

## Modelagens

#### `Entity: Absctract`

| id | dataCadastro | dataAtualizacao |
|-|-|-|
| Guid | DateTime | DateTime |

#### `Pessoa: Entidade`

| nome | sexo | email | nascimento | nacionalidade | cpf |
|-|-|-|-|-|-|
| string | char | string | date | string | string |validated | 

- unicidade: CPF
- validado: CPF, email
- requido: nome, nascimento

## Arquitetura

Arquitetura BFF com microserviço em RESTful API usando conteinerização Docker com docker componse, versionamento git e abordagem DDD.

| app | api | 
|-|-|
| dotnet 8, CQRS lógico, DDD light, RESTful API, Entity Framework Core, Sql Server, Lauers, DDD, BDD, Mediatr, Migrations, Docker | React 17+, react-query, inversifyJS, typescript, vite, swc, biome, json-serve |

## Pendente

- integracao
- cloud
- filtrar
- paginacao
