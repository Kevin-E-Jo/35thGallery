using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    public void WebRequestPost()
    {
#if UNITY_EDITOR
        StartCoroutine(WebRequestPost("http://10.10.5.52:3400/v1/gallary"));
#else
        StartCoroutine(WebRequestPost("https://api-gl.torynft.co.kr/v1/gallary"));
        //StartCoroutine(WebRequestPost("http://10.10.5.88:3400/v1/gallary"));
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator WebRequestPost(string uri)
    {
        var form = new WWWForm();
        using (var www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();
            GetResult(www);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void GetResult(UnityWebRequest www)
    {
        switch (www.result)
        {
            case UnityWebRequest.Result.InProgress:
                {
                    Debug.LogError("UnityWebRequest.Result.InProgress:");
                }
                break;
            case UnityWebRequest.Result.Success:
                {
                    var result = UnityJsonUtility.Instance.FromJson<ArtObjectsJsonObject>(www.downloadHandler.text);

                    result.result.lists.ForEach(
                        element => {
                            ArtObjects objects = new ArtObjects();
                            objects.data = element;

                            objects.action = (EventArgs data) => {
                                WebRequestEventBus.Instance.Publish(data);
                            };

                            WebRequestTexture wt = FindObjectOfType<WebRequestTexture>();

                            if (wt == null)
                            {
                                GameObject obj = new GameObject();
                                obj.name = typeof(WebRequestTexture).Name;
                                wt = obj.AddComponent<WebRequestTexture>();
                            }
                            
                            wt.Execute(objects);
                        }
                    );
                }
                break;
            case UnityWebRequest.Result.ConnectionError:
                {
                    Debug.LogError("UnityWebRequest.Result.ConnectionError");
                }
                break;
            case UnityWebRequest.Result.ProtocolError:
                {
                    Debug.LogError("UnityWebRequest.Result.ProtocolError");
                }
                break;
            case UnityWebRequest.Result.DataProcessingError:
                {
                    Debug.LogError("UnityWebRequest.Result.DataProcessingError");
                }
                break;
        }
    }
}
