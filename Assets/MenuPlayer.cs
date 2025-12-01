using UnityEngine;
using UnityEngine.UI;

public class MenuPlayer : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject menuPanel;
    public GameObject sobrePanel;

    [Header("Botões")]
    public Button startButton;          // Iniciar / Continuar
    public Button sobreButton;
    public Button quitButton;
    public Button voltarMenuButton;     // Voltar do Sobre

    [Header("HUD do jogador")]
    public Button hudMenuButton;        // Botão no HUD para abrir o menu

    private bool jogoIniciado = false;

    void Start()
    {
        // Menu inicial ON
        menuPanel.SetActive(false);
        sobrePanel.SetActive(false);

        // Listeners
        startButton.onClick.AddListener(StartOuContinuar);
        sobreButton.onClick.AddListener(MostrarSobre);
        quitButton.onClick.AddListener(SairDoJogo);
        voltarMenuButton.onClick.AddListener(VoltarAoMenu);
        hudMenuButton.onClick.AddListener(AbrirMenuJogador);
    }

    // ==========================
    //     FUNÇÕES PRINCIPAIS
    // ==========================

    // Iniciar ou continuar
    public void StartOuContinuar()
    {
        if (!jogoIniciado)
        {
            // Primeira vez jogando
            jogoIniciado = true;
            startButton.GetComponentInChildren<Text>().text = "Continuar";
        }

        // Fecha o menu e continua o jogo
        menuPanel.SetActive(false);
        sobrePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Abrir Sobre
    public void MostrarSobre()
    {
        sobrePanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    // Sair do Sobre
    public void VoltarAoMenu()
    {
        sobrePanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    // Abrir menu enquanto joga
    public void AbrirMenuJogador()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;  // Pausa o jogo
    }

    // Sair
    public void SairDoJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
