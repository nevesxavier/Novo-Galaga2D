using UnityEngine;
using UnityEngine.UI;

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

    public TipoDeMusica musicaDestaCena;

    private void Start()
    {
        if (GerenciadorDeAudio.Instancia != null)
        {
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
        }

        ConfigurarSonsDosBotoes();
    }

    private void ConfigurarSonsDosBotoes()
    {
        Button[] todosOsBotoes =
            FindObjectsByType<Button>(FindObjectsInactive.Include);

        foreach (Button botao in todosOsBotoes)
        {
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