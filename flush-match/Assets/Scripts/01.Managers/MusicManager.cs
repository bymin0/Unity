using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Audio Source")]
    public AudioSource backgroundMusic;
    public AudioSource sfxSource;

    [Header("Settings")]
    private float bgmVolume = 0.5f;
    private float sfxVolume = 0.7f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
        backgroundMusic.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
        sfxSource.volume = bgmVolume;
    }

    public float GetBGMVolume() => bgmVolume;
    public float GetSFXVolume() => sfxVolume;

    public void PlayBGM(AudioClip bgmClip)
    {
        if (backgroundMusic.clip == null || backgroundMusic.clip != bgmClip)
        {
            backgroundMusic.clip = bgmClip;
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
