using UnityEngine;

/// <summary>
/// Controla os corações que representam a vida do jogador.
///
/// Cada coração representa uma vida.
///
/// O sistema suporta até a quantidade de corações
/// configurada no Inspector.
/// </summary>
public class VidaUI : MonoBehaviour
{
    // ============================================================
    // CORAÇÕES
    // ============================================================

    [Header("CORAÇÕES")]

    [Tooltip("Coloque aqui os objetos dos corações na ordem.")]
    [SerializeField]
    private GameObject[] coracoes;


    // ============================================================
    // VIDA DO JOGADOR
    // ============================================================

    [Header("VIDA DO JOGADOR")]

    [Tooltip("Arraste aqui o objeto que possui o script Vida.")]
    [SerializeField]
    private Vida vidaJogador;


    // ============================================================
    // INICIALIZAÇÃO
    // ============================================================

    private void Start()
    {
        // Se a referência não foi colocada manualmente,
        // tenta encontrar automaticamente o jogador.
        if (vidaJogador == null)
        {
            GameObject jogador =
                GameObject.FindGameObjectWithTag("Player");


            if (jogador != null)
            {
                vidaJogador =
                    jogador.GetComponent<Vida>();
            }
        }


        // Verifica se encontrou o jogador.
        if (vidaJogador == null)
        {
            Debug.LogError(
                "VidaUI: não foi possível encontrar " +
                "o jogador com o componente Vida."
            );

            return;
        }


        // Verifica se existem corações configurados.
        if (coracoes == null ||
            coracoes.Length == 0)
        {
            Debug.LogError(
                "VidaUI: nenhum coração foi configurado " +
                "no campo Coracoes."
            );

            return;
        }


        // Atualiza os corações assim que a fase começa.
        AtualizarCoracoes(
            vidaJogador.VidaAtual,
            vidaJogador.VidaMaxima
        );


        // Começa a acompanhar qualquer alteração
        // na vida do jogador.
        vidaJogador.AoMudarVida += AtualizarCoracoes;
    }


    // ============================================================
    // QUANDO O OBJETO FOR DESTRUÍDO
    // ============================================================

    private void OnDestroy()
    {
        // Para de acompanhar o evento.
        if (vidaJogador != null)
        {
            vidaJogador.AoMudarVida -=
                AtualizarCoracoes;
        }
    }


    // ============================================================
    // ATUALIZAR CORAÇÕES
    // ============================================================

    /// <summary>
    /// Atualiza os corações da interface.
    ///
    /// Exemplo:
    ///
    /// Vida atual = 3
    /// Vida máxima = 3
    ///
    /// ❤️ ❤️ ❤️
    ///
    /// Se perder uma vida:
    ///
    /// ❤️ ❤️
    ///
    /// Se ganhar um coração:
    ///
    /// ❤️ ❤️ ❤️ ❤️
    /// </summary>
    private void AtualizarCoracoes(
        int vidaAtual,
        int vidaMaxima
    )
    {
        // Percorre todos os objetos de coração
        // configurados no Inspector.
        for (int i = 0;
             i < coracoes.Length;
             i++)
        {
            // Verifica se o objeto do coração existe.
            if (coracoes[i] == null)
            {
                continue;
            }


            // O coração aparece quando:
            //
            // índice 0 = primeira vida
            // índice 1 = segunda vida
            // índice 2 = terceira vida
            // índice 3 = quarta vida
            //
            // etc.
            bool deveAparecer =
                i < vidaAtual;


            // Ativa ou desativa o coração.
            coracoes[i].SetActive(
                deveAparecer
            );
        }


        // Verifica se a vida máxima é maior
        // que a quantidade de corações criados.
        if (vidaMaxima > coracoes.Length)
        {
            Debug.LogWarning(
                "VidaUI: o jogador possui " +
                vidaMaxima +
                " vidas, mas existem apenas " +
                coracoes.Length +
                " objetos de coração configurados."
            );
        }
    }
}