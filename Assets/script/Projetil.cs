using UnityEngine;

/// <summary>
/// Projétil usado tanto pelo jogador quanto pelos inimigos.
/// A diferença entre os dois fica na configuração do prefab:
///  - Tiro do jogador: direcao = (1, 0)  e tagAlvo = "Enemy"
///  - Tiro do inimigo: direcao = (-1, 0) e tagAlvo = "Player"
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projetil : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidade = 12f;
    [SerializeField] private Vector2 direcao = Vector2.right;

    [Header("Dano")]
    [SerializeField] private int dano = 1;
    [Tooltip("Tag do objeto que este tiro pode acertar (ex.: Enemy ou Player).")]
    [SerializeField] private string tagAlvo = "Enemy";

    [Header("Vida útil")]
    [Tooltip("Segundos até o tiro se destruir sozinho (segurança caso ele nunca saia da tela).")]
    [SerializeField] private float tempoDeVida = 5f;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Start()
    {
        direcao = direcao.normalized;
        Destroy(gameObject, tempoDeVida);
    }

    private void Update()
    {
        transform.Translate(direcao * velocidade * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (!outro.CompareTag(tagAlvo)) return;

        if (outro.TryGetComponent(out Vida vida))
        {
            vida.ReceberDano(dano);
        }

        // GerenciadorAudio.Instancia?.TocarSomImpacto();  // adicione quando criar o GerenciadorAudio
        Destroy(gameObject);
    }

    // Destrói o tiro assim que ele sai da tela (precisa de um SpriteRenderer no prefab)
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
