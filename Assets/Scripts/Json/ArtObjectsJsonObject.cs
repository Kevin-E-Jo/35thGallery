using System;
using System.Collections.Generic;

[Serializable]
public struct ArtObjectsInfo
{
    public string id;
    public string title;
    public string introduction;
    public string img_url;
    public string img_width;
    public string img_height;
    public string share_url;
}

[Serializable]
public class Result
{
    public List<ArtObjectsInfo> lists;
}

[Serializable]
public class ArtObjectsJsonObject : IJsonObject
{
    public string resultCode;
    public string resultMessage;
    public Result result;
}
