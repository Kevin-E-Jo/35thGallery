using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventViewController : MonoBehaviour
{
    
    [SerializeField] private GameObject eventObj;
    [SerializeField] private Image posterImg;
    [SerializeField] private Button closeBtn;
    [SerializeField] private RectTransform contents;
    private string curPoster;
    
    private void Awake()
    {
        closeBtn.onClick.AddListener(delegate 
        { 
            eventObj.SetActive(false);
            curPoster = "";
            ReactCommunicator.Instance.SendHeaderLayoutTypetoJS("LOBBY");
            GameEvents.Instance.RequestBlockRaycast(false);
            GameEvents.Instance.RequestSetActivePlayerInputSys(true);
            GameEvents.Instance.RequestPauseGalleryAudio(false);
        });
        
        GameEvents.Instance.OnRequestOpenEventInfo += SetEventObjectActive;
    }

    private void Start()
    {
        eventObj.SetActive(false);
        curPoster = "";
    }

    private void OnDestroy()
    {
        if(GameEvents.Instance != null)
            GameEvents.Instance.OnRequestOpenEventInfo -= SetEventObjectActive;
    }

    private void SetEventObjectActive(string name)
    {
        if (curPoster.Equals(name))
        {
            eventObj.SetActive(false);
            curPoster = "";
            ReactCommunicator.Instance.SendHeaderLayoutTypetoJS("LOBBY");
            GameEvents.Instance.RequestBlockRaycast(false);
            GameEvents.Instance.RequestSetActivePlayerInputSys(true);
            GameEvents.Instance.RequestPauseGalleryAudio(false);
        }
        else
        {
            curPoster = name;
            eventObj.SetActive(true);
            Texture2D texture = Resources.Load("Art/" + curPoster) as Texture2D;
            posterImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width , texture.height),
                new Vector2(0.5f, 0.5f));
            ReactCommunicator.Instance.SendHeaderLayoutTypetoJS("NONE");
            GameEvents.Instance.RequestBlockRaycast(true);
            GameEvents.Instance.RequestSetActivePlayerInputSys(false);
            GameEvents.Instance.RequestPauseGalleryAudio(true);
            contents.position = new Vector3(contents.position.x,0,contents.position.z);
        }
    }
}
