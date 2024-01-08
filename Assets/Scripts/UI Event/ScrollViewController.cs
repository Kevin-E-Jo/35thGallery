using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{
    public const int NUMBER_OF_AVATAR = 11;
    const int INITIAL_NUMBER = 6;
    float divisionRatio;

    //int currentIndex = INITIAL_NUMBER;
    //int previousValue = 0;

    GameObject currentGameObject;
    GameObject previousGameObject;
    
    private int CurrentTapNum = 0;

    CharctorMeshAndMaterialController charCtrl;

    GameManager gm;

    //InputField nickNameText;
    TMPro.TMP_InputField nickNameText;

    //public Image leftBlockImage;
    //public Image rightBlockImage;


    //public AvatarState state;

    [Serializable]
    public class AvatarSetInfo
    {
        public string hair;
        public string face;
        public string top;
        public string bottom;
        public string shoes;
    }
    
    private void Awake()
    {
        GameEvents.Instance.OnRequestAvatarModify += AdjustingModifyInfo;
    }

    private void OnDestroy()
    {
        if(GameEvents.Instance != null)
            GameEvents.Instance.OnRequestAvatarModify -= AdjustingModifyInfo;
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        //scrollRect.verticalScrollbar.value = 0.5f;

        //previousGameObject = null;
        //previousValue = INITIAL_NUMBER;
        divisionRatio = 1.0f / (NUMBER_OF_AVATAR - 1);
        
        //MainEventBus.Instance.Subscribe(this);
        

        
        transform.Find("Avatar View Set/BasicAvatar").gameObject.SetActive(true);
        charCtrl = transform.Find("Avatar View Set/BasicAvatar").GetComponent<CharctorMeshAndMaterialController>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //nickNameText = transform.Find("CharacterSelectPanel/NickName Field/Text").GetComponent<InputField>();
        charCtrl.CharacterSetting(gm.state);

        //OnClickEvent();
        //GetUserNickName(gm.state.nickName);
        SetCurrentGameObject(0);
        //EventArgs args = new MainEventArgs(MainGameEventType.setProfile, LocalRepository.Instance.userProfile);
        //MainEventBus.Instance.Publish(args);
        
        GameEvents.Instance.RequestLoadingSetActive(false);
#if !UNITY_EDITOR
        WebGLInput.captureAllKeyboardInput = false;
#endif
    }

    private void AdjustingModifyInfo(string msg)
    {
        AvatarSetInfo data = JsonUtility.FromJson<AvatarSetInfo>(msg);
        AvatarState tempState = gm.state;
        tempState.hairCode = data.hair;
        tempState.hairColorCode = HairColorCodeSetter(data.hair);
        tempState.faceCode = data.face;
        tempState.bottomCode = data.bottom;
        tempState.topCode = data.top;
        tempState.shoesCode = data.shoes;
        gm.state = tempState;
        
        charCtrl.CharacterSetting(gm.state);
    }
    

    /// <summary>
    /// 
    /// </summary>
    public void GetUserNickName(string nickName)
    {
        nickNameText.text = nickName;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnChangeNickNameLimitByte()
    {
        int bytecount = System.Text.Encoding.Default.GetByteCount(nickNameText.text);
        if (bytecount < 20) return;
        byte[] byteTEMP = System.Text.Encoding.Default.GetBytes(nickNameText.text);

        string text = System.Text.Encoding.Default.GetString(byteTEMP, 0, 20);
        nickNameText.text = text;
    }


    ///// <summary>
    ///// 
    ///// </summary>
    //public void OnNickNameSelect(bool isPlaceholderEnable)
    //{
    //    nickNameText.placeholder.enabled = isPlaceholderEnable;
    //}

    /// <summary>
    /// 
    /// </summary>
    public void OnClickEvent()
    {
        OnScrollViewEvent(() => {
            currentGameObject = EventSystem.current.currentSelectedGameObject;
        });

        gm.state = charCtrl.state;
    }



    ///// <summary>
    ///// 
    ///// </summary>
    //public void OnLeftBtn()
    //{
    //    OnScrollViewEvent(() => {
    //        if (1 < currentIndex)
    //        {
    //            currentIndex--;
    //        }
    //    });
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    //public void OnRightBtn()
    //{
    //    OnScrollViewEvent(() => {
    //        if (currentIndex < NUMBER_OF_AVATAR)
    //        {
    //            currentIndex++;
    //        }
    //    });
    //}

    /// <summary>
    /// 
    /// </summary>
    void SetPreviousIndex()
    {
        previousGameObject = currentGameObject;
    }
    void SetCurrentGameObject(int parts)
    {
        string code = "";
        switch (parts)
        {
            case 0:
                code = charCtrl.state.hairCode;
                break;
            case 1:
                code = charCtrl.state.faceCode;
                break;
            case 2:
                code = charCtrl.state.topCode;
                break;
            case 3:
                code = charCtrl.state.bottomCode;
                break;
            case 4:
                code = charCtrl.state.shoesCode;
                break;
        }
        OnScrollViewEvent(() => {
            currentGameObject = GameObject.Find("Scroll View/Viewport/Content/" + code);
        });
    }

    /// <summary>
    /// 
    /// </summary>
    void OnScrollViewEvent(Action action, bool update = true)
    {
        SetPreviousIndex();
        action?.Invoke();
        FocusOnContent(update);
    }

    /// <summary>
    /// 
    /// </summary>
    //float currentValue = 0.0f;
    //float targetValue = 0.0f;
    //bool bLerp = false;
    void FocusOnContent(bool update)
    {
        //t = 0.0f;
        if(currentGameObject!= null)
            OnSelectAvatarParts(currentGameObject.name);

        //bLerp = update;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //string GetAvatarName(int index)
    //{
    //    NameContainerOfAvatars temp = new NameContainerOfAvatars();
    //    return temp.GetName(index);
    //}

    /// <summary>
    /// 
    /// </summary>
    void OnSelectAvatarParts(string targetName)
    {
        string colorCode = HairColorCodeSetter(targetName);

        charCtrl.CharactorPartsChange(targetName, colorCode);
    }

    private string HairColorCodeSetter(string targetName)
    {
        string[] code = targetName.Split('_');
        string colorCode = "#000000";
        if (code[0].ToUpper() == "HAIR")
        {
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
            switch (code[1])
            {
                case "AA":
                    switch (code[2])
                    {
                        case "001":
                            colorCode = ColorCode[25];
                            break;
                        case "002":
                            colorCode = ColorCode[29];
                            break;
                        case "003":
                            colorCode = ColorCode[24];
                            break;
                        case "004":
                            colorCode = ColorCode[27];
                            break;
                    }
                    break;
                case "AB":
                    colorCode = ColorCode[24];
                    break;
                case "AC":
                    colorCode = ColorCode[25];
                    break;
                case "AD":
                    colorCode = ColorCode[26];
                    break;
                case "AE":
                    colorCode = ColorCode[27];
                    break;
                case "AF":
                    colorCode = ColorCode[28];
                    break;
                case "AG":
                    colorCode = ColorCode[29];
                    break;
            }
        }

        return colorCode;
    }

    /// <summary>
    /// 
    /// </summary>
    //float t = 0.0f;
    //void SetScrollbarValue()
    //{
    //    if (bLerp)
    //    {
    //        t += Time.deltaTime;

    //        currentValue = scrollRect.verticalScrollbar.value;
    //        //scrollRect.verticalScrollbar.value = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 5.0f);

    //        if (1.0f < t)
    //        {
    //            bLerp = false;
    //            targetValue = 0.0f;
    //            currentValue = 0.0f;
    //        }
    //    }
    //}


    /// <summary>
    /// 
    /// </summary>
    void OnOffBlockImage()
    {
        //if (scrollRect.verticalScrollbar.value < 0.05f)
        //{
        //    SetActive(leftBlockImage, true);
        //}
        //else if (leftBlockImage.enabled)
        //{
        //    SetActive(leftBlockImage, false);
        //}

        //if (0.95f < scrollRect.verticalScrollbar.value)
        //{
        //    SetActive(rightBlockImage, true);
        //}
        //else if (rightBlockImage.enabled)
        //{
        //    SetActive(rightBlockImage, false);
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    void SetActive(Image target, bool enabled)
    {
        target.enabled = enabled;
    }


    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        //SetScrollbarValue();
    }
}
