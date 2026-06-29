# 🌿 TRILHA PERDIDA — Guia de Montagem no Unity
## Versão Unity necessária: 2022.3.x LTS (gratuita em unity.com/download)

---

## 📦 PASSO 1 — Criar o Projeto

1. Abra o Unity Hub
2. Clique em **New Project**
3. Escolha o template **2D (Core)**
4. Nome: `TrilhaPerdida`
5. Clique em **Create project**

---

## 📁 PASSO 2 — Copiar os Scripts

1. No Unity, dentro da aba **Project**, abra a pasta `Assets`
2. Crie uma pasta chamada `Scripts`
3. Copie todos os arquivos `.cs` desta pasta para `Assets/Scripts`

---

## 🎬 PASSO 3 — Criar as Cenas

Vá em **File > New Scene** e crie (e salve) cada cena abaixo:

| Nome da Cena     | Descrição                          |
|------------------|------------------------------------|
| `MenuPrincipal`  | Tela inicial com botão Jogar       |
| `Cutscene01`     | Introdução antes da Fase 1         |
| `Fase1_Quiz`     | A Cabana dos Sábios                |
| `Cutscene02`     | Transição Fase 1 → 2               |
| `Fase2_Pantano`  | O Pântano das Pedras               |
| `Cutscene03`     | Transição Fase 2 → 3               |
| `Fase3_Parkour`  | As Ruínas do Parkour               |
| `TelaFinal`      | Pontuação final                    |

Depois vá em **File > Build Settings**, clique em **Add Open Scenes**
para cada uma delas (ou arraste da aba Project).

---

## 🏠 PASSO 4 — Montar a Cena: MenuPrincipal

### GameObjects necessários:

1. **GameObject vazio** → renomear para `GameManager`
   - Arraste o script `GameManager.cs`

2. **Canvas** (UI > Canvas)
   - Dentro do Canvas crie:
     - `Image` → renomear `FundoMenu` → cor verde escura ou imagem de floresta
     - `Text` → "TRILHA PERDIDA" (grande, centralizado)
     - `Button` → renomear `BotaoJogar` → texto "▶ JOGAR"
     - `Button` → renomear `BotaoSair` → texto "✕ SAIR"

3. **GameObject vazio** → renomear `MenuManager`
   - Arraste o script `MenuPrincipal.cs`
   - No botão **BotaoJogar**, seção OnClick → arraste MenuManager → função `Jogar()`
   - No botão **BotaoSair**, OnClick → `Sair()`

---

## 📖 PASSO 5 — Montar as Cenas de Cutscene (01, 02, 03)

Para cada cutscene:

1. **Canvas**
   - `Image` → `FundoImagem` (tela inteira, cor escura ou sprite)
   - `Text` → `TextoNarracao` (parte inferior da tela)
   - `Text` → `TextoAvancar` (pequeno, canto inferior direito)

2. **GameObject vazio** → `CutsceneManager`
   - Arraste `CutsceneController.cs`
   - Conecte os campos no Inspector:
     - `imagemFundo` → FundoImagem
     - `textoNarracao` → TextoNarracao
     - `textoAvancar` → TextoAvancar
     - `nomeCenaProxima` → nome da próxima cena (ex: "Fase1_Quiz")
   - **Textos sugeridos para Cutscene01:**
     - Quadro 1: "Lua caminha pela floresta em busca de sua amiga Bolota..."
     - Quadro 2: "Uma voz ecoa: 'Para avançar, você deve provar seu conhecimento!'"
     - Quadro 3: "Lua respira fundo e entra na Cabana dos Sábios."

---

## ❓ PASSO 6 — Montar a Cena: Fase1_Quiz

### Hierarquia de GameObjects:

```
Canvas
  ├── Fundo (Image - cor verde musgo)
  ├── TextoFase (Text)
  ├── TextoTentativas (Text)
  ├── PainelPergunta (Panel)
  │   ├── TextoPergunta (Text - grande, centralizado)
  │   ├── BotaoA (Button)
  │   │   └── TextoBotaoA (Text)
  │   ├── BotaoB (Button)
  │   │   └── TextoBotaoB (Text)
  │   └── BotaoC (Button)
  │       └── TextoBotaoC (Text)
  └── PainelFeedback (Panel - começa DESATIVADO)
      ├── ImagemFeedback (Image)
      └── TextoFeedback (Text)

QuizManager (GameObject vazio)
```

### Configurar o script QuizManager:
- Arraste `QuizManager.cs` no `QuizManager` GameObject
- No Inspector, conecte cada campo:
  - `textoPergunta` → TextoPergunta
  - `botoes[0,1,2]` → BotaoA, BotaoB, BotaoC
  - `textosBotoes[0,1,2]` → TextoBotaoA, TextoBotaoB, TextoBotaoC
  - `painelFeedback` → PainelFeedback
  - `textoFeedback` → TextoFeedback
  - `textoFase` → TextoFase
  - `textoTentativas` → TextoTentativas

---

## 🐸 PASSO 7 — Montar a Cena: Fase2_Pantano

### Hierarquia:

