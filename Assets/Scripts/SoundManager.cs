using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource scoreSource;
    public AudioSource otherSource;
    public AudioSource musicSource;
    public AudioLowPassFilter musicFilter;
    public AudioClip[] Sounds;

    public Button mute;
    public Sprite muted;
    public Sprite notMuted;

    public float scoreVolume = .7f;
    public float otherVolume = .7f;


    public void ScoreSound(float increase)
    {
        scoreSource.volume = scoreVolume;
        scoreSource.pitch += increase;
        if(!scoreSource.isPlaying)
        {
            scoreSource.clip = Sounds[0];
            scoreSource.Play();
        }
        else
        {
            scoreSource.Stop();
            ScoreSound(0f);
        }
    }

    public void ExpandSound()
    {        
        scoreSource.volume = scoreVolume;
        if (!scoreSource.isPlaying)
        {
            scoreSource.clip = Sounds[1];
            scoreSource.Play();
        }
        else
        {
            scoreSource.Stop();
            ExpandSound();
        }
        scoreSource.pitch = 0.7f;
    }

    public void BuySound()
    {
        otherSource.pitch = 1f;
        otherSource.volume = otherVolume;
        if (!otherSource.isPlaying)
        {
            otherSource.clip = Sounds[2];
            otherSource.Play();
        }
        else
        {
            otherSource.Stop();
            BuySound();
        }
    }

    public void GameOverSound()
    {
        otherSource.pitch = 1f;
        otherSource.volume = otherVolume;
        if (!otherSource.isPlaying)
        {
            otherSource.clip = Sounds[3];
            otherSource.Play();
        }
        else
        {
            scoreSource.Stop();
            GameOverSound();
        }
    }

    public void CloseSound()
    {
        otherSource.pitch = 1f;
        otherSource.volume = otherVolume;
        if (!otherSource.isPlaying)
        {
            otherSource.clip = Sounds[5];
            otherSource.Play();
        }
        else
        {
            otherSource.Stop();
            CloseSound();
        }
    }

    public void OpenSound()
    {
        otherSource.volume = otherVolume;
        otherSource.pitch = 1.3f;
        if (!otherSource.isPlaying)
        {
            otherSource.clip = Sounds[4];
            otherSource.Play();
        }
        else
        {
            otherSource.Stop();
            OpenSound();
        }
    }

    public void EquipSound()
    {
        otherSource.volume = otherVolume;
        otherSource.pitch = 0.8f;
        if (!otherSource.isPlaying)
        {
            otherSource.clip = Sounds[6];
            otherSource.Play();
        }
        else
        {
            otherSource.Stop();
            EquipSound();
        }
    }

    public void DenySound()
    {
        otherSource.volume = otherVolume;
        otherSource.pitch = 1f;
        if (!otherSource.isPlaying)
        {
            otherSource.clip = Sounds[7];
            otherSource.Play();
        }
        else
        {
            otherSource.Stop();
            DenySound();
        }
    }

    public void ToggleMuteSound()
    {
        if(scoreVolume != 0)
        {
            scoreVolume = 0;
            otherVolume = 0;
            musicSource.volume = 0;
            mute.image.sprite = muted;
        }
        else
        {
            scoreVolume = 1;
            otherVolume = 1;
            musicSource.volume = 1;
            mute.image.sprite = notMuted;
        }        
    }

}
