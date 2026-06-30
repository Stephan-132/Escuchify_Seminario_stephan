Feature: Actualización automática de biografía

  Scenario: Obtener biografía de un artista existente
    Given que el artista "Charly García" existe en mi base de datos
    When solicito actualizar su biografía desde Wikipedia
    Then la biografía debería ser distinta de "No se encontró biografía en Wikipedia."