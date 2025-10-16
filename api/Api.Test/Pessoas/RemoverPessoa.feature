#language: pt-br

Funcionalidade: Remover pessoa
   Como um usuário qualquer
   Eu quero remover o cadastro de uma pessoa
   Para que não seja mais listado no cadastros

Cenario: remoção com sucesso
   Dado um cadastro com uma pessoa
   | nome   | nascimento | sexo | email            | cpf            | nacionalidade |
   | Fulano | 01/01/2001 | M    | fulano@email.com | 566.247.640-31 | Brasil        |
   Quando remover o cadastro do "Fulano"
   Entao "Fulano" não será mais listado
   E o retorno terá o status code 200

Cenario: registro não encontrado
   Quando remover de um id não existente 
   Entao uma mensagem de pessoa não encontrada
   E o retorno terá o status code 404