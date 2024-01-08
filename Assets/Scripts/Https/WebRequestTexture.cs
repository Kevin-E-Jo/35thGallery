using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestTexture : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public void Execute(EventArgs args)
    {
        StartCoroutine(RequestTexture(args));
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator RequestTexture(EventArgs args)
    {
        ArtObjects objects = (args as ArtObjects);

        using (var www = UnityWebRequestTexture.GetTexture(objects.data.img_url))
        {
            yield return www.SendWebRequest();

            objects.handler = www.downloadHandler;
            objects.action?.Invoke(objects);
        }
    }
}

