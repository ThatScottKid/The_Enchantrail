using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource ClickSource, BackgroundSource;
    public AudioClip MainMenuAudio, BattleAudio, ShopAudio, DefeatAudio;

    public void ClickAudio()
    {
        ClickSource.Play();
    }

    private void ChangeAudioClip(AudioClip ac)
    {
        BackgroundSource.clip = ac;
        BackgroundSource.Play();
    }

    public void PlayMainMenu()
    {
        ChangeAudioClip(MainMenuAudio);
        BackgroundSource.volume = 1.1f;
    }

    public void PlayBattle()
    {
        ChangeAudioClip(BattleAudio);
        BackgroundSource.volume = 0.7f;
    }

    public void PlayShop()
    {
        ChangeAudioClip(ShopAudio);
        BackgroundSource.volume = 1;
    }

    public void PlayDefeat()
    {
        ChangeAudioClip(DefeatAudio);
        BackgroundSource.volume = 1;
    }
}
