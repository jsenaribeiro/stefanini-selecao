#language: pt-br

Funcionalidade: Listar pessoas
   Como um usuário qualquer
   Eu quero consultar pessoas
   Para listar seus dados

Cenário: Listar nada
   Quando listar os cadastros
   Entao listará 0 cadastros
   E retornará status 404

Cenario: Listar todos    
   Dado que os seguintes cadastros
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |
   | Beltrano   | 02/02/2002 |
   | Sicrano    | 03/03/2003 |
   Quando listar os cadastros
   Entao listará 3 cadastros
   E retornará status 200
   E conterá os dados
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |
   | Beltrano   | 02/02/2002 |
   | Sicrano    | 03/03/2003 |

Cenário: Listar com filtro parcial
   Dado que os seguintes cadastros
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |
   | Beltrano   | 02/02/2002 |
   | Sicrano    | 03/03/2003 |
   Quando filtra cadastros com "ano"
   Entao listará 3 cadastros
   E retornará status 200
   E conterá os dados
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |
   | Beltrano   | 02/02/2002 |
   | Sicrano    | 03/03/2003 |

Cenário: Listar com filtro total
   Dado que os seguintes cadastros
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |
   | Beltrano   | 02/02/2002 |
   | Sicrano    | 03/03/2003 |
   Quando filtra cadastros com "fulano"
   Entao listará 1 cadastros
   E retornará status 200
   E conterá os dados
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |

Cenário: Listar com paginado
   Dado que os seguintes cadastros
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |
   | Beltrano   | 02/02/2002 |
   | Sicrano    | 03/03/2003 |
   Quando listar com 2 linhas por página
   Entao listará 2 cadastros
   E retornará status 200
   E conterá os dados
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |
   | Beltrano   | 02/02/2002 |

Cenário: Listar ordenado
   Dado que os seguintes cadastros
   | nome       | nascimento |
   | Fulano     | 01/01/2001 |
   | Beltrano   | 02/02/2002 |
   | Sicrano    | 03/03/2003 |
   Quando listar ordenado de modo crescente
   Entao listará 3 cadastros
   E retornará status 200
   E conterá os dados
   | nome       | nascimento |
   | Beltrano   | 02/02/2002 |
   | Fulano     | 01/01/2001 |
   | Sicrano    | 03/03/2003 |