using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParkourManager : MonoBehaviour
{
    [Header("UI")]
    public Text textoFase;
    public Text textoTentativas;
    public Text textoMortes;
    public GameObject painelVitoria;
    public GameObject painelMorte;

    Vector3 checkpointAtual;
    int     mortes = 0;
    bool    concluido = false;

    void Start()
    {
        if (GameManager.Instance != null) GameManager.Instance.tentativasFase3++;
        checkpointAtual = FindObjectOfType<LuaController>()?.transform.position ?? Vector3.zero;
        AtualizarHUD();
    }

    public void MarcarCheckpoint(Vector3 pos)
    {
        checkpointAtual = pos;
        Debug.Log("Checkpoint salvo: " + pos);
    }

    public Vector3 UltimoCheckpoint() => checkpointAtual;

    public void RegistrarMorte()
    {
        mortes++;
        if (textoMortes) textoMortes.text = "Quedas: " + mortes;
        if (painelMorte) StartCoroutine(MostrarPainelMorte());
    }

    IEnumerator MostrarPainelMorte()
    {
        if (painelMorte) painelMorte.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (painelMorte) painelMorte.SetActive(false);
    }

    public void FaseCompleta()
    {
        if (concluido) return;
        concluido = true;

        if (GameManager.Instance != null)
        {
            int tentativas = mortes + 1;
            GameManager.Instance.tentativasFase3 = tentativas;
            GameManager.Instance.pontuacaoFase3 =
                GameManager.Instance.CalcularPontuacao(tentativas);
        }

        if (painelVitoria) painelVitoria.SetActive(true);
        StartCoroutine(IrParaFim());
    }

    IEnumerator IrParaFim()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("TelaFinal");
    }

    void AtualizarHUD()
    {
        if (textoFase) textoFase.text = "Fase 3 — Ruínas do Parkour";
        if (textoTentativas && GameManager.Instance != null)
            textoTentativas.text = "Tentativas: " + GameManager.Instance.tentativasFase3;
        if (textoMortes) textoMortes.text = "Quedas: 0";
    }
}
