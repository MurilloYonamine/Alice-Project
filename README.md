# 🐰 Alice Project - Down the Rabbit Hole

Um jogo de plataforma vertical inspirado em **Downwell**, onde o jogador controla Alice em sua queda pelo buraco da toca do coelho, vivenciando toda a história clássica de "Alice no País das Maravilhas" durante a descida.

## 🎮 Conceito do Jogo

Alice cai infinitamente pelo buraco do coelho enquanto:
- **Desvia de obstáculos** e armadilhas
- **Enfrenta inimigos** inspirados nos personagens do livro
- **Experiencia a narrativa** completa de Lewis Carroll
- **Explora níveis gerados** proceduralmente

## 🏗️ Arquitetura do Projeto

### 📁 Estrutura Principal

```
Assets/
├── _MAIN/                   # Sistema principal do jogo
│   ├── Scenes/              # Cenas principais
│   ├── Scripts/             # Scripts core do gameplay
│   └── Resources/           # Recursos carregáveis
│
├── _TESTING/                # Ambiente de testes e prototipagem
│   ├── Scripts/             # Scripts experimentais
│   └── Tilemap/             # Testes de tilemap
│
├── DialogueSystem/          # Sistema de diálogos narrativos
│   ├── _MAIN/               # Sistema principal de diálogos
│   └── _TESTING/            # Testes do sistema de diálogo
│
└── LevelGenerator/          # Geração procedural de níveis
    ├── Data/                # Assets e dados dos níveis
    ├── Editor/              # Ferramentas de editor
    └── Window/              # Interface do level designer
```

## 🔧 Sistemas Implementados

### 🎭 Sistema de Diálogos
- **DialogueSystem**: Gerencia narrativa e conversas
- Integração com a história de Alice
- Sistema modular para diferentes personagens

### 🗺️ Gerador de Níveis
- **LevelGenerator**: Criação procedural de níveis verticais
- **LevelData**: Estruturas de dados para níveis
- **SquareStates**: Estados dos tiles (Empty, Structure, Enemy)
- **SquareRow**: Organização em linhas para descida vertical

## 🎯 Mecânicas Principais

### 🏃‍♀️ Gameplay Core
- **Movimento vertical** contínuo (queda)
- **Controle horizontal** para navegação
- **Sistema de colisão** com obstáculos e inimigos
- **Progressão narrativa** integrada ao gameplay

### 🌍 Geração Procedural
- Níveis verticais infinitos
- Colocação inteligente de obstáculos
- Spawn balanceado de inimigos
- Integração com marcos narrativos

### 📚 Narrativa Interativa
- História inteira contada durante a queda
- Diálogos contextuais com personagens
- Eventos narrativos baseados na profundidade

## 🔮 Recursos Futuros

- [ ] Sistema de power-ups temporários
- [ ] Boss fights com personagens icônicos
- [ ] Modo história vs. modo infinito

## 👥 Desenvolvimento

**Status**: Em desenvolvimento ativo  
**Plataforma alvo**: PC/Mobile  
**Estilo**: Arcade/Narrative  

# ⭐ TO BE CONTINUED... ⭐