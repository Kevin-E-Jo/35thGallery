using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayer_2D : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    private const string url = "";//"https://torynft-gallery.s3.ap-northeast-2.amazonaws.com/";

    /// <summary>
    /// 
    /// </summary>
    public void Play(string target)
    {
        LoadVideoResource(target);
    }

    public void PlayLink(string target)
    {
        LoadVideoResource(target, true);
    }
    
    public void Stop()
    {
        videoPlayer.Stop();
    }

    void LoadVideoResource(string target, bool link =false)
    {
        //videoPlayer.url = Application.streamingAssetsPath + "/" + target + ".mp4";
        if(link)
            videoPlayer.url = url + target + ".mp4";
        else
        {
#if UNITY_EDITOR
            videoPlayer.url = "file://" + Application.streamingAssetsPath + "/" + target+ ".mp4";
#else
	         videoPlayer.url = Application.streamingAssetsPath + "/" + target+ ".mp4";
#endif
        }
        Debug.Log("video url : " + videoPlayer.url);
        StartCoroutine(LoadVideoResource()); 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadVideoResource()
    {
        videoPlayer.Prepare();

        float time = 0.0f;
        while (true)
        {
            time += Time.deltaTime;

            if (videoPlayer.isPrepared) {
                break;
            }
                
            if (10.0f < time) {
                break;
            }
                
            yield return null;
        }

        if(rawImage != null)
            rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
}
