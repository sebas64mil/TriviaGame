using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource; // un audio source para efectos de sonido

    [Header("Clips")]
    public AudioClip clickClip;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    private void Awake()
    {
        // Patrón Singleton simple
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayClick()
    {
        PlaySound(clickClip);
    }

    public void PlayCorrect()
    {
        PlaySound(correctClip);
    }

    public void PlayWrong()
    {
        PlaySound(wrongClip);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }
}
