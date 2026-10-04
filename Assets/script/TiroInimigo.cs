using UnityEngine;

/// <summary>
/// Controla os tiros do inimigo.
/// O inimigo dispara automaticamente,
/// sem precisar apertar nenhum botão.
/// </summary>
public class TiroInimigo : MonoBehaviour
{
    [Header("TIRO DO INIMIGO")]

    // Prefab do tiro que será criado
    [SerializeField] private GameObject projetilPrefab;

    // Pontos de onde os tiros serão criados
    [SerializeField] private Transform[] pontosDeTiro;

    // Tempo entre cada disparo
    [SerializeField] private float intervaloEntreTiros = 2f;

    // Controla quando será o próximo disparo
    private float proximoTiro;


    private void Start()
    {
        // Permite que o inimigo faça o primeiro disparo
        // depois que o intervalo inicial passar
        proximoTiro = Time.time + intervaloEntreTiros;
    }


    private void Update()
    {
        // Verifica se chegou a hora de disparar
        if (Time.time >= proximoTiro)
        {
            Atirar();
        }
    }


    private void Atirar()
    {
        // Define o próximo momento em que o inimigo poderá atirar
        proximoTiro = Time.time + intervaloEntreTiros;


        // Verifica se o prefab do tiro foi configurado
        if (projetilPrefab == null)
        {
            Debug.LogError(
                "ERRO: O Projetil Prefab do TiroInimigo não foi configurado!"
            );

            return;
        }


        // Se não houver pontos de tiro,
        // dispara do centro do inimigo
        if (pontosDeTiro == null || pontosDeTiro.Length == 0)
        {
            Instantiate(
                projetilPrefab,
                transform.position,
                Quaternion.identity
            );

            return;
        }


        // Se houver pontos de tiro,
        // cria um tiro em cada ponto
        foreach (Transform ponto in pontosDeTiro)
        {
            if (ponto == null)
                continue;

            Instantiate(
                projetilPrefab,
                ponto.position,
                Quaternion.identity
            );
        }
    }
}