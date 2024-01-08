using System;
using UnityEngine;
using UnityEngine.Networking;

public class ArtExhibit : MonoBehaviour, IWWWRequestCallback
{
    public string id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public void Initialize(string id)
    {
        this.id = id;
        WebRequestEventBus.Instance.Subscribe(this);
        MeshRendererOnOff(false);
    }

    /// <summary>
    /// 
    /// </summary>
    void Unsubscribe()
    {
        WebRequestEventBus.Instance.UnSubscribe(this);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnDisable()
    {
        Unsubscribe();
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnWWWRequestCallback(EventArgs e)
    {
        //Debug.LogError("OnWWWRequestCallback");
        var eventArgs = e as ArtObjects;
        SetArtObjects(new NewMaterials().CreateMaterials(e, "Universal Render Pipeline/Lit"));
        Resize(eventArgs.data);
    }

    /// <summary>
    /// 
    /// </summary>
    void SetArtObjects(Material material)
    {
        Transform picture = transform.Find("picture");
        MeshRenderer mr = picture.GetComponent<MeshRenderer>();
        MeshRendererOnOff(true);
        mr.material = material;
    }

    /// <summary>
    /// 
    /// </summary>
    void Resize(ArtObjectsInfo data)
    {
        float ratio = float.Parse(data.img_width) / float.Parse(data.img_height);

        transform.eulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
        Vector3 scale = new Vector3(1.0f, ratio, 1.0f);

        SetScale("picture", scale);
        SetScale("frame", scale);
    }

    /// <summary>
    /// 
    /// </summary>
    void SetScale(string childName, Vector3 scale)
    {
        transform.Find(childName).localScale = scale;
    }

    /// <summary>
    /// 
    /// </summary>
    void MeshRendererOnOff(bool On)
    {
        Transform picture = transform.Find("picture");
        MeshRenderer mr = picture.GetComponent<MeshRenderer>();
        mr.enabled = On;
    }
}
