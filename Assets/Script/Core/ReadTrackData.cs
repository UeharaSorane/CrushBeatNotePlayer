using MyException;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TrackDataClass;
using UnityEngine;
using UnityEngine.Networking;

public class ReadTrackData : MonoBehaviour
{
    public int Difficulty;
    float lastJudge;

    InformationData InfoData;
    Sprite cover;

    List<NoteData> ND;
    List<HoldData> HD;
    List<CommandData> CD;
    AudioClip clip;

    void Awake()
    {
        ReadInfo();
        ReadNote();
    }

    void ReadNote()
    {
        ND = new List<NoteData>();
        HD = new List<HoldData>();
        CD = new List<CommandData>();

        int nid = 0;
        int hCount = 0;
        int hid = 0;
        int cid = 0;

        clip = Resources.Load("Track") as AudioClip;

        if (Difficulty >= InfoData.Level.Length || Difficulty < 0)
        {
            throw new TrackInfoHasProblemException();
        }
        else
        {
            TextAsset Note;
            switch (Difficulty)
            {
                case 0:
                    Note = Resources.Load("Dot") as TextAsset;
                    break;
                case 1:
                    Note = Resources.Load("Line") as TextAsset;
                    break;
                case 2:
                    Note = Resources.Load("Surface") as TextAsset;
                    break;
                case 3:
                    Note = Resources.Load("Dimension") as TextAsset;
                    break;
                default:
                    Note = Resources.Load("Special" + (Difficulty - 3)) as TextAsset;
                    break;
            }
            if (Note == null || clip == null) throw new TrackDataNotFoundException();
            else
            {
                string[] s = Note.text.Split("\r\n"[0]);

                for(int i = 0; i < s.Length; i++)
                {
                    if(s[i].Contains("#NoteStart"))
                    {
                        for(int j = i+1;j < s.Length; j++)
                        {
                            if (s[j].Contains("#NoteEnd")) 
                            {
                                string time = s[j].Split('(')[1].Trim().Trim(')');
                                //Debug.Log(time);
                                lastJudge = float.Parse(time);
                                break; 
                            }
                            else
                            {
                                string time = s[j].Split('{')[0].Trim().Trim('(').Trim(')');
                                if (s[j].Contains("#"))
                                {
                                    CommandData c = new CommandData();
                                    c.id = cid;
                                    cid++;

                                    string[] command = s[j].Trim().Split('#')[1].Split(':');
                                    
                                    c.time = float.Parse(time);
                                    if(command[0] == "BpmChange")
                                    {
                                        c.type = 0;
                                        //Debug.Log(command[1]);
                                        c.parameter = float.Parse(command[1].Trim().Trim('{').Trim('}'));
                                    }

                                    CD.Add(c);
                                }
                                else
                                { 
                                    string[] note = s[j].Split(')')[1].Trim().Trim('{').Trim('}').Split(';');
                                    //Debug.Log(s[j]);

                                    for(int Ns = 0;Ns < note.Length; Ns++)
                                    {
                                        NoteData n = new NoteData();
                                        n.id = nid;
                                        nid++;
                                        n.JudgeTime = float.Parse(time);
                                        string[] res = note[Ns].Trim().Trim('[').Trim(']').Split(',');
                                        //Debug.Log(res[0]);
                                        n.road = int.Parse(res[0]);
                                        n.type = int.Parse(res[1]);
                                        n.length = int.Parse(res[2]);
                                        n.Start = int.Parse(res[3]);
                                        n.Des = int.Parse(res[4]);

                                        if (n.type == 5) hCount++;

                                        ND.Add(n);
                                    }
                                }
                            }
                        }
                        /*Debug.Log("有" + nid + "個音符");
                        Debug.Log("有" + hCount + "個長壓");
                        foreach (NoteData n in ND) Debug.Log(n);*/
                    }else if (s[i].Contains("#HoldData"))
                    {
                        for (int j = i + 1; j < s.Length; j++)
                        {
                            //Debug.Log(s[j]);
                            if (s[j].Contains("#HoldDataEnd")) break;
                            else
                            {
                                HoldData h = new HoldData();
                                h.id = hid;
                                hid++;
                                string[] type = s[j].Split('{')[0].Trim().Trim('(').Trim(')').Split(',');
                                string[] point = s[j].Split(')')[1].Trim().Trim('{').Trim('}').Split(';');
                                if (int.TryParse(type[0], out int startType))
                                {
                                    //Debug.Log(startType);
                                    h.StartType = startType;
                                }
                                else
                                {
                                    throw new TrackInfoHasProblemException();
                                }
                                if (int.TryParse(type[1], out int EndType))
                                {
                                    //Debug.Log(EndType);
                                    h.EndType = EndType;
                                }
                                else
                                {
                                    throw new TrackInfoHasProblemException();
                                }

                                h.points = new float[point.Length][];
                                for(int k = 0; k < point.Length; k++)
                                {
                                    string[] hl = point[k].Trim().Trim('[').Trim(']').Split(',');
                                    h.points[k] = new float[5];
                                    for(int l = 0; l < 5; l++)
                                    {
                                        if (float.TryParse(hl[l], out float res))
                                        {
                                            h.points[k][l] = res;
                                        }
                                        else
                                        {
                                            throw new TrackInfoHasProblemException();
                                        }
                                    }
                                }
                                HD.Add(h);
                            }
                        }

                        if(HD.Count < hCount)
                        {
                            Debug.Log(hCount + "  " + HD.Count);
                            throw new TrackInfoHasProblemException();
                        }
                        /*else
                        {
                            foreach (HoldData n in HD) Debug.Log(n);
                        }*/
                    }
                }
            }
        }
    }

