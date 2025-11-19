using UnityEngine;

public class AtaqueInimigo : MonoBehaviour
{
    public int dano = 2; // Inimigo maior bate 2
    private Inimigo inimigo;
    private InimigoMenor inimigoMenor; 
    private bool jaCausouDano;

    void Awake()
    {
        inimigo = GetComponentInParent<Inimigo>();
        inimigoMenor = GetComponentInParent<InimigoMenor>();
    }

    void OnEnable()
    {
        jaCausouDano = false;

        if (inimigoMenor != null)
        {
            dano = 1; // Inimigo menor bate 1
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((inimigo != null && !inimigo.estaAtacando) ||
            (inimigoMenor != null && !inimigoMenor.estaAtacando) ||
            jaCausouDano) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerLife>();
            if (player != null)
            {
                player.SetHealth(-dano);
                jaCausouDano = true;
                Debug.Log($"💥 Dano causado: {dano}");
            }
        }
    }
}
