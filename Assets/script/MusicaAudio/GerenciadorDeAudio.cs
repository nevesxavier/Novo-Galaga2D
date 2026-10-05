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
        }
    }

    public void TocarMusica(AudioClip clip)
    {
        if (clip == null || fonteMusica == null) return;

        if (fonteMusica.clip == clip && fonteMusica.isPlaying) return;

        fonteMusica.clip = clip;
        fonteMusica.loop = true;
        fonteMusica.Play();
    }

    public void TocarMusicaTelaInicial() => TocarMusica(musicaTelaInicial);

    public void TocarMusicaFase() => TocarMusica(musicaFaseNormal);

    public void TocarMusicaBoss() => TocarMusica(musicaBoss);

    public void TocarMusicaGameOver() => TocarMusica(musicaGameOver);

    public void TocarMusicaVitoria() => TocarMusica(musicaVitoria);

    public void TocarSFX(AudioClip clip)
    {
        if (clip != null && fonteSFX != null)
        {
            fonteSFX.PlayOneShot(clip);
        }
    }

    public void TocarSomClique() => TocarSFX(somCliqueBotao);

    public void TocarSomTiro() => TocarSFX(somTiroJogador);
}