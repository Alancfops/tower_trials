using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int dano = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Inimigo"))
        {
            var vida = other.GetComponent<EnemyLife>();

            if (vida != null)
            {
                vida.TakeDamage(dano);
                Debug.Log("⚔️ Player causou dano no inimigo!");
            }
        }
    }
}
