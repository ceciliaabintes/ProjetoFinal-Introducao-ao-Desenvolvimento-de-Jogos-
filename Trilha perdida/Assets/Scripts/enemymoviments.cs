using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoAleatorio : MonoBehaviour
{
    public float velocidade = 4.0f; 
    public float tempoMinimoEspera = 2.0f; 
    public float tempoMaximoEspera = 3.0f; 

    private float direcao = 1.0f; 
    private float tempoEspera;
    private Vector3 escalaOriginal;
    private void Start()
    {
        tempoEspera = Random.Range(tempoMinimoEspera, tempoMaximoEspera);
        escalaOriginal = transform.localScale;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * direcao * velocidade * Time.deltaTime);

        
        tempoEspera -= Time.deltaTime;

        if (tempoEspera <= 0)
        {
            direcao *= -1;
            Vector3 novaEscala = transform.localScale;
            novaEscala.x = escalaOriginal.x * direcao;
            transform.localScale = novaEscala;
            tempoEspera = Random.Range(tempoMinimoEspera, tempoMaximoEspera);
        }
    }
}

