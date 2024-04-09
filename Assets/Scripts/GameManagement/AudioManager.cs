using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance

    public AudioSource bgMusicSource; // Audio source for background music
    public AudioSource sfxSource; // Audio source for sound effects

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist AudioManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManager
        }
    }

    // Play background music
    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        bgMusicSource.clip = musicClip;
        bgMusicSource.Play();
    }

    // Play a sound effect
    public void PlaySoundEffect(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
