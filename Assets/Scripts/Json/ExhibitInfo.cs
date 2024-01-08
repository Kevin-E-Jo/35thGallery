using System;
using System.Collections.Generic;

[Serializable]
public struct ExhibitInfo
{
    public string id;
    public string title;
    public string ratio;
    public string author;
    public string introduction;
    public string imgUrl;
    public string imgWidth;
    public string imgHeight;
    public string shareUrl;
    public string image;
}

[Serializable]
public class ExhibitJsonObject : IJsonObject
{
    public List<ExhibitInfo> lists;
}
