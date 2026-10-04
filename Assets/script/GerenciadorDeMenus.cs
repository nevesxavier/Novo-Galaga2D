using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla os botões e a navegação entre as telas do jogo.
/// </summary>
public class GerenciadorDeMenus : MonoBehaviour
{
    public enum TipoDeTela
    {
        Padrão,
        GameOver,
        Vitoria
    }


    [Header("CONFIGURAÇÃO DESTA CENA")]

    [Tooltip("Defina se esta cena é uma tela normal, Game Over ou Vitória.")]
    public TipoDeTela tipoDeTelaAtual = TipoDeTela.Padrão;


    [Header("CONFIGURAÇÕES DE NOME DAS CENAS")]

    [Tooltip("Nome exato da cena da primeira fase.")]
    public string nomePrimeiraFase = "Fase1";

    [Tooltip("Nome exato da cena do Menu Inicial.")]
    public string nomeMenuInicial = "MenuInicial";


    [Header("PAINEL DE UI")]

    // Não é necessário para o menu que está em uma cena separada.
    // Pode ficar vazio.
    public GameObject painelPause;


    // Guarda o nome da cena anterior.
    private static string nomeCenaAnterior = "";

    // Guarda a última fase que o jogador estava jogando.
    private static string ultimaFaseJogada = "";


    private void Awake()
    {
        // Garante que o jogo esteja rodando normalmente.
        Time.timeScale = 1f;


        // Descobre o nome da cena atual.
        string cenaAtual = SceneManager.GetActiveScene().name;


        // Se for uma fase normal, guarda o nome dela.
        if (tipoDeTelaAtual == TipoDeTela.Padrão &&
            cenaAtual != nomeMenuInicial)
        {
            ultimaFaseJogada = cenaAtual;
        }
    }


    // =========================================================
    // BOTÃO JOGAR NOVAMENTE
    // =========================================================

    public void JogarNovamente()
    {
        // Garante que o tempo volte ao normal.
        Time.timeScale = 1f;


        // Se estiver na tela de vitória,
        // começa novamente pela primeira fase.
        if (tipoDeTelaAtual == TipoDeTela.Vitoria)
        {
            CarregarCenaComHistorico(nomePrimeiraFase);
        }


        // Se estiver no Game Over,
        // volta para a fase onde o jogador perdeu.
        else if (tipoDeTelaAtual == TipoDeTela.GameOver)
        {
            if (!string.IsNullOrEmpty(ultimaFaseJogada))
            {
                SceneManager.LoadScene(ultimaFaseJogada);
            }
            else
            {
                CarregarCenaComHistorico(nomePrimeiraFase);
            }
        }


        // Caso seja uma cena normal,
        // simplesmente reinicia a cena atual.
        else
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }


    // =========================================================
    // BOTÃO JOGAR - MENU INICIAL
    // =========================================================

    public void Jogar()
    {
        CarregarCenaComHistorico(nomePrimeiraFase);
    }


    // =========================================================
    // RETORNAR PARA A CENA ANTERIOR
    // =========================================================

    public void RetornarParaCenaAnterior()
    {
        Time.timeScale = 1f;


        if (!string.IsNullOrEmpty(nomeCenaAnterior))
        {
            string cenaAtual =
                SceneManager.GetActiveScene().name;

            string destino = nomeCenaAnterior;

            nomeCenaAnterior = cenaAtual;

            SceneManager.LoadScene(destino);
        }
        else
        {
            CarregarCenaComHistorico(nomeMenuInicial);
        }
    }


    // =========================================================
    // BOTÃO MENU INICIAL
    // =========================================================

    public void IrParaMenuInicial()
    {
        CarregarCenaComHistorico(nomeMenuInicial);
    }


    // =========================================================
    // BOTÃO RETORNAR AO JOGO
    // =========================================================

    public void RetornarAoJogo()
    {
        // Garante que o jogo volte à velocidade normal.
        Time.timeScale = 1f;


        // Verifica se existe uma fase registrada.
        if (!string.IsNullOrEmpty(ultimaFaseJogada))
        {
            // Volta para a última fase que estava sendo jogada.
            SceneManager.LoadScene(ultimaFaseJogada);
        }
        else
        {
            // Caso não exista uma fase registrada,
            // volta para a primeira fase.
            SceneManager.LoadScene(nomePrimeiraFase);
        }
    }


    // =========================================================
    // BOTÃO PAUSAR
    // =========================================================

    public void PausarJogo()
    {
        // Se houver um painel configurado,
        // mostra o painel.
        if (painelPause != null)
        {
            painelPause.SetActive(true);
        }


        // Pausa o jogo.
        Time.timeScale = 0f;
    }


    // =========================================================
    // BOTÃO SAIR
    // =========================================================

    public void Sair()
    {
        Debug.Log("O jogador clicou em SAIR.");

        // Fecha o jogo quando estiver em uma versão executável.
        Application.Quit();
    }


    // =========================================================
    // MÉTODO AUXILIAR PARA TROCAR DE CENA
    // =========================================================

    private void CarregarCenaComHistorico(string nomeDaProximaCena)
    {
        // Garante que o jogo esteja rodando normalmente.
        Time.timeScale = 1f;


        // Guarda a cena atual antes de trocar.
        nomeCenaAnterior =
            SceneManager.GetActiveScene().name;


        // Carrega a próxima cena.
        SceneManager.LoadScene(nomeDaProximaCena);
    }
}