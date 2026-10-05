using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla a movimentação vertical da nave
/// e também os Power Ups relacionados
/// à velocidade do tiro.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ControleJogador : MonoBehaviour
{
    // ============================================================
    // MOVIMENTO DA NAVE
    // ============================================================

    [Header("MOVIMENTO")]

    [Tooltip("Velocidade de movimento da nave.")]
    [SerializeField]
    private float velocidade = 6f;


    // ============================================================
    // LIMITES DA TELA
    // ============================================================

    [Header("LIMITES DA TELA")]

    [Tooltip("Margem para a nave não sair da tela.")]
    [SerializeField]
    private float margem = 0.5f;


    // ============================================================
    // POWER UP - VELOCIDADE DO TIRO
    // ============================================================

    [Header("POWER UP - VELOCIDADE DO TIRO")]

    [Tooltip("Indica se o Power Up de velocidade do tiro está ativo.")]
    [SerializeField]
    private bool velocidadeTiroAtiva = false;

    [Tooltip("Multiplicador atual da velocidade do tiro.")]
    [SerializeField]
    private float multiplicadorVelocidadeTiro = 1f;


    // ============================================================
    // VARIÁVEIS INTERNAS
    // ============================================================

    private Rigidbody2D rb;

    private float entradaVertical;

    private float limiteInferior;

    private float limiteSuperior;

    private Coroutine rotinaVelocidadeTiro;


    // ============================================================
    // PROPRIEDADES
    // ============================================================

    /// <summary>
    /// Informa se o Power Up de velocidade está ativo.
    /// </summary>
    public bool VelocidadeTiroAtiva
    {
        get
        {
            return velocidadeTiroAtiva;
        }
    }


    /// <summary>
    /// Retorna o multiplicador atual da velocidade do tiro.
    /// 
    /// Normal = 1
    /// Power Up x2 = 2
    /// Power Up x3 = 3
    /// </summary>
    public float MultiplicadorVelocidadeTiro
    {
        get
        {
            return multiplicadorVelocidadeTiro;
        }
    }


    // ============================================================
    // AWAKE
    // ============================================================

    private void Awake()
    {
        // Pega o Rigidbody2D da nave.
        rb = GetComponent<Rigidbody2D>();

        // Remove a gravidade.
        rb.gravityScale = 0f;

        // Impede a nave de girar.
        rb.freezeRotation = true;
    }


    // ============================================================
    // START
    // ============================================================

    private void Start()
    {
        // Calcula os limites da tela.
        CalcularLimites();
    }


    // ============================================================
    // UPDATE
    // ============================================================

    private void Update()
    {
        // Lê o teclado/controle.
        entradaVertical = LerEntradaVertical();
    }


    // ============================================================
    // FIXED UPDATE
    // ============================================================

    private void FixedUpdate()
    {
        // Pega a posição atual da nave.
        Vector2 posicao = rb.position;

        // Move somente no eixo Y.
        posicao.y +=
            entradaVertical *
            velocidade *
            Time.fixedDeltaTime;

        // Impede a nave de sair da tela.
        posicao.y = Mathf.Clamp(
            posicao.y,
            limiteInferior,
            limiteSuperior
        );

        // Aplica a nova posição.
        rb.MovePosition(posicao);
    }


    // ============================================================
    // LEITURA DO CONTROLE
    // ============================================================

    /// <summary>
    /// Lê W, S, setas e analógico do controle.
    /// </summary>
    private float LerEntradaVertical()
    {
        float valor = 0f;

        // --------------------------------------------------------
        // TECLADO
        // --------------------------------------------------------

        Keyboard teclado = Keyboard.current;

        if (teclado != null)
        {
            // W ou seta para cima.
            if (
                teclado.wKey.isPressed ||
                teclado.upArrowKey.isPressed
            )
            {
                valor += 1f;
            }

            // S ou seta para baixo.
            if (
                teclado.sKey.isPressed ||
                teclado.downArrowKey.isPressed
            )
            {
                valor -= 1f;
            }
        }


        // --------------------------------------------------------
        // CONTROLE
        // --------------------------------------------------------

        Gamepad controle = Gamepad.current;

        if (
            controle != null &&
            Mathf.Abs(valor) < 0.01f
        )
        {
            valor =
                controle.leftStick.y.ReadValue();
        }


        // Garante que o valor fique entre -1 e 1.
        return Mathf.Clamp(
            valor,
            -1f,
            1f
        );
    }


    // ============================================================
    // CALCULAR LIMITES DA TELA
    // ============================================================

    private void CalcularLimites()
    {
        Camera cam = Camera.main;

        // Se não encontrou a câmera.
        if (cam == null)
        {
            Debug.LogWarning(
                "ControleJogador: nenhuma câmera com a tag MainCamera encontrada."
            );

            // Valores de segurança.
            limiteInferior = -4f;
            limiteSuperior = 4f;

            return;
        }


        // Parte inferior da tela.
        limiteInferior =
            cam.ViewportToWorldPoint(
                new Vector3(0f, 0f, 0f)
            ).y + margem;


        // Parte superior da tela.
        limiteSuperior =
            cam.ViewportToWorldPoint(
                new Vector3(0f, 1f, 0f)
            ).y - margem;
    }


    // ============================================================
    // POWER UP - VELOCIDADE DO TIRO
    // ============================================================

    /// <summary>
    /// Ativa o Power Up de velocidade do tiro.
    /// </summary>
    public void AtivarVelocidadeTiro(
        DadosPowerUpTiro dados
    )
    {
        // Verifica se recebeu dados.
        if (dados == null)
        {
            Debug.LogWarning(
                "ControleJogador: dados do Power Up de tiro são nulos."
            );

            return;
        }


        // Impede valores inválidos.
        if (dados.multiplicador <= 0f)
        {
            Debug.LogWarning(
                "ControleJogador: o multiplicador do Power Up precisa ser maior que zero."
            );

            return;
        }


        if (dados.duracao <= 0f)
        {
            Debug.LogWarning(
                "ControleJogador: a duração do Power Up precisa ser maior que zero."
            );

            return;
        }


        // --------------------------------------------------------
        // SE JÁ EXISTIA UM POWER UP ATIVO
        // --------------------------------------------------------

        // Para a contagem anterior.
        if (rotinaVelocidadeTiro != null)
        {
            StopCoroutine(rotinaVelocidadeTiro);
        }


        // --------------------------------------------------------
        // ATIVA O POWER UP
        // --------------------------------------------------------

        velocidadeTiroAtiva = true;

        multiplicadorVelocidadeTiro =
            dados.multiplicador;


        Debug.Log(
            "POWER UP DE VELOCIDADE ATIVADO! " +
            "Multiplicador: " +
            multiplicadorVelocidadeTiro +
            " | Duração: " +
            dados.duracao +
            " segundos."
        );


        // Começa a contagem da duração.
        rotinaVelocidadeTiro =
            StartCoroutine(
                ContarDuracaoVelocidadeTiro(
                    dados.duracao
                )
            );
    }


    // ============================================================
    // CONTAGEM DO TEMPO DO POWER UP
    // ============================================================

    private IEnumerator ContarDuracaoVelocidadeTiro(
        float duracao
    )
    {
        // Espera o tempo configurado.
        yield return new WaitForSeconds(
            duracao
        );


        // --------------------------------------------------------
        // DESATIVA O POWER UP
        // --------------------------------------------------------

        velocidadeTiroAtiva = false;

        // Volta ao multiplicador normal.
        multiplicadorVelocidadeTiro = 1f;


        Debug.Log(
            "POWER UP DE VELOCIDADE TERMINOU. " +
            "Velocidade do tiro voltou ao normal."
        );


        // Limpa a referência da Coroutine.
        rotinaVelocidadeTiro = null;
    }
}