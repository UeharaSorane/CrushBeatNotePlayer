using System.Collections;
using System.Collections.Generic;
using TrackDataClass;
using UnityEngine;

public class SendNote : MonoBehaviour
{
    public GameObject[] BeatNote = new GameObject[6];
    public GameObject[] CrushNote = new GameObject[5];
    GameObject Manager, JudgeLine;

    float songpos;

    float lastJudge,firstNote,offset;
    float Bpm, PlayerSpeed, DSTT;
    List<NoteData> ND;
    List<HoldData> HD;
    List<CommandData> CD;

    int nowNote,nid,nowHold,nowCommand;

    bool isReady = false;

    public void setNote(float bpm,float s,float dstt, float o, List<NoteData> nd, List<HoldData> hd, List<CommandData> cd,float lj)
    {
        Manager = GameObject.Find("Manager");
        JudgeLine = GameObject.Find("JudgeLine");
        nowNote = 0;
        nid = 0;
        nowHold = 0;
        nowCommand = 0;

        Bpm = bpm;
        PlayerSpeed = s;
        DSTT = dstt * 120/bpm;
        offset = o;//offset越大代表音符越晚到,反之則越早到
        ND = nd;
        HD = hd;
        CD = cd;

        firstNote = ND[0].JudgeTime - DSTT / PlayerSpeed;

        lastJudge = lj*60/bpm + offset;

        foreach (NoteData n in ND)
        {
            n.JudgeTime = n.JudgeTime * 60 / bpm + offset;
        }

        foreach (CommandData n in CD)
        {
            n.time = n.time * 60 / bpm + offset;
        }

        foreach (HoldData h in HD)
        {
            foreach (float[] p in h.points)
            {
                p[1] = p[1] * 60 / bpm + offset;
            }
        }


        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            songpos = Manager.GetComponent<Conductor>().getPos();

            if (nowNote < ND.Count)
            {
                if (songpos >= ND[nowNote].JudgeTime - DSTT / PlayerSpeed)
                {
                    //Debug.Log(ND[nowNote].JudgeTime);
                    createNote();
                    nowNote++;
                }
            }
            if (songpos >= lastJudge)
            {
                Destroy(this.gameObject);
            }

            if (nowCommand < CD.Count)
            {
                if (songpos >= CD[nowCommand].time - DSTT / PlayerSpeed)
                {
                    switch (CD[nowCommand].type)
                    {
                        case 0:
                            DSTT *= Bpm / CD[nowCommand].parameter;
                            break;
                    }

                    nowCommand++;
                }
            }
        }
    }

    void createNote()
    {
        NoteData note = ND[nowNote];
        if(note.road == 0)
        {
            if (note.type != 5)
            {
                Vector3 StartPos = transform.GetChild(0).position;
                StartPos.x = -9 + (float)note.Start / 2;
                Vector3 DesPos = JudgeLine.transform.position;
                DesPos.x = -9 + (float)note.Des / 2;

                GameObject NoteOBJ = Instantiate(BeatNote[note.type]);

                Vector3 L = BeatNote[note.type].transform.localScale;
                L.x = note.length;
                NoteOBJ.transform.localScale = L;

                //Debug.Log(note.JudgeTime - DSTT / PlayerSpeed + " " + note.JudgeTime);
                NoteOBJ.GetComponent<BeatAct>().setNote(nid,note.type,StartPos,DesPos, note.JudgeTime, note.JudgeTime - DSTT / PlayerSpeed,false);
                nid++;
            }
            else
            {
                //Debug.Log(HD[nowHold].StartType +" " + HD[nowHold].EndType);

                List<HoldPointInformation> Hp = new List<HoldPointInformation>();

                for (int i = 0; i < HD[nowHold].points.Length; i++)
                {
                    Hp.Add(new HoldPointInformation(i, HD[nowHold].points[i][3], HD[nowHold].points[i][4], HD[nowHold].points[i][2], HD[nowHold].points[i][1] - DSTT / PlayerSpeed, HD[nowHold].points[i][1]));
                    //Debug.Log(HD[nowHold].points[i][0] + " " + HD[nowHold].points[i][1] + " " + HD[nowHold].points[i][2] + " " + HD[nowHold].points[i][3] + " " + HD[nowHold].points[i][4] + " ");
                }

                GameObject NoteOBJ = Instantiate(BeatNote[5]);
                NoteOBJ.GetComponent<BeatHolding>().setHold(nid,
                                                            new GameObject[] { BeatNote[0], BeatNote[1], BeatNote[2], BeatNote[3], BeatNote[4] },
                                                            Hp,
                                                            HD[nowHold].StartType,
                                                            HD[nowHold].EndType,
                                                            (Hp[Hp.Count - 1].JudgeTime - Hp[0].JudgeTime) * 0.8f);
                nid += 2;
                nowHold++;

            }

        }
        if (note.road == 1)
        {
            if (note.type != 4)
            {
                Vector3 StartPos = transform.position;
                StartPos.x = -9 + (float)note.Start / 2;

                Vector3 DesPos = JudgeLine.transform.position;
                DesPos.x = -9 + (float)note.Des / 2;

                GameObject NoteOBJ = Instantiate(CrushNote[note.type],StartPos, CrushNote[note.type].transform.rotation);
                Vector3 L = BeatNote[note.type].transform.localScale;
                L.y = (float)note.length / 2;
                
                NoteOBJ.transform.localScale = L;

                NoteOBJ.GetComponent<CrushAct>().setNote(nid, note.type, StartPos, DesPos, note.JudgeTime, note.JudgeTime - DSTT / PlayerSpeed,false);
                nid++;
            }
            else
            {
                List<HoldPointInformation> Hp = new List<HoldPointInformation>();

                for (int i = 0; i < HD[nowHold].points.Length; i++)
                {
                    Hp.Add(new HoldPointInformation(i, HD[nowHold].points[i][3], HD[nowHold].points[i][4], HD[nowHold].points[i][2], HD[nowHold].points[i][1] - DSTT / PlayerSpeed, HD[nowHold].points[i][1]));
                    //Debug.Log(HD[nowHold].points[i][0] + " " + HD[nowHold].points[i][1] + " " + HD[nowHold].points[i][2] + " " + HD[nowHold].points[i][3] + " " + HD[nowHold].points[i][4] + " ");
                }


                GameObject NoteOBJ = Instantiate(CrushNote[4]);
                NoteOBJ.GetComponent<CrushHolding>().setHold(nid,
                                                            new GameObject[] { CrushNote[0], CrushNote[1], CrushNote[2], CrushNote[3]},
                                                            Hp,
                                                            HD[nowHold].StartType,
                                                            HD[nowHold].EndType,
                                                            (Hp[Hp.Count - 1].JudgeTime - Hp[0].JudgeTime) * 0.8f);
                nid += 2;
                nowHold++;

            }

        }
    }

    public float getFirstNote()
    {
        return firstNote;
    }
}
