#language: pt-br

Funcionalidade: Cadastrar pessoa
   Como um usuário qualquer
   Eu quero cadastrar pessoa
   Para que seja listado no cadastros

Cenario: cadastrar com sucesso parcial
   Dado uma pessoa com dados mínimos de
   | nome   | nascimento |
   | Fulano | 01/01/2001 |
   Quando cadastrar a pessoa
   Entao terá cadastrado
   | nome   | nascimento |
   | Fulano | 01/01/2001 |

Cenario: cadastrar com sucesso completo
   Dado uma pessoa com
   | nome   | nascimento | sexo | email            | cpf            | nacionalidade |
   | Fulano | 01/01/2001 | M    | fulano@email.com | 566.247.640-31 | Brasil        |
   Quando cadastrar a pessoa
   Entao terá cadastrado
   | nome   | nascimento | sexo | email            | cpf            | nacionalidade |
   | Fulano | 01/01/2001 | M    | fulano@email.com | 566.247.640-31 | Brasil        |

Esquema do Cenario: Campos inválidos
   Dado uma pessoa com dados mínimos de
   | nome   | nascimento | 
   | Fulano | 01/01/2001 | 
   E cujo <campo> é <valor>
   Quando cadastrar a pessoa
   Então retornará erro de <campo> "inválido"

   Exemplos:
      | campo        | valor                |
      | "CPF"        | "000.000.000-00"     |
      | "Nascimento" | "00/00/0000"         |
      | "Email"      | "email-invalido.com" |


Cenario: campos obrigatórios
   Dado uma pessoa com dados mínimos de
   | nome | nascimento |
   |      |            |
   Quando cadastrar a pessoa
   Então retornará erro de "Nome" "obrigatório"
   E retornará erro de "Nascimento" "obrigatório"