using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayer_Intro : VideoPlayer_2D
{
    public string resourceName = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        PlayLink(resourceName);
    }
}
