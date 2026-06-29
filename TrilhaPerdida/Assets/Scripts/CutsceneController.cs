using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [Header("UI")]
    public Image imagemFundo;
    public Text textoNarracao;
    public Text textoAvancar;

    [Header("Conteudo")]
    public Sprite[] quadros;
    [TextArea(2,5)] public string[] textos;
    public string nomeCenaProxima;

    private int atual = 0;

    void Start()
    {
        if (textoAvancar != null)
            textoAvancar.text = "[ Clique ou SPACE para continuar ]";
        Mostrar(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            Avancar();
    }

    public void Avancar()
    {
        atual++;
        if (atual >= Mathf.Max(quadros != null ? quadros.Length : 0,
                               textos  != null ? textos.Length  : 0))
            SceneManager.LoadScene(nomeCenaProxima);
        else
            Mostrar(atual);
    }

    void Mostrar(int i)
    {
        if (imagemFundo  != null && quadros != null && i < quadros.Length)
            imagemFundo.sprite = quadros[i];
        if (textoNarracao != null && textos  != null && i < textos.Length)
            textoNarracao.text = textos[i];
    }
}
