using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    GameObject Manager;
    float playTime;
    bool ready = false;

    public void PlaySound(string se,float t)
    {
        Manager = GameObject.Find("Manager");

        playTime = t;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("SE/" + se);
        
        ready = true;
    }

    void Update()
    {
        float songpos = Manager.GetComponent<Conductor>().getPos();
        if (ready && songpos >= playTime)
        {
            ready = false;
            GetComponent<AudioSource>().Play();
            Destroy(this.gameObject, GetComponent<AudioSource>().clip.length);
        }
    }
}
