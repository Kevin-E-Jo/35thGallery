using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WebglVideoTest : MonoBehaviour
{
    public string target;
    VideoPlayer videoPlayer;
    private const string url = "https://torynft-gallery.s3.ap-northeast-2.amazonaws.com/";

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.url = url + target;
        gameObject.SetActive(false);
        
        

        //MainEventBus.Instance.Subscribe(MainGameEventType.pauseGalleryVideo, OnPause);
        //MainEventBus.Instance.Subscribe(MainGameEventType.resumeGalleryVideo, OnResume);
        GameEvents.Instance.OnRequestPauseGalleryVideo += OnPause;
    }

    public void OnDestroy()
    {
        //MainEventBus.Instance.UnSubscribe(MainGameEventType.pauseGalleryVideo, OnPause);
        //MainEventBus.Instance.UnSubscribe(MainGameEventType.resumeGalleryVideo, OnResume);
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnRequestPauseGalleryVideo -= OnPause;
        }
    }
    
    public void OnPlay()
    {
        videoPlayer.Play();
    }
    
    public void OnPause(bool pause)
    {
        if (pause)
        {
            if (videoPlayer.isPlaying)
                videoPlayer.Pause();
        }
        else
        {
            if (videoPlayer.isPaused)
                videoPlayer.Play();
        }
    }
    
    public void OnStop()
    {
        videoPlayer.Stop();
    }
}
