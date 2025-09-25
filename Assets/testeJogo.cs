using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class testeJogo : MonoBehaviour
{
    Rigidbody2D rb;
    float inputH;
    [SerializeField]private int velocidade = 6;
    [SerializeField]private Transform peDoPersonagem;
    [SerializeField]private LayerMask layerChao;
    private bool estaNoChao;
    private Animator animator;
    private int movendoHash = Animator.StringToHash("movendo");
    private int saltandoHash = Animator.StringToHash("saltando");
    private int atacarTrig   = Animator.StringToHash("atacar"); 
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        inputH = Input.GetAxisRaw("Horizontal"); // A/D ou ←/→

        //aperta a tecla w para pular
        if (Input.GetKeyDown(KeyCode.W) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }
        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.1f, layerChao);

        animator.SetBool(movendoHash, inputH != 0);
        animator.SetBool(saltandoHash, !estaNoChao);

        if (inputH < 0)
        {
            spriteRenderer.flipX = true; // Inverte o sprite para a esquerda
        }
        else if (inputH > 0)
        {
            spriteRenderer.flipX = false; // Normaliza o sprite para a direita
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(atacarTrig);
        }

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputH * velocidade, rb.linearVelocity.y);
    }
}
