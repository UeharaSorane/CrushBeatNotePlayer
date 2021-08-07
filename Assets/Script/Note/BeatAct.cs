using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatAct : MonoBehaviour
{
    GameObject Pad,SG,Manager;
    public GameObject SEObj;

    int Id, Type;
    Vector3 StartPos, DesPos;
    float judgetime, spawntime;
    int[] judgePad;
    bool isHold,isDel;

    bool Ready = false;

    float[] judgePoint = MyConstant.ConstSet.judgePoint;

    public void setNote(int id,int type,Vector3 sp,Vector3 dp,float jt,float st,bool ih)
    {
        Manager = GameObject.Find("Manager");
        Pad = GameObject.Find("Pad");
        SG = GameObject.Find("Grade");
        GameObject se = Instantiate(SEObj);
        se.GetComponent<SoundEffect>().PlaySound("Hit", jt);

        Id = id;
        Type = type;
        StartPos = sp;
        DesPos = dp;
        judgetime = jt;
        spawntime = st;
        isHold = ih;

        isDel = false;

        int judgeR = Mathf.CeilToInt(DesPos.x + (transform.localScale.x / 2)) - 1;
        int judgeL = Mathf.CeilToInt(DesPos.x - (transform.localScale.x / 2));

        if (judgeR > 8) judgeR = 8;
        else if (judgeR < -9) judgeR = -9;

        if (judgeL < -9) judgeL = -9;
        else if (judgeL > 8) judgeL = 8;

        judgePad = new int[judgeR - judgeL + 1];
        for (int i = 0; i < judgePad.Length; i++)
        {
            judgePad[i] = 9 + judgeL + i;
            //Debug.Log(judgePad[i]);
        }


        Ready = true;
    }

    void Update()
    {
        if (Ready)
        {
            setPos();
            checkHit();
        }
    }

    void setPos()
    {
        Vector3 pos;
        pos.x = StartPos.x + (DesPos.x - StartPos.x) * (Manager.GetComponent<Conductor>().getPos() - spawntime) / (judgetime - spawntime);
        pos.z = StartPos.z + (DesPos.z - StartPos.z) * (Manager.GetComponent<Conductor>().getPos() - spawntime) / (judgetime - spawntime);
        pos.y = getY(pos.z);

        transform.position = pos;
    }

    void checkHit()
    {
        if (!isDel)
        {
            float songpos = Manager.GetComponent<Conductor>().getPos();
            for (int i = 0; i < judgePad.Length; i++)
            {
                if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().nextNoteNo != Id)
                {
                    if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().nextNoteNo == -1) Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().nextNoteNo = Id;
                }
                else
                {
                    if (judgetime - songpos <= judgePoint[0] && judgetime - songpos >= -judgePoint[0])
                    {
                        if(Type == 0)
                        {
                            if (!isHold)
                            {
                                if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().status == 1)
                                {
                                    bool notP = false;
                                    for (int ji = 1; ji < judgePoint.Length; ji++)
                                    {
                                        if (judgetime - songpos >= judgePoint[ji] || judgetime - songpos <= -judgePoint[ji])
                                        {
                                            judge(songpos, 5 - ji);
                                            notP = true;
                                            break;
                                        }
                                    }
                                    if (!notP)
                                    {
                                        judge(songpos, 0);
                                    }

                                }
                            }
                            else
                            {
                                if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().Touchend)
                                {
                                    bool notP = false;
                                    for (int ji = 1; ji < judgePoint.Length; ji++)
                                    {
                                        if (judgetime - songpos >= judgePoint[ji] || judgetime - songpos <= -judgePoint[ji])
                                        {
                                            judge(songpos, 5 - ji);
                                            notP = true;
                                            break;
                                        }
                                    }
                                    if (!notP)
                                    {
                                        judge(songpos, 0);
                                    }
                                }
                            }
                        }
                        else if (Type == 1)
                        {
                            if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().status !=0  && judgetime - songpos <=judgePoint[4])
                            {
                                bool notP = false;
                                for (int ji = 1; ji < judgePoint.Length; ji++)
                                {
                                    if (judgetime - songpos <= -judgePoint[ji] )
                                    {
                                        judge(songpos, 5 - ji);
                                        notP = true;
                                        break;
                                    }
                                }
                                if (!notP)
                                {
                                    judge(songpos, 0);
                                }

                            }
                        }else if (Type == 2)
                        {
                            if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().status == 3)
                            {
                                bool notP = false;
                                for (int ji = 1; ji < judgePoint.Length; ji++)
                                {
                                    if (judgetime - songpos >= judgePoint[ji] || judgetime - songpos <= -judgePoint[ji])
                                    {
                                        judge(songpos, 5 - ji);
                                        notP = true;
                                        break;
                                    }
                                }
                                if (!notP)
                                {
                                    judge(songpos, 0);
                                }

                            }
                        }
                        else if (Type == 3)
                        {
                            if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().status == 4)
                            {
                                bool notP = false;
                                for (int ji = 1; ji < judgePoint.Length; ji++)
                                {
                                    if (judgetime - songpos >= judgePoint[ji] || judgetime - songpos <= -judgePoint[ji])
                                    {
                                        judge(songpos, 5 - ji);
                                        notP = true;
                                        break;
                                    }
                                }
                                if (!notP)
                                {
                                    judge(songpos, 0);
                                }

                            }
                        }
                        else if (Type == 4)
                        {
                            if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().status == 5)
                            {
                                bool notP = false;
                                for (int ji = 1; ji < judgePoint.Length; ji++)
                                {
                                    if (judgetime - songpos >= judgePoint[ji] || judgetime - songpos <= -judgePoint[ji])
                                    {
                                        judge(songpos, 5 - ji);
                                        notP = true;
                                        break;
                                    }
                                }
                                if (!notP)
                                {
                                    judge(songpos, 0);
                                }

                            }
                        }
                    }
                    else if (judgetime - songpos < -0.2)
                    {
                        SG.GetComponent<ShowGrade>().Grade(5, songpos - judgetime);
                        isDel = true;
                        break;
                    }
                }
            }
        }
        else
        {
            DelNote();
        }
    }

    void judge(float songpos,int grade)
    {
        SG.GetComponent<ShowGrade>().Grade(grade, songpos - judgetime);
        //Debug.Log(id);
        isDel = true;
    }
    void DelNote()
    {
        Pad.GetComponent<PadControl>().ResetPad(Id);
        Destroy(gameObject);
    }

    float getY(float z)
    {
        float songpos = Manager.GetComponent<Conductor>().getPos();
        //Debug.Log(Mathf.Sqrt(506.25f - Mathf.Pow(30 - z, 2)/4));

        if(z >= -10 && z <= 40) return -10.04f + Mathf.Sqrt(156.25f - Mathf.Pow(15 - z, 2) / 4);
        else return transform.position.y + (DesPos.y - StartPos.y) * (songpos - spawntime) / (judgetime - spawntime);

        //return StartPos.y + (DesPos.y - StartPos.y) * (songpos - spawntime) / (judgetime - spawntime);
    }

}
