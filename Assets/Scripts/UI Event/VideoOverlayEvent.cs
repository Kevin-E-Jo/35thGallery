using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoOverlayEvent : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    // Start is called before the first frame update
    [SerializeField] private Button playBtn;
    [SerializeField] private Button muteBtn;
    [SerializeField] private Sprite muteSprt;
    [SerializeField] private Sprite unmuteSprt;
    [SerializeField] private Sprite playSprt;
    [SerializeField] private Sprite pauseSprt;
    [SerializeField] private VideoPlayer videoPlayer;

    private void Awake()
    {
        playBtn.onClick.AddListener(delegate
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                playBtn.GetComponent<Image>().sprite = pauseSprt;
            }
            else
            {
                videoPlayer.Play();
                playBtn.GetComponent<Image>().sprite = playSprt;
            }
        });
        
        muteBtn.onClick.AddListener(delegate
        {
            if (videoPlayer.GetDirectAudioMute(0))
            {
                videoPlayer.SetDirectAudioMute(0,false);
                muteBtn.GetComponent<Image>().sprite = unmuteSprt;
            }
            else
            {
                videoPlayer.SetDirectAudioMute(0,true);
                muteBtn.GetComponent<Image>().sprite = muteSprt;
            }
        });
        
        
        playBtn.gameObject.SetActive(false);
        muteBtn.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("마우스 온");
        playBtn.gameObject.SetActive(true);
        muteBtn.gameObject.SetActive(true);

        if (videoPlayer.GetDirectAudioMute(0))
            muteBtn.GetComponent<Image>().sprite = muteSprt;
        else
            muteBtn.GetComponent<Image>().sprite = unmuteSprt;
        
        if (videoPlayer.isPlaying)
            playBtn.GetComponent<Image>().sprite = playSprt;
        else
            playBtn.GetComponent<Image>().sprite = pauseSprt;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        playBtn.gameObject.SetActive(false);
        muteBtn.gameObject.SetActive(false);
    }
}
