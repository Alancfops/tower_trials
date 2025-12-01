using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Inimigo : MonoBehaviour
{
    [HideInInspector] public bool estaAtacando = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    [Header("Configurações")]
    public Transform player;
    [SerializeField] private float velocidade = 2f;
    [SerializeField] private float raioAtaque = 0.9f;
    [SerializeField] private float tempoEntreAtaques = 1.5f;
    [SerializeField] private float tempoReacao = 3f;

    [Header("Detecção de Visão")]
    [SerializeField] private Vector2 tamanhoDeteccao = new Vector2(8f, 2f);
    [SerializeField] private float boxOffsetX = 2f;

    [Header("Verificação de Chão")]
    [SerializeField] private Transform peDoInimigo;
    [SerializeField] private LayerMask layerChao;

    [Header("Ataque")]
    public GameObject ataqueArea;
    [SerializeField] private Transform pontoAtaque;  

    private bool estaNoChao;
    private float proximoAtaque = 0f;
    private float direcao;
    private bool devePerseguir;
    private float distX;
    private float tempoDeEspera;

    private readonly int andandoHash  = Animator.StringToHash("andando");
    private readonly int atacandoTrig = Animator.StringToHash("atacando");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        var modelo = transform.Find("Modelo");

        if (modelo != null)
        {
            animator = modelo.GetComponentInChildren<Animator>();
            sr       = modelo.GetComponentInChildren<SpriteRenderer>();
        }
        else
        {
            animator = GetComponentInChildren<Animator>();
            sr       = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        rb.gravityScale   = 2.5f;
        rb.freezeRotation = true;

        if (ataqueArea) ataqueArea.SetActive(false);
    }

    void Update()
    {
        if (!player) return;

        distX = Mathf.Abs(player.position.x - transform.position.x);

        direcao = Mathf.Sign(player.position.x - transform.position.x);

        if (sr) sr.flipX = direcao < 0f;

        estaNoChao = Physics2D.OverlapCircle(peDoInimigo.position, 0.15f, layerChao);

        // ====================================
        //      VISÃO DO INIMIGO (BOX)
        // ====================================
        Vector2 centroVisao = 
            (Vector2)transform.position + new Vector2(boxOffsetX * direcao, 0f);

        var hits = Physics2D.OverlapBoxAll(centroVisao, tamanhoDeteccao, 0f);

        bool playerNaVisao = false;
        foreach (var h in hits)
        {
            if (h.CompareTag("Player"))
            {
                playerNaVisao = true;
                break;
            }
        }

        if (!playerNaVisao)
        {
            devePerseguir = false;
            animator.SetBool(andandoHash, false);
            return;
        }

        // ====================================
        //      PERSEGUIR OU ATACAR
        // ====================================
        if (distX > raioAtaque)
        {
            devePerseguir = true;
            animator.SetBool(andandoHash, true);

            tempoDeEspera = Time.time + tempoReacao;
        }
        else if (Time.time >= tempoDeEspera)
        {
            devePerseguir = false;
            animator.SetBool(andandoHash, false);

            if (Time.time >= proximoAtaque)
            {
                proximoAtaque = Time.time + tempoEntreAtaques;
                animator.SetTrigger(atacandoTrig);
            }
        }
        else
        {
            devePerseguir = false;
            animator.SetBool(andandoHash, false);
        }
    }

    void FixedUpdate()
    {
        float vx = (devePerseguir && estaNoChao)
            ? direcao * velocidade
            : 0f;

        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    // EVENTOS DA ANIMAÇÃO
    public void AtivarAtaque()
    {
        estaAtacando = true;
        if (ataqueArea) ataqueArea.SetActive(true);
    }

    public void DesativarAtaque()
    {
        estaAtacando = false;
        if (ataqueArea) ataqueArea.SetActive(false);
    }

    // GIZMOS — Raio de ataque agora usa pontoAtaque
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (pontoAtaque != null)
            Gizmos.DrawWireSphere(pontoAtaque.position, raioAtaque);
        else
            Gizmos.DrawWireSphere(transform.position, raioAtaque);

        float dir = 1f;

        if (Application.isPlaying && player != null)
            dir = Mathf.Sign(player.position.x - transform.position.x);

        Vector3 centroVisao =
            transform.position + new Vector3(boxOffsetX * dir, 0f, 0f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(centroVisao, tamanhoDeteccao);
    }
}
