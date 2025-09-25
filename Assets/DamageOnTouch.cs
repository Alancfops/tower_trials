using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D col)
    {
        TryHit(col.collider);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        TryHit(col);
    }

    void TryHit(Collider2D col)
    {
        var hp = col.GetComponent<PlayerHealth>();
        if (hp == null) return;

        // direção do empurrão = do dano para o player
        Vector2 dir = (hp.transform.position - transform.position);
        hp.TakeDamage(damage, dir);
    }
}
