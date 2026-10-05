using UnityEngine;

/// <summary>
/// Mantém o jogo sempre na proporção Full HD 16:9.
///
/// A área original do jogo é considerada 1920 x 1080.
///
/// Se o monitor tiver outra proporção, o jogo mantém
/// a imagem sem deformar e cria áreas pretas
/// nas partes que sobrarem.
/// </summary>
[RequireComponent(typeof(Camera))]
public class ProporcaoTela : MonoBehaviour
{
    // ============================================================
    // CONFIGURAÇÃO
    // ============================================================

    [Header("PROPORÇÃO DO JOGO")]

    [Tooltip("Largura usada como referência.")]
    [SerializeField]
    private float larguraReferencia = 1920f;

    [Tooltip("Altura usada como referência.")]
    [SerializeField]
    private float alturaReferencia = 1080f;


    // ============================================================
    // CÂMERA
    // ============================================================

    private Camera cameraPrincipal;


    // ============================================================
    // AWAKE
    // ============================================================

    private void Awake()
    {
        // Pega a câmera.
        cameraPrincipal =
            GetComponent<Camera>();

        // Garante que a câmera seja Orthographic.
        cameraPrincipal.orthographic = true;

        // Calcula a proporção.
        AjustarProporcao();
    }


    // ============================================================
    // START
    // ============================================================

    private void Start()
    {
        // Ajusta novamente quando o jogo começa.
        AjustarProporcao();
    }


    // ============================================================
    // UPDATE
    // ============================================================

    private void Update()
    {
        // Verifica se a resolução mudou.
        AjustarProporcao();
    }


    // ============================================================
    // AJUSTAR PROPORÇÃO
    // ============================================================

    private void AjustarProporcao()
    {
        if (cameraPrincipal == null)
        {
            return;
        }

        // Proporção desejada:
        //
        // 1920 / 1080 = 1.777...
        //
        float proporcaoDesejada =
            larguraReferencia /
            alturaReferencia;


        // Proporção atual da tela.
        float proporcaoAtual =
            (float)Screen.width /
            Screen.height;


        // Calcula a diferença.
        float proporcao =
            proporcaoAtual /
            proporcaoDesejada;


        // ========================================================
        // TELA MAIS LARGA QUE 16:9
        // ========================================================

        if (proporcao > 1f)
        {
            // Mantém a área vertical.
            cameraPrincipal.rect =
                new Rect(
                    (1f - 1f / proporcao) / 2f,
                    0f,
                    1f / proporcao,
                    1f
                );
        }


        // ========================================================
        // TELA MAIS ALTA/ESTREITA QUE 16:9
        // ========================================================

        else
        {
            // Mantém a área horizontal.
            cameraPrincipal.rect =
                new Rect(
                    0f,
                    (1f - proporcao) / 2f,
                    1f,
                    proporcao
                );
        }
    }
}