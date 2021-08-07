using System.Collections;
using UnityEngine;
public class MusicPlayer : MonoBehaviour
{

    public void setMusic(AudioClip m)
    {
        GetComponent<AudioSource>().clip = m;
    }

    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }

    public void Paused()
    {
        GetComponent<AudioSource>().Pause();
    }

    public void setTime(float pos)
    {
        if (pos < 0) pos = 0;

        GetComponent<AudioSource>().time = pos;
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }
}
