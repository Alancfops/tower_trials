using UnityEngine;
using UnityEngine.SceneManagement; // necessário para carregar cenas

public class TrocaDeCena : MonoBehaviour
{
    [SerializeField] private string nomeCenaDestino = "ProximaCena"; // nome da cena a carregar

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // verifica se quem encostou é o Player
        {
            Debug.Log("Player entrou no portal! Carregando cena: " + nomeCenaDestino);
            SceneManager.LoadScene(nomeCenaDestino);
        }
    }
}
