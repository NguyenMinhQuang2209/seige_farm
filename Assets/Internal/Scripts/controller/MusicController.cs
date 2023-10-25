using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource walkMusic;
    public AudioSource runMusic;
    public AudioSource jumpMusic;
    public AudioSource swordAttackMusic;
    public AudioSource handAttackMusic;
    public AudioSource gunAttackMusic;
    public AudioSource collectMusic;
    public AudioSource hitMusic;
    public static MusicController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        walkMusic.volume = 1f;
        runMusic.volume = 1f;
        jumpMusic.volume = 1f;
        gunAttackMusic.volume = 1f;
        collectMusic.volume = 1f;
        hitMusic.volume = 1f;
        handAttackMusic.volume = 1f;
        swordAttackMusic.volume = 1f;
        backgroundMusic.volume = 1f;
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }
    public void ChangeSoundEffectVolume(float value)
    {
        walkMusic.volume = value;
        runMusic.volume = value;
        jumpMusic.volume = value;
        gunAttackMusic.volume = value;
        collectMusic.volume = value;
        hitMusic.volume = value;
        handAttackMusic.volume = value;
        swordAttackMusic.volume = value;
    }
}
