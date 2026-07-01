using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public void Loads(int i)
    {
        SceneManager.LoadScene(i);
        //ResetAudio();
    }
    public void Loads(string s)
    {
        SceneManager.LoadScene(s);
        //ResetAudio();
    }
    /*public void ResetAudio()
    {
        AudioManager.Instance.PlayMusic("Theme");
        TouchController.Velocity = 1f;
    }*/
}