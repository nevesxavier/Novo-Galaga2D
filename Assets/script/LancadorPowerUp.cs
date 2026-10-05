using UnityEngine;

/// <summary>
/// Faz o inimigo lançar Power Ups durante a fase.
///
/// O inimigo pode lançar uma quantidade limitada
/// de Power Ups, com um intervalo configurável.
/// </summary>
public class LancadorPowerUp : MonoBehaviour
{
    // ============================================================
    // POWER UP
    // ============================================================

    [Header("POWER UP")]

    [Tooltip("Prefab do Power Up que será lançado.")]
    [SerializeField]
    private GameObject powerUpPrefab;


    // ============================================================
    // QUANTIDADE
    // ============================================================

    [Header("QUANTIDADE")]

    [Tooltip("Quantidade máxima de Power Ups que este inimigo poderá lançar.")]
    [SerializeField]
    private int quantidadeMaxima = 3;


    // ============================================================
    // TEMPO
    // ============================================================

    [Header("TEMPO")]

    [Tooltip("Tempo que o inimigo espera antes do primeiro lançamento.")]
    [SerializeField]
    private float tempoParaPrimeiroLancamento = 2f;

    [Tooltip("Intervalo entre cada lançamento.")]
    [SerializeField]
    private float intervaloEntreLancamentos = 5f;


    // ============================================================
    // POSIÇÃO DO LANÇAMENTO
    // ============================================================

    [Header("POSIÇÃO DO LANÇAMENTO")]

    [Tooltip("Deslocamento horizontal do ponto onde o Power Up nasce.")]
    [SerializeField]
    private float deslocamentoX = 0f;

    [Tooltip("Deslocamento vertical do ponto onde o Power Up nasce.")]
    [SerializeField]
    private float deslocamentoY = -0.5f;


    // ============================================================
    // CONTROLE INTERNO
    // ============================================================

    // Quantos Power Ups já foram lançados.
    private int quantidadeLancada = 0;


    // ============================================================
    // INÍCIO
    // ============================================================

    private void Start()
    {
        // Verifica se existe um prefab configurado.
        if (powerUpPrefab == null)
        {
            Debug.LogWarning(
                "LancadorPowerUp: nenhum Power Up foi configurado no inimigo."
            );

            return;
        }


        // Verifica se a quantidade é válida.
        if (quantidadeMaxima <= 0)
        {
            return;
        }


        // Começa a rotina de lançamento.
        InvokeRepeating(
            nameof(LancarPowerUp),
            tempoParaPrimeiroLancamento,
            intervaloEntreLancamentos
        );
    }


    // ============================================================
    // LANÇAR POWER UP
    // ============================================================

    private void LancarPowerUp()
    {
        // Verifica se já atingiu a quantidade máxima.
        if (quantidadeLancada >= quantidadeMaxima)
        {
            // Para de chamar a função.
            CancelInvoke(
                nameof(LancarPowerUp)
            );

            return;
        }


        // Calcula a posição onde o Power Up vai nascer.
        Vector3 posicaoLancamento =
            transform.position +
            new Vector3(
                deslocamentoX,
                deslocamentoY,
                0f
            );


        // Cria o Power Up na cena.
        Instantiate(
            powerUpPrefab,
            posicaoLancamento,
            Quaternion.identity
        );


        // Conta o Power Up lançado.
        quantidadeLancada++;


        Debug.Log(
            "Power Up lançado pelo inimigo! " +
            quantidadeLancada +
            "/" +
            quantidadeMaxima
        );
    }


    // ============================================================
    // SEGURANÇA
    // ============================================================

    private void OnDestroy()
    {
        // Cancela o lançamento caso o inimigo seja destruído.
        CancelInvoke(
            nameof(LancarPowerUp)
        );
    }
}