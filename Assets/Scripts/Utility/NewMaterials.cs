using System;
using UnityEngine;
using UnityEngine.Networking;

public class NewMaterials
{
    public Material CreateMaterials(EventArgs e, string shader)
    {
        var eventArgs = e as ArtObjects;
        var material = CreateMaterials(shader);
        material.mainTexture = NewTexture(eventArgs.handler);

        return material;
    }

    /// <summary>
    /// 
    /// </summary>
    Texture NewTexture(DownloadHandler handler)
    {
        return ((DownloadHandlerTexture)handler).texture;
    }

    /// <summary>
    /// 
    /// </summary>
    Material CreateMaterials(string shaderName)
    {
        return new Material(Shader.Find(shaderName));
    }
}
