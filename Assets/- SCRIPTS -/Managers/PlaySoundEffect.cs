using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool dontDestroyOnLoad;

    public void playSoundEffect()
    {
        SoundFXManager.instance.PlaySoundFXClip(audioClip, transform, 1f, dontDestroyOnLoad);
    }

}
