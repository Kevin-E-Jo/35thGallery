using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter_3DGallery : Singleton<Starter_3DGallery>
{
    /// <summary>
    /// 
    /// </summary>
    protected void Awake()
    {
        //GameObject target = GameObject.FindGameObjectWithTag("ArtExhibitList");
        //Component[] components = target.GetComponentsInChildren<ArtExhibit>();

        //int index = 1;
        //foreach (ArtExhibit component in components)
        //{
        //    component.Initialize(index.ToString());
        //    index++;
        //}

        //transform.Find("WebRequest").GetComponent<WebRequest>().WebRequestPost();

        //채팅 제거
        //GameObject photon = GameObject.FindGameObjectWithTag("Photon Manager");
        //photon.GetComponent<ChatTest>().Initialize();
        GetComponent<GameManager>().SetRandomAvatarState();
        
        LocalRepository.Instance.Init();
    }

    #if UNITY_EDITOR
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject.Find("Scroll View Panel").SetActive(false);
            GetComponent<GameManager>().SetRandomAvatarState();
            GetComponent<GameManager>().Initialize("BasicAvatar");
        }
    }
    
#endif
    
    /// <summary>
    /// 
    /// </summary>
    string selectedName = string.Empty;
    public void OnSelectAvatar(string name)
    {
        selectedName = name;
    }


    public void SetWritedNickName(string nick)
    {
        GetComponent<GameManager>().state.nickName = nick;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnEnterAnExhibition()
    {
        if (GetComponent<GameManager>().CheckNickName()) return;

        GameObject scrollView = GameObject.Find("Scroll View Panel");
        if(scrollView != null)
            scrollView.SetActive(false);
        
        /*
        #if !UNITY_EDITOR&&UNITY_WEBGL
        ReactCommunicator.Instance.SendProfiledataToJS(GetComponent<GameManager>().state.nickName);
        #endif
        */
        
        //GetComponent<GameManager>().SetRandomAvatarState();
        GetComponent<GameManager>().Initialize("BasicAvatar");
    }
}
