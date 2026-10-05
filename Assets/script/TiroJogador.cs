using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla os dois disparos da nave.
///
/// Ao apertar Espaço, a nave dispara
/// pelos dois pontos de disparo ao mesmo tempo.
///
/// A velocidade dos tiros pode ser aumentada
/// temporariamente através do Power Up.
/// </summary>
public class TiroJogador : MonoBehaviour
{
    // ============================================================
    // TIRO
    // ============================================================

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


    // ============================================================
    // CONTROLE
    // ============================================================

    [Header("CONTROLE")]

    [Tooltip("Tecla usada para disparar.")]
    [SerializeField]
    private Key teclaDeTiro = Key.Space;


    // ============================================================
    // REFERÊNCIA
    // ============================================================

    private ControleJogador controleJogador;


    // ============================================================
    // AWAKE
    // ============================================================

    private void Awake()
    {
        // Procura o ControleJogador na nave.
        controleJogador =
            GetComponent<ControleJogador>();

        if (controleJogador == null)
        {
            Debug.LogWarning(
                "TiroJogador: a nave não possui ControleJogador."
            );
        }
    }


    // ============================================================
    // UPDATE
    // ============================================================

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }


        // Dispara quando apertar Espaço.
        if (
            Keyboard.current[
                teclaDeTiro
            ].wasPressedThisFrame
        )
        {
            Atirar();
        }
    }


    // ============================================================
    // ATIRAR
    // ============================================================

    private void Atirar()
    {
        // Verifica se o prefab existe.
        if (tiroPrefab == null)
        {
            Debug.LogWarning(
                "TiroJogador: Tiro Prefab não foi configurado."
            );

            return;
        }


        // Verifica os dois pontos.
        if (
            pontoDeDisparo1 == null ||
            pontoDeDisparo2 == null
        )
        {
            Debug.LogWarning(
                "TiroJogador: os dois pontos de disparo precisam ser configurados."
            );

            return;
        }


        // ========================================================
        // CALCULA A VELOCIDADE
        // ========================================================

        float velocidadeFinal =
            velocidadeDoTiro;


        // Verifica se existe Power Up.
        if (controleJogador != null)
        {
            velocidadeFinal =
                velocidadeDoTiro *
                controleJogador
                    .MultiplicadorVelocidadeTiro;
        }


        // ========================================================
        // PRIMEIRO TIRO
        // ========================================================

        CriarTiro(
            pontoDeDisparo1,
            velocidadeFinal
        );


        // ========================================================
        // SEGUNDO TIRO
        // ========================================================

        CriarTiro(
            pontoDeDisparo2,
            velocidadeFinal
        );
    }


    // ============================================================
    // CRIAR UM TIRO
    // ============================================================

    private void CriarTiro(
        Transform pontoDeDisparo,
        float velocidade
    )
    {
        // Cria o tiro no ponto indicado.
        GameObject tiro =
            Instantiate(
                tiroPrefab,
                pontoDeDisparo.position,
                pontoDeDisparo.rotation
            );


        // Procura o componente Projetil.
        Projetil projetil =
            tiro.GetComponent<Projetil>();


        if (projetil != null)
        {
            // Configura a velocidade.
            projetil.Configurar(
                velocidade
            );


            // Configura o tempo de vida.
            projetil.ConfigurarTempoDeVida(
                tempoDeVidaDoTiro
            );
        }
        else
        {
            Debug.LogWarning(
                "TiroJogador: o prefab do tiro não possui o componente Projetil."
            );
        }
    }
}