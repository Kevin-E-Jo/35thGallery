using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMidiaPlayer
{
    public void OnPlay();
    public void OnStop();
    public void OnPause(EventArgs args = null);
    public void OnResume(EventArgs args = null);
}

public class MidiaPlayer : MonoBehaviour
{
    GameObject Target;

    private void Start()
    {
        Target = transform.Find("Player").gameObject;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            Target.SetActive(true);
            Target.GetComponent<IMidiaPlayer>().OnPlay();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            Target.transform.GetComponent<IMidiaPlayer>().OnStop();
            Target.SetActive(false);
        }
    }
}
