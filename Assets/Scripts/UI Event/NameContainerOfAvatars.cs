using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NameContainerOfAvatars
{
    Dictionary<int, string> dicForAvatarNames = new Dictionary<int, string>();

    public NameContainerOfAvatars()
    {
        var list = Enum.GetNames(typeof(NamesOfAvaterPrefabs)).ToList();

        int key = 1;
        foreach (var elements in list)
        {
            dicForAvatarNames.Add(key, elements);
            key++;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string GetName(int index)
    {
        return dicForAvatarNames[index];
    }
}
