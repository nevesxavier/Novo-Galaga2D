using UnityEngine;

/// <summary>
/// Cria um fundo estrelado com 3 camadas.
///
/// As estrelas são criadas automaticamente por código.
/// Elas se movimentam da direita para a esquerda.
///
/// Ordem de desenho:
/// CenarioFase  -> 0
/// Estrelas     -> 1, 2 e 3
/// Nave/Inimigos/Tiros -> acima
/// </summary>
public class FundoEstrelado : MonoBehaviour
{
    // ============================================================
    // CAMADA 1 - ESTRELAS DISTANTES
    // ============================================================

    [Header("CAMADA 1 - ESTRELAS PEQUENAS")]

    [SerializeField]
    private int quantidadeCamada1 = 80;

    [SerializeField]
    private float velocidadeMinimaCamada1 = 0.15f;

    [SerializeField]
    private float velocidadeMaximaCamada1 = 0.4f;

    [SerializeField]
    private float tamanhoMinimoCamada1 = 0.02f;

    [SerializeField]
    private float tamanhoMaximoCamada1 = 0.04f;


    // ============================================================
    // CAMADA 2 - ESTRELAS MÉDIAS
    // ============================================================

    [Header("CAMADA 2 - ESTRELAS MÉDIAS")]

    [SerializeField]
    private int quantidadeCamada2 = 40;

    [SerializeField]
    private float velocidadeMinimaCamada2 = 0.5f;

    [SerializeField]
    private float velocidadeMaximaCamada2 = 0.9f;

    [SerializeField]
    private float tamanhoMinimoCamada2 = 0.04f;

    [SerializeField]
    private float tamanhoMaximoCamada2 = 0.07f;


    // ============================================================
    // CAMADA 3 - ESTRELAS GRANDES
    // ============================================================

    [Header("CAMADA 3 - ESTRELAS GRANDES")]

    [SerializeField]
    private int quantidadeCamada3 = 15;

    [SerializeField]
    private float velocidadeMinimaCamada3 = 1f;

    [SerializeField]
    private float velocidadeMaximaCamada3 = 1.6f;

    [SerializeField]
    private float tamanhoMinimoCamada3 = 0.07f;

    [SerializeField]
    private float tamanhoMaximoCamada3 = 0.12f;


    // ============================================================
    // ÁREA
    // ============================================================

    [Header("ÁREA DAS ESTRELAS")]

    [SerializeField]
    private float margemHorizontal = 2f;

    [SerializeField]
    private float margemVertical = 1f;


    // ============================================================
    // CÂMERA
    // ============================================================

    private Camera cameraPrincipal;


    // ============================================================
    // START
    // ============================================================

    private void Start()
    {
        // Procura a câmera principal.
        cameraPrincipal = Camera.main;

        if (cameraPrincipal == null)
        {
            Debug.LogError(
                "FundoEstrelado: nenhuma câmera com a Tag MainCamera foi encontrada."
            );

            return;
        }

        // Cria as três camadas.
        CriarCamada(
            quantidadeCamada1,
            velocidadeMinimaCamada1,
            velocidadeMaximaCamada1,
            tamanhoMinimoCamada1,
            tamanhoMaximoCamada1,
            1
        );

        CriarCamada(
            quantidadeCamada2,
            velocidadeMinimaCamada2,
            velocidadeMaximaCamada2,
            tamanhoMinimoCamada2,
            tamanhoMaximoCamada2,
            2
        );

        CriarCamada(
            quantidadeCamada3,
            velocidadeMinimaCamada3,
            velocidadeMaximaCamada3,
            tamanhoMinimoCamada3,
            tamanhoMaximoCamada3,
            3
        );
    }


    // ============================================================
    // CRIAR CAMADA
    // ============================================================

