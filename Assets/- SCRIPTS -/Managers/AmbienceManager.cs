using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager instance;

    public List<AudioClip> ambiancePlaylist;
    private int playlistIndex;
    private AudioSource ambianceSource;
    private bool playedIntro;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Load audioclips in advance so it doesn't lag when it switches
        foreach (AudioClip clip in ambiancePlaylist)
        {
            clip.LoadAudioData();
        }

        ambianceSource = GetComponent<AudioSource>();
        playlistIndex = 0;
    }

    void Start()
    {
        if (ambiancePlaylist.Count != 0)
        {
            ambianceSource.clip = ambiancePlaylist[playlistIndex];
            ambianceSource.Play();            
        }
    }

    void Update()
    {
        if (!ambianceSource.isPlaying && !playedIntro && ambiancePlaylist.Count != 0)
        {
            if (playlistIndex < ambiancePlaylist.Count - 1) playlistIndex++;

            ambianceSource.clip = ambiancePlaylist[playlistIndex];
            ambianceSource.Play();

            if (!playedIntro)
            {
                ambianceSource.loop = true;
                playedIntro = true;
            }
        }
    }

    public void stopClip()
    {   
        if(ambiancePlaylist.Count > 0)
        {
            ambiancePlaylist.Remove(ambiancePlaylist[playlistIndex]);
            ambianceSource.Stop();
        }
    }
    
    public void playClip(AudioClip clip)
    {
        ambiancePlaylist.Add(clip);
        playlistIndex = 0;
        ambianceSource.clip = ambiancePlaylist[playlistIndex];
        ambianceSource.Play();
    }

}
