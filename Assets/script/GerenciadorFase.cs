using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla o fim da fase.
/// 
/// Se o jogador morrer:
/// -> abre GameOver
///
/// Se todos os inimigos morrerem:
/// -> abre TelaVitoria
/// </summary>
public class GerenciadorFase : MonoBehaviour
{
    [Header("CENAS")]

    // Nome da cena que será aberta quando o jogador morrer
    [SerializeField] private string cenaGameOver = "GameOver";

    // Nome da cena que será aberta quando todos os inimigos morrerem
    [SerializeField] private string cenaVitoria = "TelaVitoria";


    [Header("INIMIGOS")]

    // Quantidade de inimigos que ainda estão vivos
    private int inimigosRestantes;

    // Evita chamar duas vezes a troca de cena
    private bool faseTerminou = false;


    private void Start()
    {
        // Procura todos os objetos que possuem a Tag "Enemy"
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Enemy");

        // Guarda a quantidade encontrada
        inimigosRestantes = inimigos.Length;

        Debug.Log("Inimigos encontrados: " + inimigosRestantes);
    }


    /// <summary>
    /// Deve ser chamado quando o jogador morrer.
    /// </summary>
    public void JogadorMorreu()
    {
        // Se a fase já terminou, não faz nada
        if (faseTerminou)
            return;

        faseTerminou = true;

        Debug.Log("Jogador morreu! Abrindo GameOver.");

        // Abre a cena GameOver
        SceneManager.LoadScene(cenaGameOver);
    }


    /// <summary>
    /// Deve ser chamado sempre que um inimigo morrer.
    /// </summary>
    public void InimigoMorreu()
    {
        // Se a fase já terminou, não faz nada
        if (faseTerminou)
            return;

        // Diminui a quantidade de inimigos
        inimigosRestantes--;

        Debug.Log("Inimigo derrotado! Restam: " + inimigosRestantes);


        // Verifica se não existem mais inimigos
        if (inimigosRestantes <= 0)
        {
            faseTerminou = true;

            Debug.Log("Todos os inimigos foram derrotados!");

            // Abre a tela de vitória
            SceneManager.LoadScene(cenaVitoria);
        }
    }
}