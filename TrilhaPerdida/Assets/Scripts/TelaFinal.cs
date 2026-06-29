using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TelaFinal : MonoBehaviour
{
    [Header("UI")]
    public Text textoPontuacaoFase1;
    public Text textoPontuacaoFase2;
    public Text textoPontuacaoFase3;
    public Text textoTotal;
    public Text textoEstrelas;
    public Text textoMensagem;

    void Start()
    {
        if (GameManager.Instance == null) return;

        var gm = GameManager.Instance;

        if (textoPontuacaoFase1) textoPontuacaoFase1.text = "Fase 1 — Cabana:   " + gm.pontuacaoFase1 + " pts";
        if (textoPontuacaoFase2) textoPontuacaoFase2.text = "Fase 2 — Pântano:  " + gm.pontuacaoFase2 + " pts";
        if (textoPontuacaoFase3) textoPontuacaoFase3.text = "Fase 3 — Ruínas:   " + gm.pontuacaoFase3 + " pts";

        int total = gm.PontuacaoTotal();
        if (textoTotal) textoTotal.text = "TOTAL:  " + total + " / 300 pts";

        // Estrelas globais
        int estrelas = total >= 270 ? 3 : total >= 180 ? 2 : 1;
        if (textoEstrelas) textoEstrelas.text = new string('★', estrelas) + new string('☆', 3 - estrelas);

        if (textoMensagem)
        {
            textoMensagem.text = total >= 270
                ? "Incrível! Lua salvou Bolota sem nenhum arranhão!"
                : total >= 180
                ? "Muito bem! Bolota está sã e salva."
                : "Bolota foi resgatada, mas a aventura foi difícil!";
        }
    }

    public void JogarNovamente()
    {
        Destroy(GameManager.Instance?.gameObject);
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void Sair()
    {
        Application.Quit();
    }
}
