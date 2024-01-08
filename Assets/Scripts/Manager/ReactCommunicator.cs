using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

public class ReactCommunicator : Singleton<ReactCommunicator>
{
    [DllImport("__Internal")]
    private static extern void setHeaderLayout(string type);
    
    [DllImport("__Internal")]
    private static extern void okayToLeave();
    
    [DllImport("__Internal")]
    private static extern void openBoardModal();
    
    
    [DllImport("__Internal")]
    private static extern void openLoadingModal();
    
    
    [DllImport("__Internal")]
    private static extern void closeLoadingModal();
    
    [DllImport("__Internal")]
    private static extern void openNickAlert();
    
    [DllImport("__Internal")]
    private static extern void pingAck();
    
    [DllImport("__Internal")]
    private static extern void loadScene(string percent);
    
    [DllImport("__Internal")]
    private static extern void onDisconnectServer();


    [Serializable]
    public class ProfileClass
    {
        public string nickName;
    }
    
    [Serializable]
    public class HeaderType
    {
        public string type;
    }
    
    [Serializable]
    public class LoadPercent
    {
        public string percent;
    }

    public string CurrentLocation;
    
    private void Awake()
    {
        GameEvents.Instance.OnRequestLoadingSetActive += LoadingSetActive;
        DontDestroyOnLoad(this.gameObject);
    }

    private void LoadingSetActive(bool active)
    {
        Debug.Log("RequestLoadingSetActive called" + active);
#if UNITY_WEBGL && !UNITY_EDITOR
        if (active)
        {
            Debug.Log("Loading opne");
            openLoadingModal();
        }
        else
        {
            Debug.Log("Loading close");
            closeLoadingModal();
        }
#endif
    }
    
    //닉네임 정보 웹에 전달
    public void sendProfile(string msg)
    {
        ProfileClass data = JsonUtility.FromJson<ProfileClass>(msg);
        Debug.Log("Got Enter sign with nick name : " + data.nickName);
        //GameObject.Find("GameManager").GetComponent<GameManager>().state.nickName = data.nickName;
        Starter_3DGallery.Instance.SetWritedNickName(data.nickName);
        Starter_3DGallery.Instance.OnEnterAnExhibition();
        
        SendHeaderLayoutTypetoJS("LOBBY");
        GameEvents.Instance.RequestLobbyVideoStart();
#if !UNITY_EDITOR
        WebGLInput.captureAllKeyboardInput = true;
#endif
    }
    
    public void SetAvatar(string msg)
    {
        Debug.Log("Got modifying parts sign : " + msg);
        GameEvents.Instance.RequestAvatarModify(msg);
    }
    

    public void SendHeaderLayoutTypetoJS(string type)
    {
        Debug.Log("send ui type : " +  type);
#if UNITY_WEBGL && !UNITY_EDITOR
        HeaderType data = new HeaderType();
        data.type = type;
        string msg = JsonUtility.ToJson(data);
        setHeaderLayout(msg);
#endif
    }
    
    public void SendBoardOpenToJS()
    {
        openBoardModal();
        WebGLInput.captureAllKeyboardInput = false;
    }

    public void offKeyFocus()
    {
        Debug.Log("키 포커스 오프 신호 수신");
        //WebGLInput.captureAllKeyboardInput = false;
        GameEvents.Instance.RequestSetActivePlayerInputSys(false);
    }

    public void onKeyFocus()
    {
        Debug.Log("키 포커스 온 신호 수신");
        //WebGLInput.captureAllKeyboardInput = true;
        GameEvents.Instance.RequestSetActivePlayerInputSys(true);
    }
    
    public void sendHello()
    {
        Debug.Log("안녕 동작 신호 수신");
        GameEvents.Instance.RequestPlayerAction("Hi");
    }
    
    public void sendRunOn()
    {
        Debug.Log("달리기 신호 수신");
        GameEvents.Instance.RequestSetActiveSprint(true);
    }
    
    public void sendRunOff()
    {
        Debug.Log("걷기 신호 수신");
        GameEvents.Instance.RequestSetActiveSprint(false);
    }
    
    public void goFuture()
    {
        Debug.Log("미래관 이동 수신");
        LoadingSetActive(true);
        CurrentLocation = "FUTURE";
        SendHeaderLayoutTypetoJS(CurrentLocation);
        GameEvents.Instance.RequestTeleport(new Vector3(341.3403f, 10f, -449.0208f),new Vector3(0f, 120f, 0f));
        GameEvents.Instance.RequestPauseGalleryVideo(true);
    }
    
    public void goPast()
    {
        Debug.Log("과거관 이동 수신");
        LoadingSetActive(true);
        CurrentLocation = "PAST";
        SendHeaderLayoutTypetoJS(CurrentLocation);
        GameEvents.Instance.RequestTeleport(new Vector3(-566.69f, 10f, -533.4307f),new Vector3(0f, 0f, 0f));
        GameEvents.Instance.RequestPauseGalleryVideo(true);
    }
    
    public void goLobby()
    {
        Debug.Log("로비 이동 수신");
        LoadingSetActive(true);
        CurrentLocation = "LOBBY";
        SendHeaderLayoutTypetoJS(CurrentLocation);
        GameEvents.Instance.RequestTeleport(new Vector3(26.3166f, 10f, 9.85672f),new Vector3(0f, 180f, 0f));
    }
    
    public void exitGallery()
    {
        Debug.Log("나가기 수신");
#if UNITY_WEBGL && !UNITY_EDITOR
        WhenUserLeavesRoom();
#endif
    }
    
    public void ping()
    {
        Debug.Log("핑 수신");
#if UNITY_WEBGL && !UNITY_EDITOR
        pingAck();
#endif
    }
    
    public void closeBoardModal()
    {
        Debug.Log("닫힘 신호 수신!!");
        WebGLInput.captureAllKeyboardInput = true;
    }
    
    public void WhenUserLeavesRoom()
    {
        Debug.Log("나가기 완료 송신");
        okayToLeave();
        Application.Quit();
    }

    public void SendOpenNickAlert()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        openNickAlert();
#endif
    }
    
    public void SendLoadScenePercent(float percent)
    {
        LoadPercent data = new LoadPercent();
        data.percent = percent.ToString();
        string msg = JsonUtility.ToJson(data);
        Debug.Log("scene loading : " +(percent *100).ToString() +"%");
#if UNITY_WEBGL && !UNITY_EDITOR
        loadScene(msg);
#endif
    }
    
    public void SendDisconnectSign()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        onDisconnectServer();
#endif
    }

    
    
#if UNITY_EDITOR
    ProfileClass test = null;
    
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A))
        {
            if (test == null)
            {
                test = new ProfileClass();
                test.nickName = "유the점심꿀잠정균";
                string msg = JsonUtility.ToJson(test);
                sendProfile(msg);
            }
        }

        if (Input.GetKey(KeyCode.Z))
        {
            GameEvents.Instance.RequestSetActivePlayerInputSys(false);
        }
        if (Input.GetKey(KeyCode.X))
        {
            GameEvents.Instance.RequestSetActivePlayerInputSys(true);
        }
#endif
        
    }
    
#endif
}
