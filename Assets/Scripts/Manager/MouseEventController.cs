using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventController : MonoBehaviour
{
    private void Start()
    {
        //MainEventBus.Instance.Subscribe(MainGameEventType.blockRaycastHit, blockRaycastHit);
        //MainEventBus.Instance.Subscribe(MainGameEventType.unblockRaycastHit, unblockRaycastHit);
        GameEvents.Instance.OnRequestBlockRaycast += blockRaycastHit;
    }

    private void OnDestroy()
    {
        if(GameEvents.Instance != null)
            GameEvents.Instance.OnRequestBlockRaycast -= blockRaycastHit;
    }

    void blockRaycastHit(bool block)
    {
        isBlocking = block;
    }


    /// <summary>
    /// 
    /// </summary>
    private void Raycasthit()
    {

        //int layerMask = (1 << LayerMask.NameToLayer("Exhibit")) + (1 << LayerMask.NameToLayer("Wall"));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider == null) return;
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        //{
            string tag = hit.transform.tag;
            Debug.Log("hit obj tag : "+tag);
            if (tag.Equals("Exhibit") || tag.Equals("Video Display") || tag.Equals("Voice exhibit"))
            {
                string item = hit.transform.name;
                switch (tag)
                {
                    case "Exhibit":
                        GameEvents.Instance.RequestExhibitContents(ExhibitionType.image,item,hit.transform.localScale.x/hit.transform.localScale.y);
                        break;
                    case "Video Display":
                        GameEvents.Instance.RequestExhibitContents(ExhibitionType.video,item,hit.transform.localScale.x/hit.transform.localScale.y);
                        break;
                    case "Voice exhibit":
                        GameEvents.Instance.RequestExhibitContents(ExhibitionType.audio,item,hit.transform.localScale.x/hit.transform.localScale.y);
                        break;
                }
            }
            else if (tag.Equals("Door"))
            {
                GameObject whereto = new GameObject();
                whereto = hit.transform.gameObject;
                bool top = false;
                while (!top)
                {
                    if (whereto.transform.parent.gameObject.layer.Equals(LayerMask.NameToLayer("Door")))
                    {
                        whereto = whereto.transform.parent.gameObject;
                    }
                    else
                        top = true;
                }

                //GameEvents.Instance.RequestLoadingObjSetActive(true);
                GameEvents.Instance.RequestLoadingSetActive(true);
                GameEvents.Instance.RequestUIButtonReset(whereto.name);
                switch (whereto.name)
                {
                    case "lobby":
                        //new Vector3(0f, 10f, 0f)
                        ReactCommunicator.Instance.CurrentLocation = "LOBBY";
                        ReactCommunicator.Instance.SendHeaderLayoutTypetoJS("LOBBY");
                        GameEvents.Instance.RequestTeleport(new Vector3(26.3166f, 10f, 9.85672f),new Vector3(0f, 180f, 0f));
                        break;
                    case "past":
                        //new Vector3(-500f, 10f, -500f)
                        ReactCommunicator.Instance.CurrentLocation = "PAST";
                        ReactCommunicator.Instance.SendHeaderLayoutTypetoJS("PAST");
                        GameEvents.Instance.RequestTeleport(new Vector3(-566.69f, 10f, -533.4307f),new Vector3(0f, 0f, 0f));
                        GameEvents.Instance.RequestPauseGalleryVideo(true);
                        break;
                    case "future":
                        //new Vector3(370f, 10f, -500f)
                        ReactCommunicator.Instance.CurrentLocation = "FUTURE";
                        ReactCommunicator.Instance.SendHeaderLayoutTypetoJS("FUTURE");
                        GameEvents.Instance.RequestTeleport(new Vector3(341.3403f, 10f, -449.0208f),new Vector3(0f, 120f, 0f));
                        GameEvents.Instance.RequestPauseGalleryVideo(true);
                        break;
                }
            }
            else if (tag.Equals("Event"))
            {
                GameObject poster = new GameObject();
                poster = hit.transform.gameObject;
                bool top = false;
                while (!top)
                {
                    if (poster.transform.parent.gameObject.tag.Equals("Event"))
                    {
                        poster = poster.transform.parent.gameObject;
                    }
                    else
                        top = true;
                }
                GameEvents.Instance.RequestOpenEventInfo(poster.name);
            }
            //}
    }

    /// <summary>
    /// 
    /// </summary>
    
    bool isBlocking = false;
    private void Update()
    {
        if ( Input.GetMouseButtonDown(0) &&
             isBlocking == false)
        {
            Raycasthit();
        }
    }
}
