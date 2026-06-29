using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ─── Bloco individual do pântano ─────────────────────────────────────────────
public class BlocoController : MonoBehaviour
{
    [Header("Tipo")]
    public bool eSeguro = true;

    [HideInInspector] public bool foiPisado = false;

    SpriteRenderer sr;
    Color corOriginal;

    readonly Color COR_SEGURO    = new Color(0.55f, 0.38f, 0.18f);
    readonly Color COR_ARMADILHA = new Color(0.2f,  0.28f, 0.1f);
    readonly Color COR_HOVER     = new Color(0.9f,  0.85f, 0.3f);
    readonly Color COR_PISADO_OK = new Color(0.75f, 0.55f, 0.25f);
    readonly Color COR_PISADO_MAL= new Color(0.8f,  0.1f,  0.1f);

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.color  = eSeguro ? COR_SEGURO : COR_ARMADILHA;
            corOriginal = sr.color;
        }
    }

    void OnMouseEnter() { if (sr && !foiPisado) sr.color = COR_HOVER; }
    void OnMouseExit()  { if (sr && !foiPisado) sr.color = corOriginal; }

    void OnMouseDown()
    {
        if (foiPisado) return;
        FindObjectOfType<PantanoManager>()?.PisarBloco(this);
    }

    public void MarcarPisado()
    {
        foiPisado = true;
        if (sr) sr.color = eSeguro ? COR_PISADO_OK : COR_PISADO_MAL;
    }
}

// ─── Gerenciador da fase 2 ────────────────────────────────────────────────────
public class PantanoManager : MonoBehaviour
{
    [Header("Personagem")]
    public Transform lua;
    public float velocidade = 4f;

    [Header("Bloco de chegada")]
    public Transform blocoFinal;

    [Header("UI")]
    public Text textoFase;
    public Text textoTentativas;
    public Text textoInstrucao;
    public GameObject painelVitoria;

    bool emMovimento = false;
    bool faseOk      = false;

    void Start()
    {
        if (GameManager.Instance != null) GameManager.Instance.tentativasFase2++;
        AtualizarHUD();
        if (textoInstrucao) textoInstrucao.text = "Clique nos blocos para atravessar o pântano!\nCuidado com os blocos escuros!";
    }

    public void PisarBloco(BlocoController bloco)
    {
        if (emMovimento || faseOk) return;
        bloco.MarcarPisado();
        StartCoroutine(MoverLua(bloco));
    }

    IEnumerator MoverLua(BlocoController bloco)
    {
        emMovimento = true;
        if (textoInstrucao) textoInstrucao.text = "";

        Vector3 origem  = lua.position;
        Vector3 destino = bloco.transform.position;
        float   dist    = Vector3.Distance(origem, destino);
        float   t       = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidade / Mathf.Max(dist, 0.1f);
            lua.position = Vector3.Lerp(origem, destino, Mathf.Clamp01(t));
            yield return null;
        }

        lua.position = destino;
        emMovimento  = false;

        if (!bloco.eSeguro)
        {
            yield return StartCoroutine(Afundar());
        }
        else if (blocoFinal && Vector3.Distance(lua.position, blocoFinal.position) < 1.2f)
        {
            yield return StartCoroutine(Vitoria());
        }
    }

    IEnumerator Afundar()
    {
        float t = 0f;
        Vector3 p = lua.position;
        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f;
            lua.position = p + Vector3.down * t * 2f;
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Vitoria()
    {
        faseOk = true;
        if (painelVitoria) painelVitoria.SetActive(true);
        if (GameManager.Instance != null)
            GameManager.Instance.pontuacaoFase2 =
                GameManager.Instance.CalcularPontuacao(GameManager.Instance.tentativasFase2);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Cutscene03");
    }

    void AtualizarHUD()
    {
        if (textoFase) textoFase.text = "Fase 2 — Pântano das Pedras";
        if (textoTentativas && GameManager.Instance != null)
            textoTentativas.text = "Tentativas: " + GameManager.Instance.tentativasFase2;
    }
}
