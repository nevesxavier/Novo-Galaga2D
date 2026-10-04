using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla o tiro da nave do jogador.
/// Atira um projétil em CADA ponto de tiro da lista (1 ponto = tiro simples, 2 pontos = tiro duplo).
/// Segurar o botão de tiro dispara continuamente, respeitando o intervalo (cooldown).
/// Usa o novo Input System (pacote com.unity.inputsystem).
/// </summary>
public class TiroJogador : MonoBehaviour
{
    [Header("Tiro")]
    [SerializeField] private GameObject projetilPrefab;
    [Tooltip("Pontos de onde os tiros saem. Coloque 2 objetos aqui para tiro duplo.")]
    [SerializeField] private Transform[] pontosDeTiro;
    [Tooltip("Tempo mínimo entre um tiro e outro, em segundos.")]
    [SerializeField] private float intervaloEntreTiros = 0.25f;

    private float proximoTiro;

    private void Update()
    {
        if (ApertouTiro() && Time.time >= proximoTiro)
        {
            Atirar();
        }
    }

    // Espaço, botão esquerdo do mouse ou botão sul do controle (A / X)
    private bool ApertouTiro()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado != null && teclado.spaceKey.isPressed) return true;

        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.isPressed) return true;

        Gamepad controle = Gamepad.current;
        if (controle != null && controle.buttonSouth.isPressed) return true;

        return false;
    }

    private void Atirar()
    {
        proximoTiro = Time.time + intervaloEntreTiros;

        if (pontosDeTiro == null || pontosDeTiro.Length == 0)
        {
            // Sem pontos configurados: atira do centro da nave
            Instantiate(projetilPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            foreach (Transform ponto in pontosDeTiro)
            {
                if (ponto == null) continue;
                Instantiate(projetilPrefab, ponto.position, Quaternion.identity);
            }
        }

        // GerenciadorAudio.Instancia?.TocarSomTiro();  // adicione quando criar o GerenciadorAudio
    }
}