    private void CriarCamada(
        int quantidade,
        float velocidadeMinima,
        float velocidadeMaxima,
        float tamanhoMinimo,
        float tamanhoMaximo,
        int camada
    )
    {
        for (int i = 0; i < quantidade; i++)
        {
            // Cria a estrela.
            GameObject estrela =
                CriarEstrela(camada);

            // Coloca em uma posição aleatória.
            estrela.transform.position =
                PosicaoAleatoria();

            // Define tamanho aleatório.
            float tamanho =
                Random.Range(
                    tamanhoMinimo,
                    tamanhoMaximo
                );

            estrela.transform.localScale =
                Vector3.one * tamanho;

            // Procura o script de movimento.
            MovimentoEstrela movimento =
                estrela.GetComponent<MovimentoEstrela>();

            // Define velocidade aleatória.
            movimento.DefinirVelocidade(
                Random.Range(
                    velocidadeMinima,
                    velocidadeMaxima
                )
            );
        }
    }


    // ============================================================
    // CRIAR ESTRELA
    // ============================================================

    private GameObject CriarEstrela(int camada)
    {
        // Cria o objeto.
        GameObject estrela =
            new GameObject(
                "Estrela_Camada_" + camada
            );

        // Coloca como filha do FundoEstrelado.
        estrela.transform.SetParent(
            transform
        );

        // Adiciona Sprite Renderer.
        SpriteRenderer spriteRenderer =
            estrela.AddComponent<SpriteRenderer>();


        // ========================================================
        // CRIA UMA TEXTURA DE 1 PIXEL
        // ========================================================

        Texture2D textura =
            new Texture2D(1, 1);

        textura.SetPixel(
            0,
            0,
            Color.white
        );

        textura.Apply();


        // ========================================================
        // CRIA O SPRITE
        // ========================================================

        Sprite sprite =
            Sprite.Create(
                textura,
                new Rect(
                    0,
                    0,
                    1,
                    1
                ),
                new Vector2(
                    0.5f,
                    0.5f
                ),
                1f
            );

        spriteRenderer.sprite =
            sprite;


        // ========================================================
        // VISUAL DAS CAMADAS
        // ========================================================

        if (camada == 1)
        {
            // Estrelas pequenas e distantes.
            spriteRenderer.color =
                new Color(
                    0.7f,
                    0.8f,
                    1f,
                    0.5f
                );

            // Fica na frente do cenário.
            spriteRenderer.sortingOrder = 1;
        }
        else if (camada == 2)
        {
            // Estrelas médias.
            spriteRenderer.color =
                new Color(
                    0.85f,
                    0.9f,
                    1f,
                    0.75f
                );

            // Fica acima da primeira camada.
            spriteRenderer.sortingOrder = 2;
        }
        else
        {
            // Estrelas grandes e próximas.
            spriteRenderer.color =
                Color.white;

            // Fica acima das outras estrelas.
            spriteRenderer.sortingOrder = 3;
        }


        // ========================================================
        // MOVIMENTO
        // ========================================================

        estrela.AddComponent<MovimentoEstrela>();


        return estrela;
    }


    // ============================================================
    // POSIÇÃO ALEATÓRIA
    // ============================================================

    private Vector3 PosicaoAleatoria()
    {
        // Pega o canto inferior da câmera.
        Vector3 inferior =
            cameraPrincipal.ViewportToWorldPoint(
                new Vector3(
                    0f,
                    0f,
                    0f
                )
            );


        // Pega o canto superior da câmera.
        Vector3 superior =
            cameraPrincipal.ViewportToWorldPoint(
                new Vector3(
                    1f,
                    1f,
                    0f
                )
            );


        // Sorteia posição X.
        float x =
            Random.Range(
                inferior.x - margemHorizontal,
                superior.x + margemHorizontal
            );


        // Sorteia posição Y.
        float y =
            Random.Range(
                inferior.y - margemVertical,
                superior.y + margemVertical
            );


        // Z = 5 para manter o fundo atrás
        // dos objetos principais.
        return new Vector3(
            x,
            y,
            5f
        );
    }
}