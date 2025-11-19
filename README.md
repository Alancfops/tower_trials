# Tower Trials - Game Design Document (GDD)

## Versão: 1.0

### Autores:
- Alan Cristian
- João Alberto da Silva
- Pedro Henrique Rosa Cruz

### Localização: 
Maceió – AL  
2025.2

---

## Visão Geral

**Tower Trials** é um jogo de ação em 2D onde o jogador controla Valen, um jovem guerreiro determinado a provar seu valor enfrentando uma torre enigmática repleta de monstros e desafios. Cada andar da torre é uma batalha mais difícil, culminando em um confronto épico contra o próprio fundador do reino, Arkan Draven.

---

## História

Na cidade de Castelbravo, ergue-se a Torre da Noite Eterna, um lugar cheio de mistérios e monstros. Valen Draven, um jovem desacreditado, decide desafiar a torre para descobrir o legado do maior guerreiro de todos os tempos — seu avô, Arkan Draven. À medida que sobe pela torre, ele enfrenta inimigos e chefes, cada um mais perigoso que o anterior, até chegar ao topo onde enfrentará seu próprio destino.

---

## Gameplay

### Loop central
O jogo segue um ciclo simples de exploração e combate:
- **Exploração**: Avançar pelos andares da torre.
- **Combate**: Derrotar inimigos para ganhar XP e aumentar de nível.
- **Desafios**: Enfrentar chefes de andar e, finalmente, o chefe final para completar a jornada.

### Estrutura dos Andares
- **Andares 1 e 2**: Grupos de inimigos menores.
- **Andar 3 (Guardião Perdido) e Andar Final (Arkan)**: Encontros com chefes sem "adds".

---

## Personagens

### Valen Draven (Herói)
- **Habilidade**: Movimentação horizontal e combate com espada.
- **Motivação**: Provar-se digno do legado de sua família, enfrentando a Torre da Noite Eterna.
- **Inspiração**: Solo Leveling, Clash Royale.

### Arkan Draven (Inimigo final)
- **Habilidade**: Golpes poderosos com espada pesada.
- **História**: O fundador de Castelbravo, agora corrompido e transformado no chefe final da torre.

---

## Controles

### Teclado
- **Movimento**: A/D (esquerda/direita)
- **Pular**: W
- **Ataque Normal**: Barra de espaço (combo de até 3 golpes)
- **Golpe Especial 1**: L (requere 3 ataques consecutivos)
- **Golpe Especial 2**: Shift + Espaço (requere 5 ataques consecutivos)

---

## Câmera

A câmera utiliza **Cinemachine 2D**, acompanhando o personagem Valen em tempo real com uma Dead Zone curta e Confiner por sala. Durante as lutas contra chefes, a câmera trava na arena de combate, oferecendo uma visão mais controlada.

---

## Universo do Jogo

### Ambiente
O jogo é ambientado em uma torre medieval sombria, com andares que aumentam em dificuldade e desafios. Cada andar possui sua própria atmosfera e chefes únicos, com visuais e sons distintos.

---

## Inimigos

### Menores
- **Goblin das Sombras**: Inimigo rápido que ataca em grupo.
- **Pequeno Golem**: Bloqueia passagens e possui grande resistência.

### Chefes
1. **Rei Goblin** – Duelista ágil com adagas.
2. **Golem de Pedra** – Lento, mas muito resistente.
3. **Guardião Perdido** – Espadachim corrompido.
4. **Arkan Draven** – O fundador de Castelbravo e chefe final.

---

## Interface (HUD)

### Elementos
- **Topo esquerdo**: Barra de vida do jogador.
- **Topo direito**: Pontuação.
- **Centro superior**: Barra de vida do chefe durante lutas.

### Menus principais
1. **Tela inicial**: "Iniciar / Opções / Sair"
2. **Tela de pausa**: Opções de Retomar / Voltar ao Menu

---

## Itens

### Do personagem
- **Espada**: Arma principal que evolui conforme Valen sobe de nível.
- **Armadura**: Proteção básica do personagem.

### Do jogo
- **Poção de Cura**: Restaura parte da vida do personagem.
- **Orbe de Experiência**: Concede pontos de XP.

---

## Informações gerais

- **Plataforma inicial**: PC (Windows), com suporte para teclado e joystick.
- **Engine**: Unity 2D.
- **Estilo de jogo**: Ação medieval com mecânicas de progressão por andares.
- **Público-alvo**: Jogadores casuais e intermediários, faixa etária 12+.
