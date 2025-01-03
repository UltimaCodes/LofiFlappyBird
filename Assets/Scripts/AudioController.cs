using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] songs;
    private AudioSource audioSource;
    private int currentSongIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (songs.Length > 0)
        {
            PlaySong(currentSongIndex);
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            currentSongIndex = (currentSongIndex + 1) % songs.Length;
            PlaySong(currentSongIndex);
        }
    }

    void PlaySong(int index)
    {
        audioSource.clip = songs[index];
        audioSource.Play();
    }
}
