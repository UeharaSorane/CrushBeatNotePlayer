using UnityEngine;

public class Conductor : MonoBehaviour
{
    GameObject info;
    float songPosition;//�֦��i��
    float firstNote,lastJudge;
    float StartTime;//�֦��}�l���ɶ�(dspTime)
    float StartPos;

    int status,laststatus;//0=���}�l,1=�t����,2=�Ȱ�,3=����

    public void setConductor(float fn,float lj)
    {
        info = GameObject.Find("info");

        status = 0;
        songPosition = -99999;
        firstNote = fn;
        lastJudge = lj;
    }

    public void StartGame()
    {
        if (firstNote < 0) StartPos = firstNote;
        else StartPos = 0;
        StartTime = (float)AudioSettings.dspTime;
        Invoke("PlayMusic", -StartPos);
        status = 1;
    }

    void PlayMusic()
    {
        if(status != 2)GetComponent<MusicPlayer>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (status == 1 || status == 3)
        {
            songPosition = (float)AudioSettings.dspTime - StartTime + StartPos;
            info.GetComponent<InfoPanel>().setProgess(GetComponent<AudioSource>().time / GetComponent<AudioSource>().clip.length);
            if (songPosition >= lastJudge + 1 && status == 1)
            {
                Debug.Log("Note_End");
                status = 3;
            }
        }
        else
        {
            songPosition = -999999;
            info.GetComponent<InfoPanel>().setProgess(0);
        }
    }

    public float getPos()
    {
        return songPosition;
    }
}
