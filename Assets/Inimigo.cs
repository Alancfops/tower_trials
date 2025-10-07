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
    [SerializeField] private float raioDeteccao = 3f;
    [SerializeField] private float raioAtaque = 0.9f;
    [SerializeField] private float tempoEntreAtaques = 1.5f; // ⏳ tempo entre ataques

    [Header("Verificação de Chão")]
    [SerializeField] private Transform peDoInimigo;
    [SerializeField] private LayerMask layerChao;

    [Header("Referências")]
    public GameObject ataqueArea; // filho com BoxCollider2D + AtaqueInimigo

    private bool estaNoChao;
    private float proximoAtaque = 0f;
    private float direcao;
    private float dist;
    private bool devePerseguir;

    // Parâmetros do Animator
    private readonly int andandoHash  = Animator.StringToHash("andando");
    private readonly int atacandoTrig = Animator.StringToHash("atacando");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Pega componentes do filho "Modelo"
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

        dist    = Vector2.Distance(transform.position, player.position);
        direcao = Mathf.Sign(player.position.x - transform.position.x);

        if (sr) sr.flipX = direcao < 0f;

        // Verifica chão
        estaNoChao = Physics2D.OverlapCircle(peDoInimigo.position, 0.15f, layerChao);

        // ---------- IA por distância ----------
        if (dist > raioDeteccao)
        {
            // fora do alcance de visão
            devePerseguir = false;
            animator.SetBool(andandoHash, false);
            return;
        }

        if (dist > raioAtaque)
        {
            // perseguindo
            devePerseguir = true;
            animator.SetBool(andandoHash, true);
        }
        else
        {
            // atacar
            devePerseguir = false;
            animator.SetBool(andandoHash, false);

            // ✅ cooldown de 1.5 segundos
            if (Time.time >= proximoAtaque)
            {
                proximoAtaque = Time.time + tempoEntreAtaques;
                animator.SetTrigger(atacandoTrig);
                Debug.Log("⚔️ Atacando player!");
            }
        }
    }

    void FixedUpdate()
    {
        float vx = (devePerseguir && estaNoChao) ? direcao * velocidade : 0f;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeteccao);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioAtaque);

        if (peDoInimigo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(peDoInimigo.position, 0.15f);
        }
    }
}