    public void ReadInfo()
    {
        cover = Resources.Load("Cover", typeof(Sprite)) as Sprite;

        TextAsset Info = Resources.Load("Information") as TextAsset;

        if (Info == null) throw new TrackDataNotFoundException();
        else
        {
            string[] s = Info.text.Split("\n"[0]);
            InfoData = new InformationData();

            foreach (string s1 in s)
            {
                string[] s2 = s1.Split(':');
                if (s2[0] == "TrackName") InfoData.TrackName = s2[1];
                else if (s2[0] == "Artist") InfoData.Artist = s2[1];
                else if (s2[0] == "Style") InfoData.Style = s2[1];
                else if (s2[0] == "Bpm")
                {
                    if (float.TryParse(s2[1], out float bpm))
                    {
                        InfoData.bpm = bpm;
                    }
                    else
                    {
                        throw new TrackInfoHasProblemException();
                    }
                }
                else if (s2[0] == "Tempo")
                {
                    if (int.TryParse(s2[1], out int Tempo))
                    {
                        InfoData.Tempo = Tempo;
                    }
                    else
                    {
                        throw new TrackInfoHasProblemException();
                    }
                }
                else if (s2[0] == "Offset")
                {
                    if (float.TryParse(s2[1], out float Offset))
                    {
                        InfoData.Offset = Offset;
                    }
                    else
                    {
                        throw new TrackInfoHasProblemException();
                    }
                }
                else if (s2[0] == "Level")
                {
                    string[] Levellist = s2[1].Split(',');
                    InfoData.Level = new int[Levellist.Length];

                    for (int i = 0; i < Levellist.Length; i++)
                    {
                        if (int.TryParse(Levellist[i], out int Level))
                        {
                            InfoData.Level[i] = Level;
                            
                        }
                        else
                        {
                            
                            throw new TrackInfoHasProblemException();
                        }
                    }
                }
            }
        }
    }

    public AudioClip GetClip()
    {
        return clip;
    }

    public Sprite GetCover()
    {
        return cover;
    }

    public InformationData GetInfo()
    {
        return InfoData;
    }

    public List<NoteData> GetND()
    {
        return ND;
    }

    public List<HoldData> GetHD()
    {
        return HD;
    }

    public List<CommandData> GetCD()
    {
        return CD;
    }

    public float getJastJudge()
    {
        return lastJudge;
    }
}

