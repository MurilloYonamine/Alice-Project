# LevelGenerator - Estrutura de Pastas

Este diretório organiza toda a lógica de geração de níveis do projeto. Abaixo está uma explicação de cada pasta e arquivo principal:

## Core
Contém as abstrações e tipos fundamentais para o sistema de geração de níveis.
- **CellTypeSO.cs**: Classe base para todos os tipos de célula (Strategy Pattern).
- **LevelData.cs / LevelRow.cs**: Estruturas que representam os dados do nível e suas linhas.
- **ICellTypeContext.cs**: Interface base para contextos usados nas estratégias.
- **Contexts/**: Contextos específicos para cada tipo de célula (ex: `GroundCellContext`, `EmptyCellContext`, `EnemyCellContext`). Cada contexto carrega apenas os dados necessários para sua estratégia.
- **Services/**: Serviços que encapsulam operações como manipulação de tiles (`TileService`) e spawn de inimigos (`EnemySpawner`). Interfaces e implementações separadas para facilitar testes e desacoplamento.

## Manager
Gerencia o fluxo de geração de níveis.
- **LevelGeneratorManager.cs**: Controla a geração, avanço de níveis, pooling de inimigos e orquestra as estratégias de célula.

## Strategy
Contém as estratégias (tipos de célula) que implementam a lógica de cada tipo de célula do nível.
- **GroundCellTypeSO.cs, EmptyCellTypeSO.cs, EnemyCellTypeSO.cs**: Cada arquivo implementa uma estratégia específica, usando o contexto apropriado.

## Events
Define eventos globais relacionados à geração de níveis.
- **LevelEvents.cs**: Permite disparar e escutar eventos como avanço de chunk.

## Utilities
Funções utilitárias usadas pelo sistema.
- **CoordinateConverter.cs**: Converte coordenadas entre diferentes sistemas de referência do mapa.

## Window
Interfaces e utilitários para o editor Unity.
- **CellTypeCreatorWindow.cs**: Janela para criar novos tipos de célula.
- **LevelWindow.cs / LevelWindowVisuals.cs**: Interface visual para manipular e visualizar níveis no editor.

---

**Padrões Utilizados:**
- **Strategy Pattern:** Cada tipo de célula é uma estratégia que recebe um contexto específico.
- **Context Object Pattern:** Cada estratégia recebe um objeto de contexto com apenas os dados necessários.
- **Service Layer:** Serviços para manipulação de tiles e spawn de inimigos, desacoplados das estratégias.

**Vantagens da Estrutura:**
- Modularidade e fácil extensão.
- Testabilidade e desacoplamento.
- Organização clara para equipes e manutenção futura.
