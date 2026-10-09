
using UnityEngine;

/// <summary>
/// Controla os Power Ups que o jogador pode coletar.
/// </summary>
public class PowerUp : MonoBehaviour
{
    public enum TipoPowerUp
    {
        Vida,
        VelocidadeTiro
    }

    public enum EfeitoPowerUpVida
    {
        RecuperarVida,
        AdicionarCoracao
    }

    [Header("TIPO DO POWER UP")]

    [SerializeField]
    private TipoPowerUp tipo = TipoPowerUp.Vida;

    [Header("POWER UP DE VIDA")]

    [Tooltip("Escolha entre recuperar vida ou adicionar um coração.")]
    [SerializeField]
    private EfeitoPowerUpVida efeitoDeVida =
        EfeitoPowerUpVida.AdicionarCoracao;

    [Tooltip("Quantidade de vida recuperada.")]
    [SerializeField]
    private int quantidadeDeVida = 1;

    [Header("POWER UP - VELOCIDADE DO TIRO")]

    [SerializeField]
    private float multiplicadorVelocidadeTiro = 2f;

    [SerializeField]
    private float duracaoVelocidadeTiro = 5f;

    [Header("MOVIMENTO")]

    [SerializeField]
    private float velocidade = 2f;

    [Header("LIMITE DA TELA")]

    [SerializeField]
    private float limiteEsquerdo = -12f;

    private void Update()
    {
        transform.Translate(
            Vector3.left * velocidade * Time.deltaTime
        );

        if (transform.position.x < limiteEsquerdo)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (!outro.CompareTag("Player"))
            return;

        if (tipo == TipoPowerUp.Vida)
        {
            Vida vidaJogador = outro.GetComponent<Vida>();

            if (vidaJogador == null)
            {
                Debug.LogWarning(
                    "PowerUp: a nave não possui o componente Vida."
                );
                return;
            }

            if (efeitoDeVida == EfeitoPowerUpVida.AdicionarCoracao)
            {
                vidaJogador.AdicionarCoracao(quantidadeDeVida);

                Debug.Log(
                    "Power Up: coração adicional solicitado. Quantidade: " +
                    quantidadeDeVida
                );
            }
            else
            {
                vidaJogador.Curar(quantidadeDeVida);

                Debug.Log(
                    "Power Up: vida recuperada. Quantidade: " +
                    quantidadeDeVida
                );
            }
        }
        else if (tipo == TipoPowerUp.VelocidadeTiro)
        {
            ControleJogador controleJogador =
                outro.GetComponent<ControleJogador>();

            if (controleJogador == null)
            {
                Debug.LogWarning(
                    "PowerUp: a nave não possui o componente ControleJogador."
                );
                return;
            }

            DadosPowerUpTiro dados = new DadosPowerUpTiro
            {
                multiplicador = multiplicadorVelocidadeTiro,
                duracao = duracaoVelocidadeTiro
            };

            controleJogador.AtivarVelocidadeTiro(dados);

            Debug.Log(
                "Power Up de velocidade coletado! Multiplicador: " +
                multiplicadorVelocidadeTiro +
                "x | Duração: " +
                duracaoVelocidadeTiro + " segundos."
            );
        }

        Destroy(gameObject);
    }
}

