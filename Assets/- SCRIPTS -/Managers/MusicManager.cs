using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public List<AudioClip> musicPlaylist;
    private int playlistIndex;
    private AudioSource musicSource;
    private bool playedIntro;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Load audioclips in advance so it doesn't lag when it switches
        foreach (AudioClip clip in musicPlaylist)
        {
            clip.LoadAudioData();
        }

        musicSource = GetComponent<AudioSource>();
        playlistIndex = 0;
    }

    void Start()
    {
        if (musicPlaylist.Count != 0)
        {
            musicSource.clip = musicPlaylist[playlistIndex];
            musicSource.Play();            
        }
    }

    void Update()
    {
        if (!musicSource.isPlaying && !playedIntro && musicPlaylist.Count != 0)
        {
            if (playlistIndex < musicPlaylist.Count - 1) playlistIndex++;

            musicSource.clip = musicPlaylist[playlistIndex];
            musicSource.Play();

            if (!playedIntro)
            {
                musicSource.loop = true;
                playedIntro = true;
            }
        }
    }

    public void stopClip()
    {   
        if(musicPlaylist.Count > 0)
        {
            musicPlaylist.Remove(musicPlaylist[playlistIndex]);
            musicSource.Stop();
        }
    }

    public void playClip(AudioClip clip)
    {
        musicPlaylist.Add(clip);
        playlistIndex = 0;
        musicSource.clip = musicPlaylist[playlistIndex];
        musicSource.Play();
    }

    public AudioClip getCurrentClip()
    {
        return musicPlaylist[playlistIndex];
    } 
}
