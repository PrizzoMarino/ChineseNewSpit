using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    private AudioSource audioSource;

    private int lastTrackIndex = -1;
    private bool musicStopped = false;

    [Header("Dynamic Pitch Settings")]
    public float minPitch = 1f;
    public float maxPitch = 2f;
    public float pitchRampDuration = 120f;

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
        if (!audioSource.isPlaying && !musicStopped)
        {
            PlayRandomTrack();
        }

        if (!musicStopped)
        {
            UpdateMusicPitch();
        }
    }

    void PlayRandomTrack()
    {
        if (tracks.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, tracks.Length);
        }
        while (newIndex == lastTrackIndex && tracks.Length > 1);

        lastTrackIndex = newIndex;

        audioSource.clip = tracks[newIndex];
        audioSource.Play();
    }

    void UpdateMusicPitch()
    {
        float t = Mathf.Clamp01(Time.timeSinceLevelLoad / pitchRampDuration);
        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
    }

    public void StopMusic()
    {
        musicStopped = true;
        audioSource.Stop();
    }
}
