using UnityEngine;
using UnityEngine.UI;

public class PainelIntro : MonoBehaviour
{
    public GameObject painel;    // Referência ao painel
    public Button pularButton;   // Botão de pular

    void Start()
    {
        // Inicialmente, o painel está ativo
        painel.SetActive(true);

        // Adiciona a função que fecha o painel ao botão
        if (pularButton != null)
        {
            pularButton.onClick.AddListener(PularPainel); // Função que vai fechar o painel
        }
    }

    // Função que será chamada quando o botão "Pular" for pressionado
    public void PularPainel()
    {
        Debug.Log("Painel fechado!"); // Confirmação de que a função foi chamada
        painel.SetActive(false); // Desativa o painel de introdução
    }
}
