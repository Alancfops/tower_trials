using UnityEngine;

public class InimigoAnimationEvents : MonoBehaviour
{
    private Inimigo inimigo;

    void Awake()
    {
        inimigo = GetComponentInParent<Inimigo>();
    }

    // chamados pelos eventos do clip "atacando"
    public void AtivarAtaque()     { if (inimigo) inimigo.AtivarAtaque(); }
    public void DesativarAtaque()  { if (inimigo) inimigo.DesativarAtaque(); }
}
