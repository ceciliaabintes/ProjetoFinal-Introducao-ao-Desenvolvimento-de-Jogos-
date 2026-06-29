using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jogar()    => SceneManager.LoadScene("Cutscene01");
    public void Sair()     { Application.Quit(); Debug.Log("Saindo..."); }
    public void Creditos() => SceneManager.LoadScene("Creditos");
}
