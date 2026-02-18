using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    private AudioSource audioSource;

    private int lastTrackIndex = -1;
    private bool musicStopped = false;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayRandomTrack();
    }

    void Update()
    {
        // When the current track finishes, play another randomly
        if (!audioSource.isPlaying && !musicStopped)
        {
            PlayRandomTrack();
        }

    }

    void PlayRandomTrack()
    {
        if (tracks.Length == 0) return;

        int newIndex;

        // Keep picking until it's different from last to avoid repetition
        do
        {
            newIndex = Random.Range(0, tracks.Length);
        }
        while (newIndex == lastTrackIndex && tracks.Length > 1);

        lastTrackIndex = newIndex;

        audioSource.clip = tracks[newIndex];
        audioSource.Play();
    }

    public void StopMusic()
    {
        musicStopped = true;
        audioSource.Stop();
    }


}
