#language: pt-br

Funcionalidade: Alterar registro de pessoa
   Como um usuário qualquer
   Eu quero alterar o cadastro de uma pessoa
   Para que a mudança seja listado no cadastros

Esquema do Cenario: alteração com sucesso
   Dado uma pessoa cadastrada com
   | nome   | nascimento | sexo | email            | cpf            | nacionalidade |
   | Fulano | 01/01/2001 | M    | fulano@email.com | 566.247.640-31 | Brasil        |
   Quando alterar o cadastro do "Fulano" com <campo> = <valor>
   Entao o cadastro do cenário terá o <campo> = <valor>
   E retornará o status code 200

Exemplos:
   | campo           | valor                | 
   | "Sexo"          | "F"                  |
   | "Nome"          | "Beltrana"           |
   | "Nascimento"    | "02/02/2002"         |
   | "Email"         | "beltrano@email.com" |
   | "CPF"           | "455.767.800-94"     |
   | "Nacionalidade" | "Aegentina"          |

Esquema do Cenario: alteração com fracasso
   Dado uma pessoa cadastrada com
   | nome     | nascimento | sexo | email              | cpf            | nacionalidade |
   | Fulano   | 01/01/2001 | M    | fulano@email.com   | 566.247.640-31 | Brasil        |
   | Beltrano | 02/02/2002 | M    | beltrano@email.com | 455.767.800-94 | Brasil        |
   Quando alterar o cadastro do "Fulano" com <campo> = <valor>
   Entao retornará a mensagem de erro de <excecao>
   E retornará o status code 400

Exemplos:
   | campo         | valor                | excecao       |
   | "Nome"        | ""                   | "obrigatorio" |
   | "Nascimento"  | ""                   | "obrigatorio" |
   | "Nascimento"  | "00/00/0000"         | "invalido"    |
   | "Sexo"        | "A"                  | "invalido"    |
   | "Email"       | "beltrano_email.com" | "invalido"    |
   | "CPF"         | "000.000.000-00"     | "invalido"    |
   | "CPF          | "455.767.800-94"     | "duplicidade" |

Cenário: pessoa não encontrada
   Quando alterar o cadastro com um id não cadastrado
   Então retornará uma mensagem de erro de pessoa não encontrada
   E retornará o status code 404