using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        GameObject target = GameObject.FindGameObjectWithTag("ArtExhibitList");
        Component[] components = target.GetComponentsInChildren<ArtExhibit>();

        int index = 1;
        foreach (ArtExhibit component in components)
        {
            component.Initialize(index.ToString());
            index++;
        }
    }
}
