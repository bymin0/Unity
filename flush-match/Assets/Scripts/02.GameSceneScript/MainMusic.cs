using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    MusicManager musicManager;

    public AudioClip BGM;
    public AudioClip ClickSFX;
    public AudioClip TileSFX;
    public AudioClip MatchSFX;
    public AudioClip ClearStageSFX;
    public AudioClip TiktokSFX;

    public void Initialized(MusicManager musicManager)
    {
        this.musicManager = musicManager;
        if (musicManager != null )
            musicManager.PlayBGM(BGM);
    }

    public void UpdateBgmVolume(float volume)
    {
        musicManager.SetBgmVolume(volume);
    }

    public void UpdateSfxVolume(float volume)
    {
        musicManager.SetSFXVolume(volume);
    }

    public float GetBGMVolume()
    { return (musicManager != null) ? musicManager.GetBGMVolume() : 0f; }

    public float GetSFXVolume()
    { return (musicManager != null) ? musicManager.GetSFXVolume() : 0f; }

    public void OnClickedTileSFX()
    { musicManager.PlaySFX(TileSFX); }

    public void MatchedSFX()
    {
        //if (MatchSFX!= null && musicManager.sfxSource != null)
            musicManager.PlaySFX(MatchSFX);
    }

    public void ClickEventSFX()
    { musicManager.PlaySFX(ClickSFX); }

    public void NoTimeTikTok()
    { musicManager.PlaySFX(TiktokSFX); }

    public void StageClearSFX()
    { musicManager.PlaySFX(ClearStageSFX); }
}
