using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WebRequestEventBus : UnitySingleton<WebRequestEventBus>
{
    List<IWWWRequestCallback> subscribersList = null;

    public override void Awake()
    {
        base.Awake();
        subscribersList = new List<IWWWRequestCallback>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Subscribe(ArtExhibit subscriber)
    {
        if (subscribersList.Find(t => t.GetHashCode().Equals(subscriber.GetHashCode())) == null)
        {
            subscribersList.Add(subscriber);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void UnSubscribe(ArtExhibit target)
    {
        var elements = subscribersList.Find(t => t.GetHashCode().Equals(target.GetHashCode()));

        if (elements != null)
        {
            subscribersList.Remove(elements);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Publish(EventArgs args)
    {
        var objects = args as ArtObjects;
        var elements = subscribersList.Find(t => t.id.Equals(objects.data.id));
        elements?.OnWWWRequestCallback(args);
    }
}
