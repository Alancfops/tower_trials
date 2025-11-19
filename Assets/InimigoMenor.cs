using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class InimigoMenor : MonoBehaviour
{
    [HideInInspector] public bool estaAtacando = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    [Header("Configurações")]
    public Transform player;
    [SerializeField] private float velocidade = 1.5f;
    [SerializeField] private float raioAtaque = 0.7f;
    [SerializeField] private float tempoEntreAtaques = 1.0f;
    [SerializeField] private float tempoReacao = 1.2f; 

    [Header("Detecção")]
    [SerializeField] private Vector2 tamanhoDeteccao = new Vector2(4f, 1.6f);
    [SerializeField] private float boxOffsetX = 2f;

    [Header("Chão")]
    [SerializeField] private Transform peDoInimigo;
    [SerializeField] private LayerMask layerChao;

    [Header("Ataque")]
    public GameObject ataqueArea;

    private bool estaNoChao;
    private float proximoAtaque;
    private float direcao;
    private bool devePerseguir;
    private float distX;
    private float tempoDeEspera;

    // Parâmetros do Animator
    private readonly int andandoHash = Animator.StringToHash("movendo");
    private readonly int atacandoTrig = Animator.StringToHash("atacando");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        var modelo = transform.Find("Modelo");
        if (modelo != null)
        {
            animator = modelo.GetComponent<Animator>();
            sr = modelo.GetComponent<SpriteRenderer>();
        }
        else
        {
            animator = GetComponentInChildren<Animator>();
            sr = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        rb.gravityScale = 2.5f;
        rb.freezeRotation = true;

        if (ataqueArea) ataqueArea.SetActive(false);
    }

    void Update()
    {
        if (!player) return;

        distX = Mathf.Abs(player.position.x - transform.position.x);
        direcao = Mathf.Sign(player.position.x - transform.position.x);
        sr.flipX = direcao < 0;

        estaNoChao = Physics2D.OverlapCircle(peDoInimigo.position, 0.15f, layerChao);

        Vector2 centroVisao = (Vector2)transform.position + new Vector2(boxOffsetX * direcao, 0);
        var hits = Physics2D.OverlapBoxAll(centroVisao, tamanhoDeteccao, 0);

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
        float vx = (devePerseguir && estaNoChao) ? direcao * velocidade : 0;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    // Chamado pela animação
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
}
