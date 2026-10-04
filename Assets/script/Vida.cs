using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controla a vida do jogador e dos inimigos.
///
/// Quando a vida chega a zero:
/// - Se for o jogador, chama o GameOver.
/// - Se for um inimigo, informa ao GerenciadorFase.
/// </summary>
public class Vida : MonoBehaviour
{
    [Header("VIDA")]

    // Quantidade máxima de vida
    [SerializeField] private int vidaMaxima = 3;

    // Se estiver marcado, o objeto será destruído quando morrer
    [SerializeField] private bool destruirAoMorrer = true;


    [Header("EVENTOS")]

    // Evento que pode ser configurado pelo Inspector
    // para explosão, som, animação etc.
    public UnityEvent AoMorrer;

    // Evento que informa:
    // primeiro valor = vida atual
    // segundo valor = vida máxima
    public event Action<int, int> AoMudarVida;


    // Vida atual do objeto
    public int VidaAtual { get; private set; }

    // Permite consultar a vida máxima
    public int VidaMaxima => vidaMaxima;


    private void Awake()
    {
        // Começa com a vida máxima
        VidaAtual = vidaMaxima;
    }


    /// <summary>
    /// Aplica dano ao jogador ou inimigo.
    /// </summary>
    public void ReceberDano(int dano)
    {
        // Se já estiver morto, não recebe mais dano
        if (VidaAtual <= 0)
            return;

        // Diminui a vida sem deixar ficar abaixo de zero
        VidaAtual = Mathf.Max(0, VidaAtual - dano);

        // Informa que a vida mudou
        AoMudarVida?.Invoke(VidaAtual, vidaMaxima);


        // Verifica se morreu
        if (VidaAtual == 0)
        {
            // Executa os eventos configurados no Inspector
            AoMorrer?.Invoke();


            // Procura o GerenciadorFase na cena
            // FindAnyObjectByType é o método atual recomendado pelo Unity
            GerenciadorFase gerenciador =
                FindAnyObjectByType<GerenciadorFase>();


            // Verifica se encontrou o GerenciadorFase
            if (gerenciador != null)
            {
                // Se quem morreu foi o jogador
                if (CompareTag("Player"))
                {
                    gerenciador.JogadorMorreu();
                }

                // Se quem morreu foi um inimigo
                else if (CompareTag("Enemy"))
                {
                    gerenciador.InimigoMorreu();
                }
            }


            // Destrói o objeto se essa opção estiver ativada
            if (destruirAoMorrer)
            {
                Destroy(gameObject);
            }
        }
    }


    /// <summary>
    /// Recupera uma quantidade de vida.
    /// </summary>
    public void Curar(int quantidade)
    {
        // Não pode curar um objeto que já morreu
        if (VidaAtual <= 0)
            return;

        // Aumenta a vida sem ultrapassar o máximo
        VidaAtual = Mathf.Min(
            vidaMaxima,
            VidaAtual + quantidade
        );

        // Informa que a vida mudou
        AoMudarVida?.Invoke(VidaAtual, vidaMaxima);
    }
}