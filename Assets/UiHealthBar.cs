using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public PlayerHealth target;
    public Image fill;

    void Awake()
    {
        if (target != null) target.OnHealthChanged += UpdateBar;
    }

    void OnDestroy()
    {
        if (target != null) target.OnHealthChanged -= UpdateBar;
    }

    void Start()
    {
        if (target != null) UpdateBar(target.health, target.maxHealth);
    }

    void UpdateBar(int cur, int max)
    {
        if (fill != null) fill.fillAmount = (max > 0) ? (float)cur / max : 0f;
    }
}
