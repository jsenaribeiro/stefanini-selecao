# Backend

Projeto WebApi C# RESTful

## Estrutura

### Domain

- Commons
  - Entity
  - IRepository
  - IUnitOfWork

- Pessoas
  - Pessoa
  - Endereco
  - CPF
  - Endereco
  - Nome
  - Email
  - IPessoaRepository

### Infrastructure

- Configurations
  - PessoaConfiguration
  
- Repositories
  - AbstractRepository
  - PessoaRepository

### Application

- Controllers
  - PessoaController
- Extensions
  - ServiceExtensions
  - BuildExtensions
- Program.cs
- appsettings.cs


