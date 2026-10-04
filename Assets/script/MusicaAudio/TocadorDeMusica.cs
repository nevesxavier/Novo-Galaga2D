using UnityEngine;
using UnityEngine.UI;

public class TocadorDeMusica : MonoBehaviour
{
    public enum TipoDeMusica { Menu, FaseNormal, Boss, GameOver, Vitoria }
    public TipoDeMusica musicaDestaCena;

    private void Start()
    {
        // 1. Toca a música correspondente à cena
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

        // 2. Conecta o som de clique a TODOS os botões da UI presentes na cena
        ConfigurarSonsDosBotoes();
    }

    private void ConfigurarSonsDosBotoes()
    {
        // Encontra todos os botões (incluindo painéis inativos) sem usar FindObjectsSortMode
        Button[] todosOsBotoes = FindObjectsByType<Button>(FindObjectsInactive.Include);

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