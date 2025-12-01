using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;           // Referência ao painel do menu
    public GameObject sobrePanel;          // Referência ao painel "Sobre"
    public Button startButton;             // Botão "Iniciar"
    public Button sobreButton;             // Botão "Sobre"
    public Button quitButton;              // Botão "Sair"
    public Button voltarMenuButton;        // Botão "Voltar para o Menu" (no painel sobre)

    void Start()
    {
        // Inicialmente, o menu está ativo
        menuPanel.SetActive(true);
        sobrePanel.SetActive(false);        // O painel de "Sobre" começa invisível

        // Adiciona os listeners para os botões
        startButton.onClick.AddListener(IniciarJogo);
        sobreButton.onClick.AddListener(MostrarSobre);  // Abre o painel "Sobre"
        quitButton.onClick.AddListener(SairDoJogo);     // Fecha o jogo
        voltarMenuButton.onClick.AddListener(VolverParaMenu);  // Volta ao menu
    }

    // Função que será chamada quando o botão "Iniciar" for pressionado
    public void IniciarJogo()
    {
        menuPanel.SetActive(false);  // Desativa o painel do menu
    }

    // Função que abre o painel "Sobre" com informações do jogo
    public void MostrarSobre()
    {
        sobrePanel.SetActive(true);   // Ativa o painel de "Sobre"
        menuPanel.SetActive(false);  // Desativa o painel do menu
    }

    // Função para voltar ao menu quando o botão "Voltar para o Menu" for pressionado
    public void VolverParaMenu()
    {
        sobrePanel.SetActive(false);  // Desativa o painel de "Sobre"
        menuPanel.SetActive(true);    // Ativa o painel do menu
    }

    // Função para sair do jogo
    public void SairDoJogo()
    {
        Application.Quit();           // Fecha o jogo
    }
}
