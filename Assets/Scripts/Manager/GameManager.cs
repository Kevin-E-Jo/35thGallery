using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    
    [SerializeField]
    //private GameObject playerPrefab;
    static private GameObject player;
    ChatTest photonChat;

    public AvatarState state = new AvatarState();

    private GameObject skySphere;
    
    string[] ColorCode = {
        "#434343", // 0
        "#A43D3D",
        "#FFAE3D",
        "#0F6C3F",
        "#2B5B7B",
        "#D1A4E9",
        "#653625",
        "#B3CFCF", // 7

        "#FAE7D6", // 8
        "#F7DAC6",
        "#F5D5C2",
        "#F1D1B3",
        "#F0C8B4",
        "#EEC2AC",
        "#E7B49B",
        "#DBA58A",
        "#A17766",
        "#845C4E",
        "#614C39",
        "#443521",
        "#D13319",
        "#298A3A",
        "#4DB199",
        "#23759E", // 23
        "#4F2B18",
        "#463C33", // 25
        "#5E4834",
        "#81715E",
        "#675446",
        "#80593A", // 29
        
    };


    string[] MeshCode = {
        "AA", //0
        "AB",
        "AC",
        "AD",
        "AE",
        "AF", 
        "AG", //6
        "AH",
        "AI",
        "AJ" };

    private void Awake()
    {
        GameEvents.Instance.OnRequestTeleport += Teleport;
        GameEvents.Instance.OnRequestPlayerAction += OnPlayAnimation;
        GameEvents.Instance.OnRequestSetActivePlayerInputSys += OnPlayMoveEnable;
        GameEvents.Instance.OnRequestSetActiveSprint += ConvertSprint;
        
        skySphere = GameObject.Find("SkySphere");
        StartCoroutine(ChangeSkySphereCheckRealTime());        
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnRequestTeleport -= Teleport;
            GameEvents.Instance.OnRequestPlayerAction -= OnPlayAnimation;
            GameEvents.Instance.OnRequestSetActivePlayerInputSys -= OnPlayMoveEnable;
            GameEvents.Instance.OnRequestSetActiveSprint -= ConvertSprint;
        }
            
    }
    
    private void ConvertSprint(bool isSprint)
    {
        if (player != null)
            player.GetComponent<StarterAssetsInputs>().sprint = isSprint;
    }

    public void OnPlayAnimation(string animationName)
    {
        if (player != null)
            player.GetComponent<Animator>().SetTrigger(animationName);
    }
    
    public void OnPlayMoveEnable(bool Enable)
    {
        if (player != null)
        {
            player.GetComponent<StarterAssetsInputs>().MoveInput(Vector2.zero);
            player.GetComponent<StarterAssetsInputs>().enabled = Enable;
            player.GetComponent<PlayerInput>().enabled = Enable;
            
            #if !UNITY_EDITOR && UNITY_WEBGL
            WebGLInput.captureAllKeyboardInput = Enable;            
            Input.ResetInputAxes();
            #endif
        }
    }

    private void Update()
    {
        //if(player != null)
        //    Debug.Log("player input move : " + player.GetComponent<StarterAssetsInputs>().move);
    }

    public void SetNickName()
    {
        //state.nickName = GameObject.Find("CharacterSelectPanel/NickName Field/InputField (TMP)").GetComponent<TMPro.TMP_InputField>().text;
        PhotonNetwork.NickName = state.nickName;
    }

    //public void SetRandomizeNickName()
    //{
    //    state.nickName = NamePiece1List[Random.Range(0, NamePiece1List.Length)] + NamePiece2List[Random.Range(0, NamePiece2List.Length)];
    //}

    private void Teleport(Vector3 pos, Vector3 rot)
    {
        Debug.Log("teleport called");
        
            player.GetComponent<StarterAssetsInputs>().enabled = false;
            player.GetComponent<PlayerInput>().enabled = false;
            player.GetComponent<ThirdPersonController>().enabled = false;
            StartCoroutine(MakeSurePlayerMoved(pos, rot));
    }

    private IEnumerator MakeSurePlayerMoved(Vector3 pos, Vector3 rot)
    {
        bool flag = true;
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        
        
        StarterAssetsInputs assetsInputs =  player.GetComponent<StarterAssetsInputs>();
        Vector3 prevPos;
        while (flag)
        {
            prevPos = cam.transform.position;
            Debug.Log("loop called");
            player.transform.position = pos;
            player.transform.rotation = Quaternion.Euler(rot);
            yield return new WaitForSeconds(0.5f);
            
            if (math.abs(player.transform.position.x - pos.x) + math.abs(player.transform.position.z - pos.z) < 5
                && Vector3.Distance(prevPos, cam.transform.position) < 1f)
                flag = false;
        }
        
        player.GetComponent<StarterAssetsInputs>().enabled = true;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;
        
        yield return new WaitForSeconds(0.5f);
        float diff = GetCalculatableValue(cam.transform.localRotation.eulerAngles.y) - rot.y;
        Debug.Log("angle diff : " + diff.ToString());
        while (Mathf.Abs(diff) > 5f)
        {
            yield return new WaitForFixedUpdate();
            assetsInputs.LookInput(new Vector2(-diff/20f,0));
            diff = GetCalculatableValue(cam.transform.localRotation.eulerAngles.y) - rot.y;//GetCalculatableValue(player.transform.localRotation.eulerAngles.y);
            
            Debug.Log("angle diff : " + diff.ToString() + " vector force : " + (-diff/10f).ToString());
        }
        
        assetsInputs.LookInput(new Vector2(0,0));
        
        
        //GameEvents.Instance.RequestLoadingObjSetActive(false);
        
        GameEvents.Instance.RequestLoadingSetActive(false);
        if(ReactCommunicator.Instance.CurrentLocation == "LOBBY")
            GameEvents.Instance.RequestPauseGalleryVideo(false);
        yield return null;
    }
    
    private float GetCalculatableValue(float deg)
    {
        return (deg < 0) ? deg + 360 : deg;
    }

    public void SetRandomAvatarState()
    {
        /// ??? ??? ???? ???? ???
        //string faceMat = Random.Range(1, 14).ToString("D3");
        //string hairMesh = MeshCode[Random.Range(0, 7)];
        //string bottomMesh = MeshCode[Random.Range(0, 4)];
        //string bottomMat = bottomMesh == "AC" ? Random.Range(1, 7).ToString("D3") : Random.Range(1, 4).ToString("D3");
        //string shoesMesh = MeshCode[Random.Range(0, 3)];
        //string shoesMat = Random.Range(1, 5).ToString("D3");
        //string topMesh = MeshCode[Random.Range(0, 5)];
        //string topMat = (topMesh == "AA" || topMesh == "AB")
        //    ? Random.Range(1, 5).ToString("D3")
        //    : Random.Range(1, 8).ToString("D3");
        //string hairColor = ColorCode[Random.Range(0, 8)];
        //string skinColor = ColorCode[12];//Random.Range(8, 24)];

        ///// 35?? ? ?? ?? ?? ??
        //string hairMat = hairMesh != "AA" ? "001" : Random.Range(1, 4).ToString("D3");
        //switch (hairMesh)
        //{
        //    case "AA":
        //        switch (hairMat)
        //        {
        //            case "001":
        //                hairColor = ColorCode[25];
        //                break;
        //            case "002":
        //                hairColor = ColorCode[29];
        //                break;
        //            case "003":
        //                hairColor = ColorCode[24];
        //                break;
        //            case "004":
        //                hairColor = ColorCode[27];
        //                break;
        //        }
        //        break;
        //    case "AB":
        //        hairColor = ColorCode[24];
        //        break;
        //    case "AC":
        //        hairColor = ColorCode[25];
        //        break;
        //    case "AD":
        //        hairColor = ColorCode[26];
        //        break;
        //    case "AE":
        //        hairColor = ColorCode[27];
        //        break;
        //    case "AF":
        //        hairColor = ColorCode[28];
        //        break;
        //    case "AG":
        //        hairColor = ColorCode[29];
        //        break;
        //}
        /////

        //state.hairCode = "HAIR_" + hairMesh + "_" + hairMat;
        //state.hairColorCode = hairColor;
        //state.bottomCode = "BOTTOM_" + bottomMesh + "_" + bottomMat;
        //state.bottomColorCode = "#000000";
        //state.topCode = "TOP_" + topMesh + "_" + topMat;
        //state.topColorCode = "#000000";
        //state.shoesCode = "SHOES_" + shoesMesh + "_" + shoesMat;
        //state.shoesColorCode = "#000000";
        //state.faceCode = "FACE_AA_" + faceMat;
        //state.faceColorCode = "#000000";
        //state.skinCode = "SKIN_AA_001";
        //state.skinColorCode = skinColor;
        //state.nickName = "";
        ///

        state.hairCode = "HAIR_AD_001";
        state.hairColorCode = "#653625";
        state.bottomCode = "BOTTOM_AE_002";
        state.bottomColorCode = "#000000";
        state.topCode = "TOP_AG_002";
        state.topColorCode = "#000000";
        state.shoesCode = "SHOES_AD_002";
        state.shoesColorCode = "#000000";
        state.faceCode = "FACE_AA_001" ;
        state.faceColorCode = "#000000";
        state.skinCode = "SKIN_AA_001";
        state.skinColorCode = "#F0C8B4";
        state.nickName = "";

        //SetRandomizeNickName();
    }

    public bool CheckNickName()
    {
        SetNickName();
        Debug.Log("CheckNickName :#" + state.nickName + "#");
        if (string.IsNullOrEmpty(state.nickName))
        {
            Debug.Log("CheckNickName :#" + state.nickName + "#NullOrEmpty " + string.IsNullOrEmpty(state.nickName));
            //GameObject.Find("Scroll View Panel/CharacterSelectPanel/Alert").SetActive(true);
            ReactCommunicator.Instance.SendOpenNickAlert();
            return true;
        }
        else
        {
            Debug.Log("CheckNickName :#" + state.nickName + "#NotNullOrEmpty " + string.IsNullOrEmpty(state.nickName));
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Initialize(string avatarName)
    {
        string jsondata = JsonUtility.ToJson(state);

        player = PhotonNetwork.Instantiate("Avatars/" + avatarName, new Vector3(26.3166f, 10f, 9.85672f), Quaternion.identity, 0, new object[] { jsondata });
        player.tag = "Player";
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<CharctorMeshAndMaterialController>().CharacterSetting(state);

        GameObject.Find("CM vcam1").
            GetComponent<CinemachineVirtualCamera>().
            Follow = player.transform.Find("PlayerCameraRoot");
        GameObject.Find("CM vcam1").
            GetComponent<CinemachineVirtualCamera>().
            LookAt = player.transform.Find("PlayerCameraRoot");

        GameObject.Find("Main UI Canvas").
            GetComponent<UICanvasControllerInput>().
            starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();

    }

    /// <summary>
    /// Called when a Photon Player got disconnected. We need to load a smaller scene.
    /// </summary>
    /// <param name="other">Other.</param>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            //LoadArena();
        }
    }

    public IEnumerator ChangeSkySphereCheckRealTime()
    {
        bool isDay = true;
        while (true)
        {
            yield return new WaitForSeconds(1);
            int hour = int.Parse(DateTime.Now.ToString("HH"));
            if (hour <= 18 && hour >= 6)
            {
                Debug.Log("ChangeSkySphereCheckRealTime - Day : " + hour);
                isDay = true;
            }
            else
            {
                Debug.Log("ChangeSkySphereCheckRealTime Night : " + hour);
                isDay = false;
            }
            skySphere.transform.GetChild(0).gameObject.SetActive(isDay);
            skySphere.transform.GetChild(1).gameObject.SetActive(!isDay);
        }
        yield return null;              
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log( "OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting  
    }

    /// <summary>
    /// 
    /// </summary>
    //void LoadArena()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
    //    }

    //    Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
    //}

    public static void SetPlayerInputEnabled(bool state)
    {
        player.GetComponent<PlayerInput>().enabled = state;
    }
}
