using UnityEngine;

/// <summary>
/// Controla o movimento de cada estrela.
///
/// As estrelas se movimentam da direita
/// para a esquerda, simulando o movimento
/// da nave pelo espaço.
/// </summary>
public class MovimentoEstrela : MonoBehaviour
{
    // Velocidade da estrela.
    private float velocidade;


    // ============================================================
    // CONFIGURAR VELOCIDADE
    // ============================================================

    public void DefinirVelocidade(float novaVelocidade)
    {
        velocidade = novaVelocidade;
    }


    // ============================================================
    // UPDATE
    // ============================================================

    private void Update()
    {
        // Move a estrela para a esquerda.
        transform.Translate(
            Vector3.left *
            velocidade *
            Time.deltaTime,
            Space.World
        );


        // Pega a câmera.
        Camera cameraPrincipal =
            Camera.main;

        if (cameraPrincipal == null)
        {
            return;
        }


        // Posição atual.
        Vector3 posicao =
            transform.position;


        // Limite esquerdo da câmera.
        Vector3 limiteEsquerdo =
            cameraPrincipal.ViewportToWorldPoint(
                new Vector3(
                    0,
                    0.5f,
                    0
                )
            );


        // ========================================================
        // RECICLAR ESTRELA
        // ========================================================

        // Se saiu pela esquerda...
        if (posicao.x < limiteEsquerdo.x - 2f)
        {
            // Pega o lado direito da câmera.
            Vector3 ladoDireito =
                cameraPrincipal.ViewportToWorldPoint(
                    new Vector3(
                        1,
                        Random.Range(
                            0f,
                            1f
                        ),
                        0
                    )
                );

            // Coloca novamente à direita.
            posicao.x =
                ladoDireito.x + 2f;

            // Sorteia uma nova altura.
            posicao.y =
                ladoDireito.y;

            // Aplica a posição.
            transform.position =
                posicao;
        }
    }
}