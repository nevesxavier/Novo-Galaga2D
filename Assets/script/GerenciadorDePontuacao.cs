
using UnityEngine;
using TMPro;

public class GerenciadorDePontuacao : MonoBehaviour
{
    public static GerenciadorDePontuacao Instancia;

    [Header("PONTUAÇÃO")]
    public int pontuacaoAtual = 0;
    public int recorde = 0;

    [Header("TEXTOS DA INTERFACE")]
    public TMP_Text textoPontuacao;
    public TMP_Text textoRecorde;

    private const string CHAVE_RECORDE = "RecordePontuacao";

    private void Awake()
    {
        // Impede a criação de gerenciadores duplicados.
        if (Instancia != null && Instancia != this)
        {
            // Se esta nova cena tiver textos próprios,
            // transfere as referências antes de destruir a cópia.
            Instancia.ConfigurarTextos(
                textoPontuacao,
                textoRecorde
            );

            Destroy(gameObject);
            return;
        }

        Instancia = this;

        // Mantém o gerenciador ao trocar de cena.
        DontDestroyOnLoad(gameObject);

        // Carrega o recorde salvo.
        recorde = PlayerPrefs.GetInt(CHAVE_RECORDE, 0);
    }

    private void Start()
    {
        AtualizarInterface();
    }

    public void AdicionarPontos(int pontos)
    {
        if (pontos <= 0)
            return;

        pontuacaoAtual += pontos;

        if (pontuacaoAtual > recorde)
        {
            recorde = pontuacaoAtual;

            PlayerPrefs.SetInt(CHAVE_RECORDE, recorde);
            PlayerPrefs.Save();
        }

        AtualizarInterface();
    }

    public void ZerarPontuacao()
    {
        pontuacaoAtual = 0;
        AtualizarInterface();
    }

    // Permite atualizar os textos da cena atual.
    public void ConfigurarTextos(
        TMP_Text novoTextoPontuacao,
        TMP_Text novoTextoRecorde)
    {
        if (novoTextoPontuacao != null)
            textoPontuacao = novoTextoPontuacao;

        if (novoTextoRecorde != null)
            textoRecorde = novoTextoRecorde;

        AtualizarInterface();
    }

    private void AtualizarInterface()
    {
        if (textoPontuacao != null)
        {
            textoPontuacao.text =
                "Pontos: " + pontuacaoAtual;
        }

        if (textoRecorde != null)
        {
            textoRecorde.text =
                "Recorde: " + recorde;
        }
    }

    private void OnDestroy()
    {
        if (Instancia == this)
            Instancia = null;
    }
}

