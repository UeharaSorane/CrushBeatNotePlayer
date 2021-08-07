using System.Collections;
using System.Collections.Generic;
using TrackDataClass;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlay : MonoBehaviour
{
    float baseWidth = 1280;
    float baseHeight = 720;
    float DSTT = 5;

    GameObject MC,SendNote, InfoPanel,PlayerStatus;

    private void Start()
    {
        MC = GameObject.FindWithTag("MainCamera");
        MC.GetComponent<Camera>().aspect = this.baseWidth / this.baseHeight;
        InformationData info = MC.GetComponent<ReadTrackData>().GetInfo();

        InfoPanel = GameObject.Find("info");
        InfoPanel.GetComponent<InfoPanel>().setInfo(info, MC.GetComponent<ReadTrackData>().GetCover(), MC.GetComponent<ReadTrackData>().Difficulty);
        InfoPanel.GetComponent<InfoPanel>().show();

        PlayerStatus = GameObject.Find("PlayerStatus");
        int NoteCount = MC.GetComponent<ReadTrackData>().GetND().Count + MC.GetComponent<ReadTrackData>().GetHD().Count;
        PlayerStatus.transform.GetChild(0).GetComponent<Tension>().setTension(MC.GetComponent<PlayerSetting>().getMode(), NoteCount);
        PlayerStatus.transform.GetChild(2).GetComponent<ShowCombo>().setCombo(NoteCount);
        PlayerStatus.transform.GetChild(3).GetComponent<Score>().setNoteScore(NoteCount, MC.GetComponent<ReadTrackData>().GetHD().Count);
        PlayerStatus.GetComponent<CanvasGroup>().alpha = 1;

        SendNote = GameObject.Find("SendNote");
        SendNote.GetComponent<SendNote>().setNote(info.bpm,
                                                  MC.GetComponent<PlayerSetting>().getSpeed(),
                                                  DSTT, 
                                                  info.Offset + MC.GetComponent<PlayerSetting>().getOffset(),
                                                  MC.GetComponent<ReadTrackData>().GetND(),
                                                  MC.GetComponent<ReadTrackData>().GetHD(),
                                                  MC.GetComponent<ReadTrackData>().GetCD(),
                                                  MC.GetComponent<ReadTrackData>().getJastJudge());
        GetComponent<Conductor>().setConductor(SendNote.GetComponent<SendNote>().getFirstNote(),
                                               MC.GetComponent<ReadTrackData>().getJastJudge() +info.Offset + MC.GetComponent<PlayerSetting>().getOffset());
        GetComponent<MusicPlayer>().setMusic(MC.GetComponent<ReadTrackData>().GetClip());

        Invoke("StartGame", 3);
    }

    void StartGame()
    {
        GetComponent<Conductor>().StartGame();
    }
}
