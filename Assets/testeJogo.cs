using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TesteJogo : MonoBehaviour
{
    Rigidbody2D rb;
    float inputH;
    [SerializeField] private int velocidade = 6;
    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask layerChao;
    private bool estaNoChao;

    private Animator animator;
    private int movendoHash = Animator.StringToHash("movendo");
    private int saltandoHash = Animator.StringToHash("saltando");
    private int atacarTrig   = Animator.StringToHash("atacar");

    private SpriteRenderer spriteRenderer;

    public GameObject ataqueArea;

    private static TesteJogo instance;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }

        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.1f, layerChao);

        animator.SetBool(movendoHash, inputH != 0);
        animator.SetBool(saltandoHash, !estaNoChao);

        // virar sprite
        if (inputH < 0)
            spriteRenderer.flipX = true;
        else if (inputH > 0)
            spriteRenderer.flipX = false;

        // ataque
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(atacarTrig);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputH * velocidade, rb.linearVelocity.y);
    }

    public void AtivarAtaque()
    {
        ataqueArea.SetActive(true);
    }

    public void DesativarAtaque()
    {
        ataqueArea.SetActive(false);
    }
}
