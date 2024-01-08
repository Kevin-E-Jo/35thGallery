using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRepository : Singleton<LocalRepository>
{
    UserProfile userProfile = new UserProfile();
    public UserProfile UserProfile{
        set
        {
            userProfile = value;
        }
        get
        {
            return userProfile;
        }
        
    }
    public ExhibitJsonObject exhibitObject { private set; get; }

    public void Init()
    {
        exhibitObject = UnityJsonUtility.Instance.FromJson<ExhibitJsonObject>(Resources.Load("Json/ExhibitInfo").ToString());
    }

    public ExhibitInfo FindExhibitInfo(string id)
    {
        return exhibitObject.lists.Find(t => t.id.Equals(id));
    }

    /// <summary>
    /// 
    /// </summary>
    public T FactoryMethod<T> ()
    {
        T instance = (T)Activator.CreateInstance(typeof(T));
        return instance;
    }
}
