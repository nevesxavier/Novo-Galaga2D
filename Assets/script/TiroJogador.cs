
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla os dois disparos da nave.
/// A nave dispara continuamente enquanto Espaço
/// ou o botão esquerdo do mouse estiver pressionado.
/// </summary>
public class TiroJogador : MonoBehaviour
{
    [Header("TIRO")]

    [Tooltip("Prefab do tiro.")]
    [SerializeField]
    private GameObject tiroPrefab;

    [Tooltip("Primeiro ponto de disparo da nave.")]
    [SerializeField]
    private Transform pontoDeDisparo1;

    [Tooltip("Segundo ponto de disparo da nave.")]
    [SerializeField]
    private Transform pontoDeDisparo2;

    [Tooltip("Velocidade normal do tiro.")]
    [SerializeField]
    private float velocidadeDoTiro = 15f;

    [Tooltip("Tempo de vida do tiro.")]
    [SerializeField]
    private float tempoDeVidaDoTiro = 5f;

    [Header("CONTROLE")]

    [Tooltip("Tecla usada para disparar.")]
    [SerializeField]
    private Key teclaDeTiro = Key.Space;

    [Tooltip("Intervalo entre os disparos, em segundos.")]
    [SerializeField]
    private float intervaloEntreTiros = 0.2f;

    private ControleJogador controleJogador;
    private float tempoParaProximoTiro = 0f;

    private void Awake()
    {
        controleJogador = GetComponent<ControleJogador>();

        if (controleJogador == null)
        {
            Debug.LogWarning(
                "TiroJogador: a nave não possui ControleJogador.",
                this
            );
        }
    }

    private void Update()
    {
        if (Keyboard.current == null &&
            Mouse.current == null)
        {
            return;
        }

        bool espacoPressionado =
            Keyboard.current != null &&
            Keyboard.current[teclaDeTiro].isPressed;

        bool mousePressionado =
            Mouse.current != null &&
            Mouse.current.leftButton.isPressed;

        // Dispara enquanto Espaço ou o botão esquerdo
        // do mouse estiver pressionado.
        if (espacoPressionado || mousePressionado)
        {
            if (Time.time >= tempoParaProximoTiro)
            {
                Atirar();

                tempoParaProximoTiro =
                    Time.time + Mathf.Max(0.01f, intervaloEntreTiros);
            }
        }
    }

    private void Atirar()
    {
        if (tiroPrefab == null)
        {
            Debug.LogWarning(
                "TiroJogador: configure o prefab do tiro.",
                this
            );
            return;
        }

        if (pontoDeDisparo1 == null ||
            pontoDeDisparo2 == null)
        {
            Debug.LogWarning(
                "TiroJogador: configure os dois pontos de disparo.",
                this
            );
            return;
        }

        float velocidadeFinal = velocidadeDoTiro;

        // Mantém o efeito do Power-Up de velocidade.
        if (controleJogador != null)
        {
            velocidadeFinal *=
                controleJogador.MultiplicadorVelocidadeTiro;
        }

        CriarTiro(pontoDeDisparo1, velocidadeFinal);
        CriarTiro(pontoDeDisparo2, velocidadeFinal);
    }

    private void CriarTiro(
        Transform pontoDeDisparo,
        float velocidade)
    {
        if (tiroPrefab == null || pontoDeDisparo == null)
        {
            return;
        }

        GameObject tiro = Instantiate(
            tiroPrefab,
            pontoDeDisparo.position,
            pontoDeDisparo.rotation
        );

        Projetil projetil = tiro.GetComponent<Projetil>();

        if (projetil != null)
        {
            projetil.Configurar(velocidade);
            projetil.ConfigurarTempoDeVida(tempoDeVidaDoTiro);
        }
        else
        {
            Debug.LogWarning(
                "TiroJogador: o prefab não possui o componente Projetil.",
                tiro
            );
        }
    }
}
