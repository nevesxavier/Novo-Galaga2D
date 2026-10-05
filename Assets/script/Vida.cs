using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controla a vida do jogador e dos inimigos.
///
/// Também permite escolher o comportamento do Power Up de vida:
///
/// 1 - Regenerar a vida atual.
/// 2 - Adicionar um novo coração à vida máxima.
/// </summary>
public class Vida : MonoBehaviour
{
    // ============================================================
    // VIDA
    // ============================================================

    [Header("VIDA")]

    // Quantidade máxima de vida/corações.
    [SerializeField]
    private int vidaMaxima = 3;

    // Se estiver marcado, o objeto será destruído quando morrer.
    [SerializeField]
    private bool destruirAoMorrer = true;


    // ============================================================
    // CONFIGURAÇÃO DO POWER UP DE VIDA
    // ============================================================

    [Header("POWER UP DE VIDA")]

    [Tooltip("Escolha o que o Power Up de vida fará.")]
    [SerializeField]
    private TipoEfeitoVida tipoEfeitoPowerUp = TipoEfeitoVida.RegenerarVida;

    // Quantidade de vida que será recuperada
    // quando estiver usando o modo "Regenerar Vida".
    [Tooltip("Quantidade de corações recuperados no modo Regenerar Vida.")]
    [SerializeField]
    private int quantidadeRegenerada = 1;

    // Limite máximo de corações quando estiver usando
    // o modo "Adicionar Coração".
    [Tooltip("Quantidade máxima de corações que o jogador poderá ter.")]
    [SerializeField]
    private int limiteMaximoDeCorações = 5;


    // ============================================================
    // EVENTOS
    // ============================================================

    [Header("EVENTOS")]

    // Evento executado quando o jogador/inimigo morrer.
    public UnityEvent AoMorrer;

    // Evento enviado quando a vida muda.
    //
    // Primeiro valor = vida atual.
    // Segundo valor = vida máxima.
    public event Action<int, int> AoMudarVida;


    // ============================================================
    // INFORMAÇÕES DA VIDA
    // ============================================================

    // Vida atual.
    public int VidaAtual { get; private set; }

    // Vida máxima.
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
        // Começa com a vida máxima.
        VidaAtual = vidaMaxima;
    }


    // ============================================================
    // RECEBER DANO
    // ============================================================

    /// <summary>
    /// Aplica dano ao jogador ou inimigo.
    /// </summary>
    public void ReceberDano(int dano)
    {
        // Se já estiver morto, não recebe dano.
        if (VidaAtual <= 0)
            return;

        // Diminui a vida.
        VidaAtual = Mathf.Max(
            0,
            VidaAtual - dano
        );

        // Informa que a vida mudou.
        AoMudarVida?.Invoke(
            VidaAtual,
            vidaMaxima
        );


        // Verifica se morreu.
        if (VidaAtual == 0)
        {
            // Executa os eventos configurados no Inspector.
            AoMorrer?.Invoke();


            // Procura o GerenciadorFase.
            GerenciadorFase gerenciador =
                FindAnyObjectByType<GerenciadorFase>();


            if (gerenciador != null)
            {
                // Se morreu o jogador.
                if (CompareTag("Player"))
                {
                    gerenciador.JogadorMorreu();
                }

                // Se morreu um inimigo.
                else if (CompareTag("Enemy"))
                {
                    gerenciador.InimigoMorreu();
                }
            }


            // Destrói o objeto se estiver configurado.
            if (destruirAoMorrer)
            {
                Destroy(gameObject);
            }
        }
    }


    // ============================================================
    // CURAR
    // ============================================================

    /// <summary>
    /// Recupera uma quantidade de vida,
    /// sem aumentar a vida máxima.
    ///
    /// Exemplo:
    ///
    /// 3 corações máximos
    /// 1 coração atual
    ///
    /// Curar(2)
    ///
    /// Resultado:
    /// 3/3
    /// </summary>
    public void Curar(int quantidade)
    {
        // Não pode curar quem já morreu.
        if (VidaAtual <= 0)
            return;

        // Não permite valores negativos.
        if (quantidade <= 0)
            return;

        // Recupera a vida.
        VidaAtual = Mathf.Min(
            vidaMaxima,
            VidaAtual + quantidade
        );

        // Atualiza a interface.
        AoMudarVida?.Invoke(
            VidaAtual,
            vidaMaxima
        );

        Debug.Log(
            "❤️ Vida regenerada! " +
            VidaAtual + "/" + vidaMaxima
        );
    }


    // ============================================================
    // POWER UP DE VIDA
    // ============================================================

    /// <summary>
    /// Aplica o Power Up de vida.
    ///
    /// O comportamento depende da opção escolhida
    /// no Inspector.
    /// </summary>
    public void AplicarPowerUpVida(int quantidade)
    {
        // Verifica qual modo foi escolhido.
        switch (tipoEfeitoPowerUp)
        {
            // ====================================================
            // MODO 1 - REGENERAR VIDA
            // ====================================================

            case TipoEfeitoVida.RegenerarVida:

                Curar(quantidadeRegenerada);

                Debug.Log(
                    "❤️ Power Up: regeneração de vida."
                );

                break;


            // ====================================================
            // MODO 2 - ADICIONAR CORAÇÃO
            // ====================================================

            case TipoEfeitoVida.AdicionarCoracao:

                AdicionarCoracao(quantidade);

                Debug.Log(
                    "❤️ Power Up: novo coração adicionado."
                );

                break;
        }
    }


    // ============================================================
    // ADICIONAR CORAÇÃO
    // ============================================================

    /// <summary>
    /// Aumenta a vida máxima do jogador.
    ///
    /// Exemplo:
    ///
    /// Antes:
    /// ❤️ ❤️ ❤️
    ///
    /// Depois:
    /// ❤️ ❤️ ❤️ ❤️
    /// </summary>
    public void AdicionarCoracao(int quantidade)
    {
        // Impede valores inválidos.
        if (quantidade <= 0)
            return;

        // Verifica se já chegou ao limite.
        if (vidaMaxima >= limiteMaximoDeCorações)
        {
            Debug.Log(
                "❤️ O jogador já possui o máximo de corações."
            );

            return;
        }


        // Guarda a quantidade anterior.
        int vidaMaximaAnterior = vidaMaxima;


        // Aumenta a vida máxima.
        vidaMaxima += quantidade;


        // Não ultrapassa o limite configurado.
        vidaMaxima = Mathf.Min(
            vidaMaxima,
            limiteMaximoDeCorações
        );


        // Calcula quantos corações realmente foram adicionados.
        int coracoesAdicionados =
            vidaMaxima - vidaMaximaAnterior;


        // Também adiciona os novos corações à vida atual.
        VidaAtual += coracoesAdicionados;


        // Garante que a vida atual não ultrapasse o máximo.
        VidaAtual = Mathf.Min(
            VidaAtual,
            vidaMaxima
        );


        // Atualiza a interface.
        AoMudarVida?.Invoke(
            VidaAtual,
            vidaMaxima
        );


        Debug.Log(
            "❤️ Novo coração adicionado!"
        );

        Debug.Log(
            "Vida: " +
            VidaAtual +
            "/" +
            vidaMaxima
        );
    }
}