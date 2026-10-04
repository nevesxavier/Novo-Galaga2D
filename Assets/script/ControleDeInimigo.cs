using UnityEngine;

/// <summary>
/// Controla o movimento do inimigo.
/// O inimigo se movimenta entre posições aleatórias
/// dentro dos limites definidos no Inspector.
/// </summary>
public class ControleInimigo : MonoBehaviour
{
    [Header("MOVIMENTO DO INIMIGO")]

    // Velocidade com que o inimigo se movimenta
    [SerializeField] private float velocidade = 2f;

    // Distância mínima para considerar que chegou ao destino
    [SerializeField] private float distanciaMinima = 0.1f;


    [Header("LIMITES DA TELA")]

    // Limite esquerdo da área de movimento
    [SerializeField] private float limiteXEsquerdo = -7f;

    // Limite direito da área de movimento
    [SerializeField] private float limiteXDireito = 7f;

    // Limite inferior da área de movimento
    [SerializeField] private float limiteYBaixo = -4f;

    // Limite superior da área de movimento
    [SerializeField] private float limiteYCima = 4f;


    // Próxima posição que o inimigo deverá alcançar
    private Vector3 destino;


    void Start()
    {
        // Escolhe a primeira posição para o inimigo ir
        EscolherNovoDestino();
    }


    void Update()
    {
        // Move o inimigo em direção ao destino
        transform.position = Vector3.MoveTowards(
            transform.position,
            destino,
            velocidade * Time.deltaTime
        );


        // Verifica se o inimigo chegou perto o suficiente
        if (Vector3.Distance(transform.position, destino) <= distanciaMinima)
        {
            // Escolhe outro destino
            EscolherNovoDestino();
        }
    }


    /// <summary>
    /// Escolhe uma nova posição aleatória dentro dos limites.
    /// </summary>
    private void EscolherNovoDestino()
    {
        float novaPosicaoX = Random.Range(
            limiteXEsquerdo,
            limiteXDireito
        );

        float novaPosicaoY = Random.Range(
            limiteYBaixo,
            limiteYCima
        );

        destino = new Vector3(
            novaPosicaoX,
            novaPosicaoY,
            transform.position.z
        );
    }
}