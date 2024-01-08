using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityJsonUtility : Singleton<UnityJsonUtility>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    public string ToJson<T>(T obj) where T : IJsonObject
    {
        return JsonUtility.ToJson(obj);
    }

    /// <summary>
    /// 
    /// </summary>
    public T FromJson<T>(string json) where T : IJsonObject
    {
        return JsonUtility.FromJson<T>(json);
    }

}
