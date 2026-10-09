
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TocadorDeMusica : MonoBehaviour
{
    public enum TipoDeMusica
    {
        Menu,
        FaseNormal,
        Boss,
        GameOver,
        Vitoria
    }

    [Header("MÚSICA DESTA CENA")]
    public TipoDeMusica musicaDestaCena;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    private void Start()
    {
        TocarMusicaDaCena();
        ConfigurarSonsDosBotoes();
    }

    private void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        // Só executa a troca para a cena à qual este objeto pertence.
        if (cena.name == gameObject.scene.name)
        {
            TocarMusicaDaCena();
        }
    }

    private void TocarMusicaDaCena()
    {
        if (GerenciadorDeAudio.Instancia == null)
        {
            Debug.LogError(
                "TocadorDeMusica: GerenciadorDeAudio não encontrado na cena " +
                gameObject.scene.name
            );
            return;
        }

        switch (musicaDestaCena)
        {
            case TipoDeMusica.Menu:
                GerenciadorDeAudio.Instancia.TocarMusicaTelaInicial();
                break;

            case TipoDeMusica.FaseNormal:
                GerenciadorDeAudio.Instancia.TocarMusicaFase();
                break;

            case TipoDeMusica.Boss:
                GerenciadorDeAudio.Instancia.TocarMusicaBoss();
                break;

            case TipoDeMusica.GameOver:
                GerenciadorDeAudio.Instancia.TocarMusicaGameOver();
                break;

            case TipoDeMusica.Vitoria:
                GerenciadorDeAudio.Instancia.TocarMusicaVitoria();
                break;
        }

        Debug.Log(
            "Cena: " + gameObject.scene.name +
            " | Música selecionada: " + musicaDestaCena
        );
    }

    private void ConfigurarSonsDosBotoes()
    {
        Button[] todosOsBotoes =
            FindObjectsByType<Button>(FindObjectsInactive.Include);

        foreach (Button botao in todosOsBotoes)
        {
            botao.onClick.RemoveListener(TocarSomDoBotao);
            botao.onClick.AddListener(TocarSomDoBotao);
        }
    }

    private void TocarSomDoBotao()
    {
        if (GerenciadorDeAudio.Instancia != null)
        {
            GerenciadorDeAudio.Instancia.TocarSomClique();
        }
    }
}
