using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider backgroundMusicSlider;
    [SerializeField] private Slider soundEffectMusicSlider;
    AudioSource backgroundMusic;
    private void Start()
    {
        backgroundMusic = MusicController.instance.backgroundMusic;
        backgroundMusicSlider.minValue = 0f;
        backgroundMusicSlider.maxValue = 1f;
        soundEffectMusicSlider.minValue = 0f;
        soundEffectMusicSlider.maxValue = 1f;
        soundEffectMusicSlider.value = 1f;
        backgroundMusicSlider.value = 1f;
        MusicController.instance.ChangeSoundEffectVolume(1f);
    }
    private void Update()
    {
        MusicController.instance.ChangeSoundEffectVolume(soundEffectMusicSlider.value);
        backgroundMusic.volume = backgroundMusicSlider.value;
    }
}
