using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla a nave do jogador em um jogo horizontal (esquerda -> direita).
/// A nave se move apenas para cima e para baixo e fica limitada à tela.
/// Usa o novo Input System (pacote com.unity.inputsystem).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ControleJogador : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidade = 6f;

    [Header("Limites da tela")]
    [Tooltip("Margem em unidades do mundo, para a nave não ficar metade para fora da tela.")]
    [SerializeField] private float margem = 0.5f;

    private Rigidbody2D rb;
    private float entradaVertical;
    private float limiteInferior;
    private float limiteSuperior;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;   // sem gravidade
        rb.freezeRotation = true;
    }

    private void Start()
    {
        CalcularLimites();
    }

    private void Update()
    {
        entradaVertical = LerEntradaVertical();
    }

    private void FixedUpdate()
    {
        // Move só no eixo Y; o X fica parado
        Vector2 posicao = rb.position;
        posicao.y += entradaVertical * velocidade * Time.fixedDeltaTime;
        posicao.y = Mathf.Clamp(posicao.y, limiteInferior, limiteSuperior);

        rb.MovePosition(posicao);
    }

    // W / Seta para cima = +1 | S / Seta para baixo = -1 | analógico esquerdo do controle também funciona
    private float LerEntradaVertical()
    {
        float valor = 0f;

        Keyboard teclado = Keyboard.current;
        if (teclado != null)
        {
            if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) valor += 1f;
            if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) valor -= 1f;
        }

        Gamepad controle = Gamepad.current;
        if (controle != null && Mathf.Abs(valor) < 0.01f)
        {
            valor = controle.leftStick.y.ReadValue();
        }

        return Mathf.Clamp(valor, -1f, 1f);
    }

    private void CalcularLimites()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("ControleJogador: nenhuma câmera com a tag MainCamera encontrada.");
            limiteInferior = -4f;
            limiteSuperior = 4f;
            return;
        }

        // Converte a parte de baixo (0) e de cima (1) da tela para coordenadas do mundo
        limiteInferior = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y + margem;
        limiteSuperior = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y - margem;
    }
}
