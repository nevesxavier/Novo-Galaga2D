using UnityEngine;

/// <summary>
/// Projétil usado tanto pelo jogador quanto pelos inimigos.
///
/// Tiro do jogador:
/// direcao = (1, 0)
/// tagAlvo = "Enemy"
///
/// Tiro do inimigo:
/// direcao = (-1, 0)
/// tagAlvo = "Player"
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projetil : MonoBehaviour
{
    // ============================================================
    // MOVIMENTO
    // ============================================================

    [Header("MOVIMENTO")]

    [SerializeField]
    private float velocidade = 12f;

    [SerializeField]
    private Vector2 direcao = Vector2.right;


    // ============================================================
    // DANO
    // ============================================================

    [Header("DANO")]

    [SerializeField]
    private int dano = 1;

    [Tooltip("Tag do objeto que este tiro pode acertar.")]
    [SerializeField]
    private string tagAlvo = "Enemy";


    // ============================================================
    // VIDA ÚTIL
    // ============================================================

    [Header("VIDA ÚTIL")]

    [Tooltip("Segundos até o tiro se destruir.")]
    [SerializeField]
    private float tempoDeVida = 5f;


    // ============================================================
    // AWAKE
    // ============================================================

    private void Awake()
    {
        // Pega o Rigidbody2D.
        Rigidbody2D rb =
            GetComponent<Rigidbody2D>();

        // Tiro não precisa de física normal.
        rb.bodyType =
            RigidbodyType2D.Kinematic;

        // Remove a gravidade.
        rb.gravityScale = 0f;

        // Faz o Collider funcionar como Trigger.
        GetComponent<Collider2D>().isTrigger = true;
    }


    // ============================================================
    // START
    // ============================================================

    private void Start()
    {
        // Normaliza a direção.
        direcao = direcao.normalized;

        // Destrói o tiro depois do tempo configurado.
        Destroy(
            gameObject,
            tempoDeVida
        );
    }


    // ============================================================
    // UPDATE
    // ============================================================

    private void Update()
    {
        // Move o tiro.
        transform.Translate(
            direcao *
            velocidade *
            Time.deltaTime,
            Space.World
        );
    }


    // ============================================================
    // CONFIGURAR VELOCIDADE
    // ============================================================

    /// <summary>
    /// Permite que outro script altere
    /// a velocidade deste projétil.
    /// </summary>
    public void Configurar(float novaVelocidade)
    {
        // Garante que a velocidade não seja negativa.
        velocidade =
            Mathf.Max(0f, novaVelocidade);
    }


    // ============================================================
    // CONFIGURAR VIDA ÚTIL
    // ============================================================

    /// <summary>
    /// Permite configurar o tempo de vida
    /// deste projétil.
    /// </summary>
    public void ConfigurarTempoDeVida(
        float novoTempo
    )
    {
        // Garante que o tempo seja válido.
        tempoDeVida =
            Mathf.Max(0.1f, novoTempo);
    }


    // ============================================================
    // COLISÃO
    // ============================================================

    private void OnTriggerEnter2D(
        Collider2D outro
    )
    {
        // Se não for o alvo correto,
        // ignora a colisão.
        if (!outro.CompareTag(tagAlvo))
        {
            return;
        }


        // Procura o componente Vida.
        if (outro.TryGetComponent(
            out Vida vida
        ))
        {
            // Aplica o dano.
            vida.ReceberDano(dano);
        }


        // Destrói o tiro depois do impacto.
        Destroy(gameObject);
    }


    // ============================================================
    // SAIU DA TELA
    // ============================================================

    private void OnBecameInvisible()
    {
        // Destrói o tiro quando sair da tela.
        Destroy(gameObject);
    }
}