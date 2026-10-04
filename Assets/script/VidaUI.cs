using UnityEngine;

/// <summary>
/// Controla os corações que representam a vida do jogador.
/// Cada coração representa uma vida.
/// </summary>
public class VidaUI : MonoBehaviour
{
    [Header("CORAÇÕES")]

    // Objetos dos corações da interface
    [SerializeField] private GameObject[] coracoes;


    [Header("VIDA DO JOGADOR")]

    // Referência para o script Vida da nave
    [SerializeField] private Vida vidaJogador;


    private void Start()
    {
        // Se a nave não estiver configurada no Inspector,
        // tenta encontrar automaticamente um objeto com a Tag Player.
        if (vidaJogador == null)
        {
            GameObject jogador = GameObject.FindGameObjectWithTag("Player");

            if (jogador != null)
            {
                vidaJogador = jogador.GetComponent<Vida>();
            }
        }


        // Verifica novamente se encontrou a nave
        if (vidaJogador == null)
        {
            Debug.LogError(
                "VidaUI: não foi possível encontrar o objeto do jogador com o componente Vida."
            );

            return;
        }


        // Atualiza os corações quando a fase começa
        AtualizarCoracoes(
            vidaJogador.VidaAtual,
            vidaJogador.VidaMaxima
        );


        // Começa a acompanhar as mudanças de vida
        vidaJogador.AoMudarVida += AtualizarCoracoes;
    }


    private void OnDestroy()
    {
        // Para de acompanhar o evento quando o VidaUI for destruído
        if (vidaJogador != null)
        {
            vidaJogador.AoMudarVida -= AtualizarCoracoes;
        }
    }


    /// <summary>
    /// Atualiza os corações de acordo com a vida atual.
    /// </summary>
    private void AtualizarCoracoes(int vidaAtual, int vidaMaxima)
    {
        // Percorre todos os corações
        for (int i = 0; i < coracoes.Length; i++)
        {
            // O coração aparece se ainda existir aquela vida
            bool deveAparecer = i < vidaAtual;

            // Ativa ou desativa o coração
            if (coracoes[i] != null)
            {
                coracoes[i].SetActive(deveAparecer);
            }
        }
    }
}