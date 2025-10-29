using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private int sliderType;
    public void SetVolume(float level)
    {
        switch (sliderType) {
            case 0:
                SoundMixerManager.instance.SetMasterVolume(level);
                break;
            case 1:
                SoundMixerManager.instance.SetSoundFXVolume(level);
                break;
            case 2:
                SoundMixerManager.instance.SetMusicVolume(level);
                break;
            case 3:
                SoundMixerManager.instance.SetAmbienceVolume(level);
                break;
        }
    }
}
