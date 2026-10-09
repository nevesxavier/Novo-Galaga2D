using UnityEngine;

public class GerenciadorDeAudio : MonoBehaviour
{
    public static GerenciadorDeAudio Instancia;

    [Header("Fontes de Áudio")]
    public AudioSource fonteMusica;
    public AudioSource fonteSFX;

    [Header("Músicas")]
    public AudioClip musicaTelaInicial;
    public AudioClip musicaFaseNormal;
    public AudioClip musicaBoss;
    public AudioClip musicaGameOver;
    public AudioClip musicaVitoria;

    [Header("Efeitos Sonoros (SFX)")]
    public AudioClip somCliqueBotao;
    public AudioClip somTiroJogador;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        // Configura a fonte de música.
        if (fonteMusica != null)
        {
            fonteMusica.loop = true;
            fonteMusica.playOnAwake = false;
        }
    }

    public void TocarMusica(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError(
                "GerenciadorDeAudio: a música solicitada não está configurada!"
            );
            return;
        }

        if (fonteMusica == null)
        {
            Debug.LogError(
                "GerenciadorDeAudio: a Fonte Musica não está configurada!"
            );
            return;
        }

        // Não reinicia a música se ela já estiver tocando.
        if (fonteMusica.clip == clip && fonteMusica.isPlaying)
        {
            return;
        }

        // Para a música anterior.
        fonteMusica.Stop();

        // Seleciona e inicia a nova música.
        fonteMusica.clip = clip;
        fonteMusica.loop = true;
        fonteMusica.time = 0f;
        fonteMusica.Play();

        Debug.Log("GerenciadorDeAudio: música iniciada: " + clip.name);
    }

    public void TocarMusicaTelaInicial()
    {
        TocarMusica(musicaTelaInicial);
    }

    public void TocarMusicaFase()
    {
        TocarMusica(musicaFaseNormal);
    }

    public void TocarMusicaBoss()
    {
        TocarMusica(musicaBoss);
    }

    public void TocarMusicaGameOver()
    {
        TocarMusica(musicaGameOver);
    }

    public void TocarMusicaVitoria()
    {
        TocarMusica(musicaVitoria);
    }

    public void TocarSFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning(
                "GerenciadorDeAudio: efeito sonoro não configurado!"
            );
            return;
        }

        if (fonteSFX == null)
        {
            Debug.LogWarning(
                "GerenciadorDeAudio: a Fonte SFX não está configurada!"
            );
            return;
        }

        fonteSFX.PlayOneShot(clip);
    }

    public void TocarSomClique()
    {
        TocarSFX(somCliqueBotao);
    }

    public void TocarSomTiro()
    {
        TocarSFX(somTiroJogador);
    }
}
