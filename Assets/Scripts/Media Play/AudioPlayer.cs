using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        GameEvents.Instance.OnRequestPauseGalleryAudio += OnPause;
        //MainEventBus.Instance.Subscribe(MainGameEventType.pauseGalleryAudio, OnPause);
        //MainEventBus.Instance.Subscribe(MainGameEventType.resumGalleryAudio, OnResume);
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
            GameEvents.Instance.OnRequestPauseGalleryAudio -= OnPause;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnPlay()
    {
        audioSource?.Play();
    }
    
    public void OnPause(bool pause)
    {
        if (pause)
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnStop()
    {
        audioSource?.Stop();
    }

}
