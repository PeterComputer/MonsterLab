using UnityEngine;
using UnityEngine.UI;

public class getVolumeOnEnable : MonoBehaviour
{
    [SerializeField] private int sliderType;
    private SoundMixerManager soundMixerManager;
    private Slider volumeSlider;

    private void OnEnable() {
        switch (sliderType) {
            case 0:
                volumeSlider.value = soundMixerManager.GetSavedMasterVolume();
                break;
            case 1:
                volumeSlider.value = soundMixerManager.GetSavedSoundFXVolume();
                break;
            case 2:
                volumeSlider.value = soundMixerManager.GetSavedMusicVolume();
                break;
        }
    }



    void Awake() {
        volumeSlider = GetComponent<Slider>();
        soundMixerManager = GameObject.FindGameObjectWithTag("SoundMixerManager").GetComponent<SoundMixerManager>();
    }
}