```
Cenário
  ├── BlocoInicio (Sprite quadrado, cor cinza claro)
  ├── Bloco1 (Sprite quadrado)
  ├── Bloco2 (Sprite quadrado)
  ... (total ~12 blocos dispostos em grade)
  └── BlocoFinal (Sprite, outro lado do pântano)

Lua (Sprite círculo ou quadrado representando a personagem)

Canvas
  ├── TextoFase
  ├── TextoTentativas
  ├── TextoInstrucao
  └── PainelVitoria (Panel - começa DESATIVADO)
      └── TextoVitoria (Text - "Parabéns!")

PantanoManager (GameObject vazio)
```

### Configurar os blocos:
- Cada bloco: adicione `BoxCollider2D` + script `BlocoController.cs`
- Marque `eSeguro = true` em ~60% dos blocos, `false` nos demais
- **Dica:** organize em uma grade 4x3 para o jogador ter escolha

### Configurar PantanoManager:
- Arraste `PantanoManager.cs`
- `lua` → Transform da Lua
- `blocoFinal` → Transform do BlocoFinal
- Conecte os textos de UI

---

## 🏃 PASSO 8 — Montar a Cena: Fase3_Parkour

### Criar a Layer "Ground":
1. Selecione qualquer plataforma
2. No Inspector, canto superior direito: **Layer > Add Layer**
3. Crie a layer `Ground`
4. Aplique em todas as plataformas

### Hierarquia:

```
Plataformas/
  ├── PlataformaInicio (Sprite retângulo)
  ├── Plataforma1..N
  ├── PlataformaMeta (com Tag "Meta")
  └── PlataformaMovel1 (com script PlataformaMovel.cs)
      ├── PontoA (Transform vazio)
      └── PontoB (Transform vazio)

ZonaMorte (Sprite longo, transparente, abaixo das plataformas)
  └── Tag: "ZonaMorte"

Lua
  ├── Script: LuaController.cs
  ├── Rigidbody2D (Gravity Scale: 3)
  ├── CapsuleCollider2D
  ├── SpriteRenderer
  └── VerificadorChao (filho vazio, posicionado nos pés)

Canvas
  ├── TextoFase
  ├── TextoTentativas
  ├── TextoMortes
  └── PainelVitoria (desativado)

ParkourManager (GameObject vazio)
  └── Script: ParkourManager.cs
```

### Configurar LuaController:
- `verificadorChao` → VerificadorChao (filho)
- `camadaChao` → selecionar layer Ground
- `parkourManager` → ParkourManager GameObject

### Tags necessárias (Edit > Project Settings > Tags):
- `Player` → aplicar na Lua
- `ZonaMorte` → aplicar nas zonas de morte
- `Meta` → aplicar na plataforma final
- `Checkpoint` → aplicar em plataformas opcionais de checkpoint

### Layout sugerido das plataformas:
```
[INICIO]  [P1]        [P2]  [P3]
                  [P4]      [P5]  [P6]
     [P7 MOVEL]       [P8]       [META]
```

---

## 🏆 PASSO 9 — Montar a Cena: TelaFinal

```
Canvas
  ├── Fundo (Image)
  ├── Titulo (Text - "FIM DA AVENTURA!")
  ├── TextoPontuacaoFase1 (Text)
  ├── TextoPontuacaoFase2 (Text)
  ├── TextoPontuacaoFase3 (Text)
  ├── TextoTotal (Text)
  ├── TextoEstrelas (Text)
  ├── TextoMensagem (Text)
  ├── BotaoJogarNovamente (Button)
  └── BotaoSair (Button)

TelaFinalManager (GameObject vazio)
  └── Script: TelaFinal.cs
```

Conecte tudo no Inspector e nos botões chame `JogarNovamente()` e `Sair()`.

---

## ▶️ PASSO 10 — Testar!

1. Abra a cena `MenuPrincipal`
2. Pressione **Play (▶)** no topo do Unity
3. Clique em Jogar e siga o fluxo!

---

## 🎨 Dicas visuais rápidas (sem artista):

- Use `Sprites > Square` embutido do Unity e mude as cores no Inspector
- Para a Lua: círculo branco/azul claro
- Para plataformas: retângulos marrons ou cinzas
- Para blocos do pântano: quadrados com cores diferentes (marrom=seguro, verde escuro=armadilha)
- Fundo das cenas: `Camera > Background Color` em verde escuro ou azul noite

---

## 📋 Resumo dos Scripts e onde usar:

| Script                | Cena                | Objeto           |
|-----------------------|---------------------|------------------|
| GameManager.cs        | MenuPrincipal       | GameManager      |
| MenuPrincipal.cs      | MenuPrincipal       | MenuManager      |
| CutsceneController.cs | Cutscene01/02/03    | CutsceneManager  |
| QuizManager.cs        | Fase1_Quiz          | QuizManager      |
| BlocoController.cs    | Fase2_Pantano       | Cada bloco       |
| PantanoManager.cs     | Fase2_Pantano       | PantanoManager   |
| LuaController.cs      | Fase3_Parkour       | Lua              |
| ParkourManager.cs     | Fase3_Parkour       | ParkourManager   |
| Plataformas.cs        | Fase3_Parkour       | Plataformas      |
| TelaFinal.cs          | TelaFinal           | TelaFinalManager |

---

Boa sorte, e qualquer dúvida é só perguntar! 🌿
