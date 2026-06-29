using System.Collections;
using UnityEngine;

// ─── Plataforma que se move entre dois pontos ─────────────────────────────────
[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaMovel : MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB;
    public float velocidade = 2.5f;

    Rigidbody2D rb;
    Transform destino;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        destino = pontoB;
    }

    void FixedUpdate()
    {
        if (!pontoA || !pontoB) return;
        rb.MovePosition(Vector2.MoveTowards(rb.position, destino.position, velocidade * Time.fixedDeltaTime));
        if (Vector2.Distance(rb.position, destino.position) < 0.05f)
            destino = destino == pontoA ? pontoB : pontoA;
    }
}

// ─── Plataforma que some ao pisar ─────────────────────────────────────────────
public class PlataformaDesaparecendo : MonoBehaviour
{
    public float tempoParaSumir   = 1.2f;
    public float tempoParaVoltar  = 3f;

    bool ativada = false;
    SpriteRenderer sr;
    Collider2D col;

    void Start()
    {
        sr  = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (!ativada && c.gameObject.CompareTag("Player"))
        {
            ativada = true;
            StartCoroutine(Sumir());
        }
    }

    IEnumerator Sumir()
    {
        float t = 0f;
        while (t < tempoParaSumir)
        {
            t += Time.deltaTime;
            if (sr) sr.enabled = Mathf.Sin(t * 18f) > 0f;
            yield return null;
        }
        if (col) col.enabled = false;
        if (sr)  sr.enabled  = false;

        yield return new WaitForSeconds(tempoParaVoltar);

        if (col) col.enabled = true;
        if (sr)  sr.enabled  = true;
        ativada = false;
    }
}
