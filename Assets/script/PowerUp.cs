using UnityEngine;

/// <summary>
/// Controla os Power Ups que o jogador pode pegar.
///
/// Tipos disponíveis:
/// - Vida
/// - Velocidade do Tiro
///
/// O Power Up se movimenta da direita para a esquerda.
/// </summary>
public class PowerUp : MonoBehaviour
{
    // ============================================================
    // TIPOS DE POWER UP
    // ============================================================

    public enum TipoPowerUp
    {
        Vida,
        VelocidadeTiro
    }


    // ============================================================
    // TIPO
    // ============================================================

    [Header("TIPO DO POWER UP")]

    [Tooltip("Escolha qual Power Up este objeto representa.")]
    [SerializeField]
    private TipoPowerUp tipo = TipoPowerUp.Vida;


    // ============================================================
    // POWER UP DE VIDA
    // ============================================================

    [Header("POWER UP DE VIDA")]

    [Tooltip("Quantidade de vida recuperada.")]
    [SerializeField]
    private int quantidadeDeVida = 1;


    // ============================================================
    // POWER UP DE VELOCIDADE DO TIRO
    // ============================================================

    [Header("POWER UP - VELOCIDADE DO TIRO")]

    [Tooltip("Multiplica a velocidade normal do tiro.")]
    [SerializeField]
    private float multiplicadorVelocidadeTiro = 2f;

    [Tooltip("Tempo que o aumento da velocidade ficará ativo.")]
    [SerializeField]
    private float duracaoVelocidadeTiro = 5f;


    // ============================================================
    // MOVIMENTO
    // ============================================================

    [Header("MOVIMENTO")]

    [Tooltip("Velocidade do Power Up indo para a esquerda.")]
    [SerializeField]
    private float velocidade = 2f;


    // ============================================================
    // LIMITE DA TELA
    // ============================================================

    [Header("LIMITE DA TELA")]

    [Tooltip("Posição X onde o Power Up será destruído.")]
    [SerializeField]
    private float limiteEsquerdo = -12f;


    // ============================================================
    // UPDATE
    // ============================================================

    private void Update()
    {
        // Move o Power Up para a esquerda.
        transform.Translate(
            Vector3.left *
            velocidade *
            Time.deltaTime
        );


        // Se saiu da tela, destrói.
        if (transform.position.x < limiteEsquerdo)
        {
            Destroy(gameObject);
        }
    }


    // ============================================================
    // PEGAR POWER UP
    // ============================================================

    private void OnTriggerEnter2D(
        Collider2D outro
    )
    {
        // Só a nave pode pegar o Power Up.
        if (!outro.CompareTag("Player"))
        {
            return;
        }


        // ========================================================
        // POWER UP DE VIDA
        // ========================================================

        if (tipo == TipoPowerUp.Vida)
        {
            // Procura o componente Vida da nave.
            Vida vidaJogador =
                outro.GetComponent<Vida>();


            // Verifica se encontrou.
            if (vidaJogador == null)
            {
                Debug.LogWarning(
                    "PowerUp: a nave não possui o componente Vida."
                );

                return;
            }


            // Recupera a vida.
            vidaJogador.Curar(
                quantidadeDeVida
            );


            Debug.Log(
                "Power Up de VIDA coletado! " +
                "Quantidade: " +
                quantidadeDeVida
            );
        }


        // ========================================================
        // POWER UP DE VELOCIDADE
        // ========================================================

        else if (
            tipo == TipoPowerUp.VelocidadeTiro
        )
        {
            // Procura o ControleJogador da nave.
            ControleJogador controleJogador =
                outro.GetComponent<ControleJogador>();


            // Verifica se encontrou.
            if (controleJogador == null)
            {
                Debug.LogWarning(
                    "PowerUp: a nave não possui o componente ControleJogador."
                );

                return;
            }


            // ----------------------------------------------------
            // CRIA OS DADOS DO POWER UP
            // ----------------------------------------------------

            DadosPowerUpTiro dados =
                new DadosPowerUpTiro();


            // Envia o multiplicador configurado
            // no Inspector.
            dados.multiplicador =
                multiplicadorVelocidadeTiro;


            // Envia a duração configurada
            // no Inspector.
            dados.duracao =
                duracaoVelocidadeTiro;


            // ----------------------------------------------------
            // ATIVA O POWER UP
            // ----------------------------------------------------

            controleJogador
                .AtivarVelocidadeTiro(
                    dados
                );


            Debug.Log(
                "Power Up de VELOCIDADE coletado! " +
                "Multiplicador: " +
                multiplicadorVelocidadeTiro +
                "x | Duração: " +
                duracaoVelocidadeTiro +
                " segundos."
            );
        }


        // ========================================================
        // DESTRUIR POWER UP
        // ========================================================

        Destroy(gameObject);
    }
}