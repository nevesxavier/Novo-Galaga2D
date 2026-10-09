
using UnityEngine;

/// <summary>
/// Faz o inimigo lançar dois tipos de Power Ups durante a fase.
/// Permite configurar a quantidade máxima e o intervalo.
/// </summary>
public class LancadorPowerUp : MonoBehaviour
{
    [Header("POWER UPS")]

    [Tooltip("Primeiro prefab de Power Up.")]
    [SerializeField]
    private GameObject powerUpPrefab;

    [Tooltip("Segundo prefab de Power Up.")]
    [SerializeField]
    private GameObject segundoPowerUpPrefab;

    [Header("QUANTIDADE")]

    [Tooltip("Quantidade máxima de Power Ups que este inimigo poderá lançar.")]
    [SerializeField]
    private int quantidadeMaxima = 3;

    [Header("TEMPO")]

    [Tooltip("Tempo antes do primeiro lançamento.")]
    [SerializeField]
    private float tempoParaPrimeiroLancamento = 2f;

    [Tooltip("Intervalo entre os lançamentos.")]
    [SerializeField]
    private float intervaloEntreLancamentos = 5f;

    [Header("POSIÇÃO DO LANÇAMENTO")]

    [Tooltip("Deslocamento horizontal do lançamento.")]
    [SerializeField]
    private float deslocamentoX = 0f;

    [Tooltip("Deslocamento vertical do lançamento.")]
    [SerializeField]
    private float deslocamentoY = -0.5f;

    // Quantidade de Power Ups já lançados.
    private int quantidadeLancada = 0;

    // Alterna entre os dois prefabs.
    private bool usarSegundoPrefab = false;

    private void Start()
    {
        if (powerUpPrefab == null && segundoPowerUpPrefab == null)
        {
            Debug.LogWarning(
                "LancadorPowerUp: configure pelo menos um prefab no Inspector.",
                this
            );
            return;
        }

        if (quantidadeMaxima <= 0)
        {
            Debug.LogWarning(
                "LancadorPowerUp: a quantidade máxima deve ser maior que zero.",
                this
            );
            return;
        }

        if (intervaloEntreLancamentos <= 0f)
        {
            Debug.LogWarning(
                "LancadorPowerUp: o intervalo deve ser maior que zero.",
                this
            );
            return;
        }

        tempoParaPrimeiroLancamento =
            Mathf.Max(0f, tempoParaPrimeiroLancamento);

        InvokeRepeating(
            nameof(LancarPowerUp),
            tempoParaPrimeiroLancamento,
            intervaloEntreLancamentos
        );
    }

    private void LancarPowerUp()
    {
        if (!isActiveAndEnabled)
        {
            CancelInvoke(nameof(LancarPowerUp));
            return;
        }

        if (quantidadeLancada >= quantidadeMaxima)
        {
            CancelInvoke(nameof(LancarPowerUp));
            return;
        }

        GameObject prefabEscolhido = EscolherPrefab();

        if (prefabEscolhido == null)
        {
            Debug.LogWarning(
                "LancadorPowerUp: não há um prefab válido para lançar.",
                this
            );
            CancelInvoke(nameof(LancarPowerUp));
            return;
        }

        Vector3 posicaoLancamento = transform.position +
            new Vector3(deslocamentoX, deslocamentoY, 0f);

        GameObject novoPowerUp = Instantiate(
            prefabEscolhido,
            posicaoLancamento,
            Quaternion.identity
        );

        if (novoPowerUp == null)
        {
            Debug.LogWarning(
                "LancadorPowerUp: não foi possível criar o Power Up.",
                this
            );
            return;
        }

        quantidadeLancada++;

        Debug.Log(
            "Power Up lançado! " +
            quantidadeLancada + "/" + quantidadeMaxima,
            this
        );

        if (quantidadeLancada >= quantidadeMaxima)
        {
            CancelInvoke(nameof(LancarPowerUp));
        }
    }

    private GameObject EscolherPrefab()
    {
        GameObject prefabEscolhido;

        // Alterna entre os prefabs quando ambos estão configurados.
        if (powerUpPrefab != null && segundoPowerUpPrefab != null)
        {
            prefabEscolhido = usarSegundoPrefab
                ? segundoPowerUpPrefab
                : powerUpPrefab;

            usarSegundoPrefab = !usarSegundoPrefab;
        }
        else
        {
            // Se apenas um estiver configurado, usa esse prefab.
            prefabEscolhido = powerUpPrefab != null
                ? powerUpPrefab
                : segundoPowerUpPrefab;
        }

        return prefabEscolhido;
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(LancarPowerUp));
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(LancarPowerUp));
    }
}

