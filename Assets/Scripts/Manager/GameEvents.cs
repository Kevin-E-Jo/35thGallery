using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ExhibitionType
{
    image=0,
    video,
    audio
}
public class GameEvents : Singleton<GameEvents>
{
    
    
    public event Action<bool> OnRequestPauseGalleryVideo;
    public void RequestPauseGalleryVideo(bool pause)
    {
        if (OnRequestPauseGalleryVideo != null)
        {
            OnRequestPauseGalleryVideo(pause);
        }
    }
    
    public event Action<bool> OnRequestPauseGalleryAudio;
    public void RequestPauseGalleryAudio(bool pause)
    {
        if (OnRequestPauseGalleryAudio != null)
        {
            OnRequestPauseGalleryAudio(pause);
        }
    }
    
    public event Action<bool> OnRequestBlockRaycast;
    public void RequestBlockRaycast(bool block)
    {
        if (OnRequestBlockRaycast != null)
        {
            OnRequestBlockRaycast(block);
        }
    }
    
    public event Action<bool> OnRequestSetActivePlayerInputSys;
    public void RequestSetActivePlayerInputSys(bool active)
    {
        if (OnRequestSetActivePlayerInputSys != null)
        {
            OnRequestSetActivePlayerInputSys(active);
        }
    }

    public event Action<bool> OnRequestSetActiveSprint;
    public void RequestSetActiveSprint(bool active)
    {
        if (OnRequestSetActiveSprint != null)
        {
            OnRequestSetActiveSprint(active);
        }
    }

    public event Action<ExhibitionType,string,float> OnRequestExhibitContents;
    public void RequestExhibitContents(ExhibitionType type,string item,float ratio)
    {
        if (OnRequestExhibitContents != null)
        {
            OnRequestExhibitContents(type,item,ratio);
        }
    }
    
    public event Action<Vector3,Vector3> OnRequestTeleport;
    public void RequestTeleport(Vector3 pos,Vector3 rot)
    {
        if (OnRequestTeleport != null)
        {
            OnRequestTeleport(pos,rot);
        }
    }
    
    public event Action<string> OnRequestUIButtonReset;
    public void RequestUIButtonReset(string whereto)
    {
        if (OnRequestUIButtonReset != null)
        {
            OnRequestUIButtonReset(whereto);
        }
    }

    public event Action<string> OnRequestOpenEventInfo;
    public void RequestOpenEventInfo(string name)
    {
        if (OnRequestOpenEventInfo != null)
        {
            OnRequestOpenEventInfo(name);
        }
    }
    
    public event Action<bool> OnRequestLoadingSetActive;
    public void RequestLoadingSetActive(bool active)
    {
        if (OnRequestLoadingSetActive != null)
        {
            OnRequestLoadingSetActive(active);
        }
    }
    
    public event Action<string> OnRequestPlayerAction;
    public void RequestPlayerAction(string action)
    {
        if (OnRequestPlayerAction != null)
        {
            OnRequestPlayerAction(action);
        }
    }
    
    public event Action<string> OnRequestAvatarModify;
    public void RequestAvatarModify(string msg)
    {
        if (OnRequestAvatarModify != null)
        {
            OnRequestAvatarModify(msg);
        }
    }
    
    public event Action OnRequestLobbyVideoStart;
    public void RequestLobbyVideoStart()
    {
        if (OnRequestLobbyVideoStart != null)
        {
            OnRequestLobbyVideoStart();
        }
    }
}
