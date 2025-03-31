using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private float savedMasterVolume;
    private float savedSoundFXVolume;
    private float savedMusicVolume;

    void Awake() {
        savedMasterVolume = PlayerPrefs.GetFloat("masterVolume");
        savedSoundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");
        savedMusicVolume = PlayerPrefs.GetFloat("musicVolume");

        SetMasterVolume(savedMasterVolume);
        SetSoundFXVolume(savedSoundFXVolume);
        SetMusicVolume(savedMusicVolume);
    }

    public void SetMasterVolume(float level) {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("masterVolume", level);
        PlayerPrefs.Save();
    }
    public void SetSoundFXVolume(float level) {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("soundFXVolume", level);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float level) {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("musicVolume", level);
        PlayerPrefs.Save();
    }
    public float GetSavedMasterVolume() {
        return savedMasterVolume;
    }
    public float GetSavedSoundFXVolume() {
        return savedSoundFXVolume;
    }
    public float GetSavedMusicVolume() {
        return savedMusicVolume;
    }
}
