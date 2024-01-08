using System;
using UnityEngine.Networking;

public class ArtObjects : EventArgs
{
    public DownloadHandler handler { set; get; }
    public ArtObjectsInfo data { set; get; }
    public Action<EventArgs> action;
}
