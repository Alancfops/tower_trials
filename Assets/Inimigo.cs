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

    [Header("Detecção (retangular e direcional)")]
    [SerializeField] private Vector2 tamanhoDeteccao = new Vector2(8f, 2f); // largura x altura
    [SerializeField] private float boxOffsetX = 2f;                          // deslocamento à frente

    [Header("Verificação de Chão")]
    [SerializeField] private Transform peDoInimigo;
    [SerializeField] private LayerMask layerChao;

    [Header("Referências")]
    public GameObject ataqueArea; // filho com BoxCollider2D + AtaqueInimigo

    private bool estaNoChao;
    private float proximoAtaque = 0f;
    private float direcao;        // -1 = esquerda | +1 = direita
    private bool devePerseguir;
    private float distX;          // distância horizontal ao player

    private readonly int andandoHash  = Animator.StringToHash("andando");
    private readonly int atacandoTrig = Animator.StringToHash("atacando");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        var modelo = transform.Find("Modelo");
        if (modelo != null)
        {
            animator = modelo.GetComponent<Animator>();
            sr       = modelo.GetComponent<SpriteRenderer>();
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

        // Direção que o inimigo deve olhar (com base no X do player)
        direcao = Mathf.Sign(player.position.x - transform.position.x);
        if (sr) sr.flipX = direcao < 0f;

        // Checa chão (pequeno círculo nos pés)
        estaNoChao = Physics2D.OverlapCircle(peDoInimigo.position, 0.15f, layerChao);

        // Centro do box de visão, deslocado para a frente do inimigo
        Vector2 centroVisao = (Vector2)transform.position + new Vector2(boxOffsetX * direcao, 0f);

        // OverlapBoxAll sem filtro de layer; filtramos por Tag depois
        var hits = Physics2D.OverlapBoxAll(centroVisao, tamanhoDeteccao, 0f);

        bool playerNaVisao = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Player"))
            {
                playerNaVisao = true;
                break;
            }
        }

        // Distância horizontal ao player (evita diagonais enganarem)
        distX = Mathf.Abs(player.position.x - transform.position.x);

        // ---------- FSM por visão retangular + distância horizontal ----------
        if (!playerNaVisao)
        {
            devePerseguir = false;
            animator.SetBool(andandoHash, false);
            return;
        }

        // Dentro da visão: persegue se fora do alcance, ataca se perto
        if (distX > raioAtaque)
        {
            devePerseguir = true;
            animator.SetBool(andandoHash, true);
        }
        else
        {
            devePerseguir = false;
            animator.SetBool(andandoHash, false);

            if (Time.time >= proximoAtaque)
            {
                proximoAtaque = Time.time + tempoEntreAtaques;
                animator.SetTrigger(atacandoTrig);
                // Debug.Log("Atacando player!");
            }
        }
    }

    void FixedUpdate()
    {
        // move só quando deve perseguir e está no chão
        float vx = (devePerseguir && estaNoChao) ? direcao * velocidade : 0f;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);   // use velocity para Rigidbody2D
    }

    // Eventos de animação
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

    // Gizmos para ajustar o box de visão no editor
    void OnDrawGizmosSelected()
    {
        // Direção para desenhar o box no editor
        float dir = 1f;
        if (Application.isPlaying)
            dir = Mathf.Sign((player ? player.position.x : transform.position.x) - transform.position.x);
        else if (sr != null)
            dir = sr.flipX ? -1f : 1f;

        Vector3 centro = transform.position + new Vector3(boxOffsetX * dir, 0f, 0f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(centro, new Vector3(tamanhoDeteccao.x, tamanhoDeteccao.y, 0f));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioAtaque);

        if (peDoInimigo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(peDoInimigo.position, 0.15f);
        }
    }
}
