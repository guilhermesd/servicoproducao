Feature: Registrar status do pedido de produção
  Como um sistema de produção
  Quero registrar o status de um pedido
  Para que eu possa acompanhar seu progresso

  Scenario: Registrar status de produção com sucesso
    Given que tenho um pedido válido com status "EmPreparacao"
    When envio a requisição para registrar o status de produção
    Then a resposta deve retornar status 200 OK
