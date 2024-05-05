using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public static MusicController _MusicInstance;
    public AudioSource backgroundSound;

    public AudioClip[] musicTracks;  // array with different audioclips
    private List<int> trackIndices;
    private int currentTrackIndex = 0;

    public Image muteOrUnmute;
    public Sprite[] images;

    [HideInInspector] public bool mute;

    private void Awake() 
    {
        if(_MusicInstance != null && _MusicInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _MusicInstance = this;
        }
    }

    private void Start()
    {

        backgroundSound = GetComponent<AudioSource>();
        if(PlayerPrefs.HasKey("MusicSettings"))
        {
            if(PlayerPrefs.GetInt("MusicSettings") == 1) // if music was turn off
            {
                mute = false;
                ToggleSound();
            }
            if(PlayerPrefs.GetInt("MusicSettings") == 0) // if music was turn on
            {
                mute = true;
                ToggleSound();
            }
        }
        else
        {
            mute = false;
        }
 
        if(musicTracks.Length > 0)
        {
            GenerateTrackIndices();
            ShuffleTrackIndices();
            PlayNextTrack();
        }
        else
        {
            backgroundSound.Play();
        }
    }

    private void Update()
    {
        // If current track is end, start next track
        if (!backgroundSound.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void GenerateTrackIndices()
    {
        // create list of track indexes
        trackIndices = new List<int>();
        for (int i = 0; i < musicTracks.Length; i++)
        {
            trackIndices.Add(i);
        }
        ShuffleTrackIndices();
    }

    private void ShuffleTrackIndices()
    {
        // Fisher-Yates shuffle algotithm
        int i = trackIndices.Count;
        while (i > 1)
        {
            i--;
            int k = Random.Range(0, i + 1);
            int value = trackIndices[k];
            trackIndices[k] = trackIndices[i];
            trackIndices[i] = value;
        }
    }

    private void PlayNextTrack()
    {
        // choose next track from indexes list
        int nextIndex = trackIndices[currentTrackIndex];
        AudioClip nextTrack = musicTracks[nextIndex];

        backgroundSound.clip = nextTrack;
        backgroundSound.Play();

        // increase index current track
        currentTrackIndex = (currentTrackIndex + 1) % trackIndices.Count;

        // if all music were played
        if (currentTrackIndex == 0)
        {
            ShuffleTrackIndices();
        }
    }

    public void ToggleSound()
    {
        if(mute)
        {
            AudioListener.volume = 1f;
            muteOrUnmute.sprite = images[0];
            mute = false;
            PlayerPrefs.SetInt("MusicSettings", 0);
        }
        else
        {
            AudioListener.volume = 0f;
            muteOrUnmute.sprite = images[1];
            mute = true;
            PlayerPrefs.SetInt("MusicSettings", 1);
        }
    }
}
