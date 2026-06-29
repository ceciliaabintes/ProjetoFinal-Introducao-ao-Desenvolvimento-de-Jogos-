using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int pontuacaoFase1 = 0;
    public int pontuacaoFase2 = 0;
    public int pontuacaoFase3 = 0;
    public int tentativasFase1 = 0;
    public int tentativasFase2 = 0;
    public int tentativasFase3 = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public int CalcularPontuacao(int tentativas)
    {
        int erros = tentativas - 1;
        if (erros <= 0) return 100;
        if (erros == 1) return 70;
        if (erros <= 3) return 50;
        return 30;
    }

    public int CalcularEstrelas(int tentativas)
    {
        int erros = tentativas - 1;
        if (erros <= 0) return 3;
        if (erros <= 2) return 2;
        return 1;
    }

    public int PontuacaoTotal() => pontuacaoFase1 + pontuacaoFase2 + pontuacaoFase3;

    public void IrParaCena(string nome) => SceneManager.LoadScene(nome);
    public void ReiniciarCena() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
