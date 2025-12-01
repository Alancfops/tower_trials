using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyLife : MonoBehaviour
{
    [Header("UI Vida")]
    public Image greenbar;
    public Image redbar;
    public TMP_Text lifeText;

    [Header("Configuração de Vida")]
    public int maxLife = 3;
    public int currentLife;

    [Header("Recompensa ao morrer")]
    public bool curaPlayerAoMorrer = false;   
    public int quantidadeCura = 1;           

    private Animator anim;
    private readonly int morteTrigger = Animator.StringToHash("morrendo");
    private Coroutine redRoutine;

    private PlayerLife playerLife; // 🔹 referência ao player

    void Awake()
    {
        currentLife = maxLife;
        anim = GetComponentInChildren<Animator>();

        if (lifeText != null)
            lifeText.text = currentLife.ToString();

        // 🔹 encontra o Player na cena
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            playerLife = playerGO.GetComponent<PlayerLife>();
    }

    public void TakeDamage(int amount)
    {
        currentLife -= amount;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);

        float ratio = (float)currentLife / maxLife;

        if (lifeText != null)
            lifeText.text = currentLife.ToString();

        SetGreen(ratio);

        if (redRoutine != null) StopCoroutine(redRoutine);
        redRoutine = StartCoroutine(AnimateRedTo(ratio));

        if (currentLife <= 0)
            Die();
    }

    void SetGreen(float x)
    {
        if (!greenbar) return;
        Vector3 s = greenbar.rectTransform.localScale;
        s.x = x;
        greenbar.rectTransform.localScale = s;
    }

    void SetRed(float x)
    {
        if (!redbar) return;
        Vector3 s = redbar.rectTransform.localScale;
        s.x = x;
        redbar.rectTransform.localScale = s;
    }

    IEnumerator AnimateRedTo(float target)
    {
        yield return new WaitForSeconds(0.2f);

        float start = redbar.rectTransform.localScale.x;
        float t = 0f;
        float duration = 0.3f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(start, target, t / duration);
            SetRed(v);
            yield return null;
        }

        SetRed(target);
    }

    private void Die()
    {
        // Se for o inimigo pequeno, cura o player
        if (curaPlayerAoMorrer && playerLife != null)
        {
            playerLife.SetHealth(quantidadeCura);
        }

        // Animação de morte
        if (anim != null)
            anim.SetTrigger(morteTrigger);

        // Destruir o inimigo após a animação de morte
        Destroy(gameObject, 0.6f); // Aguardar a animação de morte (ajustar o tempo)
    }
}
