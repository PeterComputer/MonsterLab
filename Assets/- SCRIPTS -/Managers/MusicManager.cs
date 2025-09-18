using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioClip[] musicPlaylist;
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
        if (musicPlaylist.Length != 0)
        {
            musicSource.clip = musicPlaylist[playlistIndex];
            musicSource.Play();            
        }
    }

    void Update()
    {   
        // If there currently isn't a clip playing, the intro clip hasn't been played and the playlist isn't empty
        if (!musicSource.isPlaying && !playedIntro && musicPlaylist.Length != 0)
        {   
            // If there are still songs in the playlist, advance to the next one
            if (playlistIndex < musicPlaylist.Length - 1) playlistIndex++;

            // Play the next clip
            musicSource.clip = musicPlaylist[playlistIndex];
            musicSource.Play();

            // If the intro hasn't been played yet (aka it was just selected to play), set it to loop the next track
            if (!playedIntro)
            {
                musicSource.loop = true;
                playedIntro = true;
            }
        }
    }

}
