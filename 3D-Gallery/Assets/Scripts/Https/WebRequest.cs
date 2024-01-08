using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    public void Start()
    {
#if UNITY_EDITOR
        StartCoroutine(WebRequestPost("localhost:3400/v1/gallary"));
#else
        StartCoroutine(WebRequestPost("http://43.200.133.6/v1/gallary/v1/gallary")); 
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
                                WebRequestEventBus.Instance.Publish(objects);
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

                }
                break;
            case UnityWebRequest.Result.ProtocolError:
                {

                }
                break;
            case UnityWebRequest.Result.DataProcessingError:
                {

                }
                break;
        }
    }
}
