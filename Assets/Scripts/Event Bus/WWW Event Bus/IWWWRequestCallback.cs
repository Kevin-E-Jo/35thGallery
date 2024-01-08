
using System;
using UnityEngine.Networking;

public interface IWWWRequestCallback
{
    string id { set; get; }
    public void OnWWWRequestCallback(EventArgs e);
}
