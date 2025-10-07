using UnityEngine;

public class AtaqueInimigo : MonoBehaviour
{
    public int dano = 1;
    private Inimigo inimigo;
    private bool jaCausouDano;

    void Awake()
    {
        inimigo = GetComponentInParent<Inimigo>();
    }

    void OnEnable()
    {
        jaCausouDano = false; // reset a cada ataque
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inimigo.estaAtacando || jaCausouDano) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerLife>();
            if (player != null)
            {
                player.SetHealth(-dano);
                jaCausouDano = true;
                Debug.Log("💥 Player atingido pelo golpe do inimigo!");
            }
        }
    }
}
