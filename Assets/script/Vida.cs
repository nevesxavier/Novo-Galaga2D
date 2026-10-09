
using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controla a vida do jogador e dos inimigos.
/// Também controla os efeitos do Power Up de vida.
/// Ao derrotar um inimigo, adiciona pontos ao jogador.
/// </summary>
public class Vida : MonoBehaviour
{
    // ============================================================
    // VIDA
    // ============================================================

    [Header("VIDA")]

    [SerializeField]
    private int vidaMaxima = 3;

    [SerializeField]
    private bool destruirAoMorrer = true;


    // ============================================================
    // CONFIGURAÇÃO DO POWER UP DE VIDA
    // ============================================================

    [Header("POWER UP DE VIDA")]

    [Tooltip("Escolha o efeito do Power Up.")]
    [SerializeField]
    private TipoEfeitoVida tipoEfeitoPowerUp =
        TipoEfeitoVida.RegenerarVida;

    [Tooltip("Quantidade de corações recuperados.")]
    [SerializeField]
    private int quantidadeRegenerada = 1;

    [Tooltip("Quantidade máxima de corações permitida.")]
    [SerializeField]
    private int limiteMaximoDeCorações = 5;


    // ============================================================
    // PONTUAÇÃO
    // ============================================================

    [Header("PONTUAÇÃO DO INIMIGO")]

    [Tooltip("Pontos recebidos ao derrotar este inimigo.")]
    [SerializeField]
    private int pontosAoMorrer = 100;


    // ============================================================
    // EVENTOS
    // ============================================================

    [Header("EVENTOS")]

    public UnityEvent AoMorrer;

    public event Action<int, int> AoMudarVida;


    // ============================================================
    // INFORMAÇÕES DA VIDA
    // ============================================================

    public int VidaAtual { get; private set; }

    public int VidaMaxima => vidaMaxima;


    // ============================================================
    // TIPOS DE EFEITO
    // ============================================================

    public enum TipoEfeitoVida
    {
        RegenerarVida,
        AdicionarCoracao
    }


    // ============================================================
    // INICIALIZAÇÃO
    // ============================================================

    private void Awake()
    {
        VidaAtual = vidaMaxima;
    }


    // ============================================================
    // RECEBER DANO
    // ============================================================

    public void ReceberDano(int dano)
    {
        // Impede dano inválido ou dano após a morte.
        if (VidaAtual <= 0 || dano <= 0)
            return;

        // Diminui a vida.
        VidaAtual = Mathf.Max(0, VidaAtual - dano);

        // Atualiza os sistemas que acompanham a vida.
        AoMudarVida?.Invoke(VidaAtual, vidaMaxima);

        // Verifica se o personagem morreu.
        if (VidaAtual == 0)
        {
            // Executa os eventos configurados no Inspector.
            AoMorrer?.Invoke();

            // Se for o jogador, informa o GerenciadorFase.
            if (CompareTag("Player"))
            {
                GerenciadorFase gerenciador =
                    FindAnyObjectByType<GerenciadorFase>();

                if (gerenciador != null)
                {
                    gerenciador.JogadorMorreu();
                }
            }
            // Se for um inimigo, adiciona pontos e atualiza a fase.
            else if (CompareTag("Enemy"))
            {
                // Adiciona pontos apenas uma vez,
                // pois a vida já chegou a zero.
                if (GerenciadorDePontuacao.Instancia != null)
                {
                    GerenciadorDePontuacao.Instancia
                        .AdicionarPontos(pontosAoMorrer);

                    Debug.Log(
                        "Inimigo derrotado! +" +
                        pontosAoMorrer + " pontos."
                    );
                }
                else
                {
                    Debug.LogWarning(
                        "GerenciadorDePontuacao não encontrado. " +
                        "Os pontos não foram adicionados."
                    );
                }

                // Informa ao gerenciador da fase que o inimigo morreu.
                GerenciadorFase gerenciador =
                    FindAnyObjectByType<GerenciadorFase>();

                if (gerenciador != null)
                {
                    gerenciador.InimigoMorreu();
                }
            }

            // Destrói o objeto, se estiver configurado.
            if (destruirAoMorrer)
            {
                Destroy(gameObject);
            }
        }
    }


    // ============================================================
    // CURAR
    // ============================================================

    public void Curar(int quantidade)
    {
        if (VidaAtual <= 0 || quantidade <= 0)
            return;

        VidaAtual = Mathf.Min(
            vidaMaxima,
            VidaAtual + quantidade
        );

        AoMudarVida?.Invoke(VidaAtual, vidaMaxima);

        Debug.Log(
            "Vida regenerada! " +
            VidaAtual + "/" + vidaMaxima
        );
    }


    // ============================================================
    // POWER UP DE VIDA
    // ============================================================

    public void AplicarPowerUpVida(int quantidade)
    {
        switch (tipoEfeitoPowerUp)
        {
            case TipoEfeitoVida.RegenerarVida:

                Curar(quantidadeRegenerada);

                Debug.Log(
                    "Power Up: regeneração de vida."
                );

                break;

            case TipoEfeitoVida.AdicionarCoracao:

                AdicionarCoracao(quantidade);

                Debug.Log(
                    "Power Up: novo coração adicionado."
                );

                break;
        }
    }


    // ============================================================
    // ADICIONAR CORAÇÃO
    // ============================================================

    public void AdicionarCoracao(int quantidade)
    {
        if (quantidade <= 0)
            return;

        if (vidaMaxima >= limiteMaximoDeCorações)
        {
            Debug.Log(
                "O jogador já possui o máximo de corações."
            );

            return;
        }

        int vidaMaximaAnterior = vidaMaxima;

        vidaMaxima += quantidade;

        vidaMaxima = Mathf.Min(
            vidaMaxima,
            limiteMaximoDeCorações
        );

        int coracoesAdicionados =
            vidaMaxima - vidaMaximaAnterior;

        VidaAtual += coracoesAdicionados;

        VidaAtual = Mathf.Min(
            VidaAtual,
            vidaMaxima
        );

        AoMudarVida?.Invoke(VidaAtual, vidaMaxima);

        Debug.Log("Novo coração adicionado!");

        Debug.Log(
            "Vida: " + VidaAtual + "/" + vidaMaxima
        );
    }
}
