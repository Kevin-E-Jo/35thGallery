using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AvatarAnimationController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public GameObject Player;

    public GameObject hiBtn;
    public GameObject hiPushBtn;

    public void Start()
    {
        StartCoroutine(PlayerCheck());
        //_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

        //GameEvents.Instance.OnRequestSetActivePlayerInputSys += SetActivePlayerInputSys;
        //MainEventBus.Instance.Subscribe(MainGameEventType.inactivatePlayerInputSys, InactivatePlayerInputSys);
        //MainEventBus.Instance.Subscribe(MainGameEventType.activatePlayerInputSys, ActivatePlayerInputSys);
    }

    IEnumerator PlayerCheck()
    {
        while (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    

    public void OnDestroy()
    {
        //if (GameEvents.Instance != null)
            //GameEvents.Instance.OnRequestSetActivePlayerInputSys -= SetActivePlayerInputSys;
        //MainEventBus.Instance.UnSubscribe(MainGameEventType.inactivatePlayerInputSys, InactivatePlayerInputSys);
        //MainEventBus.Instance.UnSubscribe(MainGameEventType.activatePlayerInputSys, ActivatePlayerInputSys);
    }

    
    //void SetActivePlayerInputSys(bool active)
    //{
    //    if (transform.tag.Equals("Player")){
    //        GetComponent<PlayerInput>().enabled = active;
    //    }
    //}

    public void OnPlayAnimation(string animationName)
    {
        if (Player == null)
            StartCoroutine(PlayerCheck());
        //eventBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        else
        {
            hiBtn.SetActive(false);
            hiPushBtn.SetActive(true);
            Player.GetComponent<Animator>().SetTrigger(animationName);
            StartCoroutine(WaitAnimation());
        }
    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(3.0f);

        hiBtn.SetActive(true);
        hiPushBtn.SetActive(false);

        yield return null;
    }

    public void OnConvertSprint(bool isSprint)
    {
        GameEvents.Instance.RequestSetActiveSprint(isSprint);
    }
    
}
