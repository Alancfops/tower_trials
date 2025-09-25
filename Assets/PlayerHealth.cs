using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 5;
    public int health;

    [Header("I-frames (invencibilidade breve)")]
    public float invulnTime = 0.5f;
    public float blinkInterval = 0.08f;

    [Header("Feedback")]
    public float knockbackForce = 6f;

    public event Action<int,int> OnHealthChanged; // (vidaAtual, vidaMax)
    public event Action OnDied;

    Rigidbody2D rb;
    SpriteRenderer sr;
    bool invulnerable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        health = maxHealth;
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void Heal(int amount)
    {
        if (health <= 0) return;
        health = Mathf.Min(maxHealth, health + amount);
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        if (invulnerable || health <= 0) return;

        health = Mathf.Max(0, health - amount);
        OnHealthChanged?.Invoke(health, maxHealth);

        // knockback (opcional)
        if (hitDirection != Vector2.zero)
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        if (health == 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(CoInvuln());
        }
    }

    public void Kill()
    {
        if (health > 0)
        {
            health = 0;
            OnHealthChanged?.Invoke(health, maxHealth);
            Die();
        }
    }

    void Die()
    {
        OnDied?.Invoke();
        // exemplo simples: recarrega a fase
        Scene s = SceneManager.GetActiveScene();
        SceneManager.LoadScene(s.buildIndex);
    }

    IEnumerator CoInvuln()
    {
        invulnerable = true;
        if (sr != null)
        {
            float t = 0f;
            while (t < invulnTime)
            {
                sr.enabled = !sr.enabled;
                yield return new WaitForSeconds(blinkInterval);
                t += blinkInterval;
            }
            sr.enabled = true;
        }
        invulnerable = false;
    }
}
