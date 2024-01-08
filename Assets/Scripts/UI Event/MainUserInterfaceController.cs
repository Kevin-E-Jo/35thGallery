using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUserInterfaceController : MonoBehaviour
{
    [SerializeField] private Button ExitBtn;
    [SerializeField] private Button movePastBtn;
    [SerializeField] private Button moveFutureBtn;
    [SerializeField] private Button moveLobbyBtn;
    [SerializeField] private Button hiBtn;
    [SerializeField] private Button runBtn;
    [SerializeField] private Button guestBookBtn;
    [SerializeField] private Button tutorialBtn;
    [SerializeField] private GameObject tutorialObj;
    [SerializeField] private Button tutorialCloseBtn;
    [SerializeField] private GameObject loadingObj;
    [SerializeField] private GameObject upperRightObj;
    [SerializeField] private GameObject lowerCenterObj;
    [SerializeField] private GameObject lowerRightObj;
    [SerializeField] private GameObject eventObj;
    [SerializeField] private Button eventCloseBtn;

    private void Awake()
    {
        ExitBtn.onClick.AddListener(delegate { ReactCommunicator.Instance.WhenUserLeavesRoom(); });
        
        movePastBtn.onClick.AddListener(delegate
        {
            SetLoadingObjectActive(true);
            movePastBtn.gameObject.SetActive(false);
            moveFutureBtn.gameObject.SetActive(true);
            moveLobbyBtn.gameObject.SetActive(true);
            ExitBtn.gameObject.SetActive(false);
            GameEvents.Instance.RequestTeleport(new Vector3(-566.69f, 10f, -533.4307f),new Vector3(0f, 0f, 0f));
        });
        moveFutureBtn.onClick.AddListener(delegate
        {
            SetLoadingObjectActive(true);
            movePastBtn.gameObject.SetActive(true);
            moveFutureBtn.gameObject.SetActive(false);
            moveLobbyBtn.gameObject.SetActive(true);
            ExitBtn.gameObject.SetActive(false);
            GameEvents.Instance.RequestTeleport(new Vector3(341.3403f, 10f, -449.0208f),new Vector3(0f, 120f, 0f));
        });
        moveLobbyBtn.onClick.AddListener(delegate {
            SetLoadingObjectActive(true);
            movePastBtn.gameObject.SetActive(false);
            moveFutureBtn.gameObject.SetActive(false);
            moveLobbyBtn.gameObject.SetActive(false);
            ExitBtn.gameObject.SetActive(true);
            GameEvents.Instance.RequestTeleport(new Vector3(26.3166f, 10f, 9.85672f),new Vector3(0f, 180f, 0f));
        });
        
#if UNITY_WEBGL && !UNITY_EDITOR            
            guestBookBtn.onClick.AddListener(delegate { ReactCommunicator.Instance.SendBoardOpenToJS(); });
#endif       
        tutorialBtn.onClick.AddListener(delegate { tutorialObj.SetActive(!tutorialObj.activeSelf); });
        tutorialCloseBtn.onClick.AddListener(delegate { tutorialObj.SetActive(false); });
        
        eventCloseBtn.onClick.AddListener(delegate { eventObj.SetActive(false); });

        GameEvents.Instance.OnRequestUIButtonReset += UIReset;
        //GameEvents.Instance.OnRequestOpenEventInfo += SetEventObjectActive;
    }

    private void Start()
    {
        ExitBtn.gameObject.SetActive(true);
        movePastBtn.gameObject.SetActive(false);
        moveFutureBtn.gameObject.SetActive(false);
        moveLobbyBtn.gameObject.SetActive(false);
        tutorialObj.SetActive(false);
        loadingObj.SetActive(false);
        eventObj.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnRequestUIButtonReset -= UIReset;
            //GameEvents.Instance.OnRequestOpenEventInfo -= SetEventObjectActive;
        }
            
    }

    private void SetLoadingObjectActive(bool active)
    {
#if UNITY_WEBGL && !UNITY_EDITOR        
        GameEvents.Instance.RequestLoadingSetActive(active);
#elif UNITY_EDITOR
        loadingObj.SetActive(active);
#endif
        lowerCenterObj.SetActive(!active);
        lowerRightObj.SetActive(!active);
        upperRightObj.SetActive(!active);
    }
    
    private void SetEventObjectActive()
    {
        eventObj.SetActive(!eventObj.activeSelf);
    }
    
    
    
    private void UIReset(string whereto)
    {
        switch (whereto)
        {
            case "past":
                movePastBtn.gameObject.SetActive(false);
                moveFutureBtn.gameObject.SetActive(true);
                moveLobbyBtn.gameObject.SetActive(true);
                ExitBtn.gameObject.SetActive(false);
                break;
            case "future":
                movePastBtn.gameObject.SetActive(true);
                moveFutureBtn.gameObject.SetActive(false);
                moveLobbyBtn.gameObject.SetActive(true);
                ExitBtn.gameObject.SetActive(false);
                break;
            case "lobby":
                movePastBtn.gameObject.SetActive(false);
                moveFutureBtn.gameObject.SetActive(false);
                moveLobbyBtn.gameObject.SetActive(false);
                ExitBtn.gameObject.SetActive(true);
                break;
        }
    }
}
