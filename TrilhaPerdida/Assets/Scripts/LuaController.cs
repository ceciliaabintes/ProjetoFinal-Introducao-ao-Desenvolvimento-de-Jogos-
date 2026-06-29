using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class LuaController : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade    = 5.5f;
    public float forcaPulo     = 13f;
    public float gravidade     = 3.5f;    // multiplicador de gravidade

    [Header("Chao")]
    public Transform verificadorChao;
    public float     raioChao  = 0.18f;
    public LayerMask camadaChao;

    [Header("Refs")]
    public ParkourManager parkourManager;

    Rigidbody2D     rb;
    SpriteRenderer  sr;
    bool noChao     = false;
    bool morreu     = false;
    bool podePular  = true;        // coyote time simples

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = gravidade;
    }

    void Update()
    {
        if (morreu) return;

        // ─── Verificação de chão ───────────────────────────────────────────
        noChao = Physics2D.OverlapCircle(verificadorChao.position, raioChao, camadaChao);

        // ─── Movimento horizontal ──────────────────────────────────────────
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * velocidade, rb.velocity.y);
        if (h > 0.01f) sr.flipX = false;
        if (h < -0.01f) sr.flipX = true;

        // ─── Pulo ──────────────────────────────────────────────────────────
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)
          || Input.GetKeyDown(KeyCode.UpArrow)) && noChao)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
        }

        // ─── Morte por queda ───────────────────────────────────────────────
        if (transform.position.y < -15f) Morrer();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("ZonaMorte"))  Morrer();
        if (col.CompareTag("Checkpoint")) parkourManager?.MarcarCheckpoint(transform.position);
        if (col.CompareTag("Meta"))       parkourManager?.FaseCompleta();
    }

    public void Morrer()
    {
        if (morreu) return;
        morreu = true;
        rb.velocity    = Vector2.zero;
        rb.isKinematic = true;

        if (parkourManager != null) parkourManager.RegistrarMorte();
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        sr.color = new Color(1,0.3f,0.3f,0.5f);
        yield return new WaitForSeconds(1.2f);

        Vector3 cp = parkourManager != null ? parkourManager.UltimoCheckpoint() : Vector3.zero;
        transform.position = cp;
        rb.isKinematic = false;
        morreu = false;
        sr.color = Color.white;
    }

    void OnDrawGizmosSelected()
    {
        if (!verificadorChao) return;
        Gizmos.color = Color.lime != null ? Color.green : Color.green;
        Gizmos.DrawWireSphere(verificadorChao.position, raioChao);
    }
}
