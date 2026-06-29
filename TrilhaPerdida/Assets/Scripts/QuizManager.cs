using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [Header("UI Pergunta")]
    public Text textoPergunta;
    public Button[] botoes = new Button[3];
    public Text[]   textosBotoes = new Text[3];

    [Header("UI Feedback")]
    public GameObject painelFeedback;
    public Text       textoFeedback;
    public Image      imagemFeedback; // verde/vermelho

    [Header("UI HUD")]
    public Text textoFase;
    public Text textoProgresso;   // "Pergunta 1 de 3"
    public Text textoTentativas;

    [Header("Cores")]
    public Color corCerta   = new Color(0.2f, 0.8f, 0.3f);
    public Color corErrada  = new Color(0.9f, 0.2f, 0.2f);
    public Color corNormal  = new Color(0.25f, 0.55f, 0.3f);
    public Color corHover   = new Color(0.4f, 0.75f, 0.45f);

    // ─── Banco de perguntas ───────────────────────────────────────────────────
    readonly string[] perguntas = {
        "Qual animal tem o melhor olfato na floresta?",
        "O que exploradores usam para não se perder?",
        "Que planta indica que há água por perto?",
    };

    readonly string[][] opcoes = {
        new[]{ "Águia", "Urso", "Cachorro" },
        new[]{ "Celular", "Bússola", "Lanterna" },
        new[]{ "Cacto",  "Margarida", "Musgo" },
    };

    readonly int[] corretas = { 2, 1, 2 }; // índice 0=A,1=B,2=C
    // ─────────────────────────────────────────────────────────────────────────

    int perguntaAtual = 0;
    bool bloqueado    = false;

    void Start()
    {
        if (GameManager.Instance != null) GameManager.Instance.tentativasFase1++;

        for (int i = 0; i < botoes.Length; i++)
        {
            int idx = i; // captura para lambda
            botoes[i].onClick.AddListener(() => Responder(idx));
        }

        AtualizarHUD();
        CarregarPergunta(0);
    }

    void CarregarPergunta(int idx)
    {
        bloqueado = false;
        painelFeedback.SetActive(false);

        textoPergunta.text = perguntas[idx];
        for (int i = 0; i < 3; i++)
        {
            textosBotoes[i].text = new[]{"A","B","C"}[i] + ")  " + opcoes[idx][i];
            var img = botoes[i].GetComponent<Image>();
            if (img) img.color = corNormal;
        }

        if (textoProgresso) textoProgresso.text = $"Pergunta {idx + 1} de {perguntas.Length}";
    }

    void Responder(int escolha)
    {
        if (bloqueado) return;
        bloqueado = true;

        bool acertou = (escolha == corretas[perguntaAtual]);

        // Colore botão escolhido
        var img = botoes[escolha].GetComponent<Image>();
        if (img) img.color = acertou ? corCerta : corErrada;

        // Se errou, mostra a certa também
        if (!acertou)
        {
            var imgCerta = botoes[corretas[perguntaAtual]].GetComponent<Image>();
            if (imgCerta) imgCerta.color = corCerta;
        }

        // Feedback
        painelFeedback.SetActive(true);
        if (imagemFeedback) imagemFeedback.color = acertou ? corCerta : corErrada;

        if (acertou)
        {
            textoFeedback.text = perguntaAtual < perguntas.Length - 1
                ? "✔  Correto!  Próxima pergunta..."
                : "✔  Parabéns! Porta desbloqueada!\nBolota está mais fundo na floresta...";
            StartCoroutine(acertou && perguntaAtual >= perguntas.Length - 1
                ? FinalizarFase()
                : ProximaPergunta());
        }
        else
        {
            textoFeedback.text = "✘  Errou! Lua caiu na armadilha...\nRecomeçando!";
            StartCoroutine(Reiniciar());
        }
    }

    IEnumerator ProximaPergunta()
    {
        yield return new WaitForSeconds(1.5f);
        perguntaAtual++;
        CarregarPergunta(perguntaAtual);
    }

    IEnumerator Reiniciar()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FinalizarFase()
    {
        yield return new WaitForSeconds(2.5f);
        if (GameManager.Instance != null)
            GameManager.Instance.pontuacaoFase1 =
                GameManager.Instance.CalcularPontuacao(GameManager.Instance.tentativasFase1);
        SceneManager.LoadScene("Cutscene02");
    }

    void AtualizarHUD()
    {
        if (textoFase)       textoFase.text = "Fase 1 — Cabana dos Sábios";
        if (textoTentativas && GameManager.Instance != null)
            textoTentativas.text = "Tentativas: " + GameManager.Instance.tentativasFase1;
    }
}
