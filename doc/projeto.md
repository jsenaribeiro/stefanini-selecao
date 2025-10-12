# Projeto

Projeto de cadastro de pessoas contendo

- documentação swagger
- versionamento de api
- testes unitários
- banco de dados
- cloud based

Sobre o versionamento de api

- v1: sem endereço
- v2: com endereço obrigatório


# API

O projeto de webapi em .NET 8 C#

## Requisitos

- **Consultar** com filtros o registro de pessoas
- **Cadastrar** registro de pessoa
- **Alterar** registro de pessoa
- **Remover** registro de pessoa

Uma pessoa segue o modelo de pessoa para o backend.

### Entity (abstract class)

| Campo | Tipo | Default |
|-|-|-|
| id | Guid | Guid.Empty
| dataCadastro | DateTime | DateTime.Now |
| dataAtualizacao | DateTime | null |

### Pessoa : Entity

| Campo | Tipo | Opcional | Validação |
|-|-|:-:|:-|
| nome | string | o | não |
| sexo | Enum | x | não |
| email | Email | x | formato |
| nascimento | DateOnly | o | não |
| nacionalidade | string | x | não |
| cpf | CPF | x | formato, unicidade |

# App

Projeto em React Web 17+

* lista das pessoas cadastradas
* adicionar cadastro de pessoa
* remover pessoa do cadastro
* filtro depessoa por nome
* paginar a tabela

