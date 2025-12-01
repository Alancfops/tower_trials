using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;   

public class PlayerLife : MonoBehaviour
{
    [Header("UI Vida")]
    public Image lifebar;
    public Image greenbar;
    public Image redbar;
    public Image fadePanel;
    public GameObject deathText;

    public TMP_Text lifeText;

    [Header("Configuração de Vida")]
    public int maxLife = 10;
    public int currentLife;

    [Header("Invulnerabilidade")]
    public float invulnTime = 0.8f;
    public float blinkInterval = 0.1f;

    private SpriteRenderer sr;
    private bool invulnerable = false;
    private Coroutine redRoutine;
    private Coroutine blinkRoutine;

    private Animator anim;
    private readonly int morteTrigger = Animator.StringToHash("morrendo");

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentLife = maxLife;
        UpdateBars(1f);

        if (lifeText != null)
            lifeText.text = currentLife.ToString();

        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            fadePanel.color = new Color(c.r, c.g, c.b, 0f);
            fadePanel.gameObject.SetActive(true);
        }

        if (deathText != null)
            deathText.SetActive(false);
    }

    public void SetHealth(int amount)
    {
        if (amount < 0 && invulnerable) return;

        currentLife = Mathf.Clamp(currentLife + amount, 0, maxLife);
        float ratio = (float)currentLife / maxLife;

        if (lifeText != null)
            lifeText.text = currentLife.ToString();

        SetGreen(ratio);

        if (redRoutine != null) StopCoroutine(redRoutine);
        redRoutine = StartCoroutine(AnimateRedTo(ratio));

        if (amount < 0 && currentLife > 0)
        {
            if (blinkRoutine != null) StopCoroutine(blinkRoutine);
            blinkRoutine = StartCoroutine(Blink());
        }

        if (currentLife <= 0)
            StartCoroutine(Die());
    }

    void UpdateBars(float ratio)
    {
        SetGreen(ratio);
        SetRed(ratio);
    }

    void SetGreen(float x)
    {
        if (!greenbar) return;
        var s = greenbar.rectTransform.localScale;
        s.x = Mathf.Clamp01(x);
        greenbar.rectTransform.localScale = s;
    }

    void SetRed(float x)
    {
        if (!redbar) return;
        var s = redbar.rectTransform.localScale;
        s.x = Mathf.Clamp01(x);
        redbar.rectTransform.localScale = s;
    }

    IEnumerator AnimateRedTo(float target)
    {
        if (!redbar) yield break;

        yield return new WaitForSeconds(0.25f);
        float start = redbar.rectTransform.localScale.x;
        float t = 0f;
        float dur = 0.35f;

        while (t < dur)
        {
            t += Time.deltaTime;
            float x = Mathf.Lerp(start, target, t / dur);
            SetRed(x);
            yield return null;
        }
        SetRed(target);
    }

    IEnumerator Blink()
    {
        invulnerable = true;
        float elapsed = 0f;

        while (elapsed < invulnTime)
        {
            if (sr) sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        if (sr) sr.enabled = true;
        invulnerable = false;
    }

    IEnumerator Die()
    {
        if (fadePanel != null)
        {
            Color baseC = fadePanel.color;
            float t = 0f, dur = 0.6f;

            while (t < dur)
            {
                t += Time.deltaTime;
                float a = Mathf.Lerp(0f, 1f, t / dur);
                fadePanel.color = new Color(baseC.r, baseC.g, baseC.b, a);
                yield return null;
            }

            fadePanel.color = new Color(baseC.r, baseC.g, baseC.b, 1f);
        }

        if (deathText != null)
            deathText.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
