using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushAct : MonoBehaviour
{
    GameObject Manager, Crusher, Grade;
    public GameObject SEObj;
    int Id, Type;

    Vector3 StartPos, DesPos;
    float judgetime, spawntime;
    float MaxJudge, MinJudge;
    bool isHold;
    bool isDel;
    bool Ready = false;

    float[] judgePoint = MyConstant.ConstSet.judgePoint;

    public void setNote(int id, int type, Vector3 sp, Vector3 dp, float jt, float st, bool ih)
    {
        Manager = GameObject.Find("Manager");
        Crusher = GameObject.Find("Crusher");
        Grade = GameObject.Find("Grade");
        GameObject se = Instantiate(SEObj);
        se.GetComponent<SoundEffect>().PlaySound("Hit", jt);

        Id = id;
        Type = type;
        StartPos = sp;
        DesPos = dp;
        judgetime = jt;
        spawntime = st;
        isHold = ih;

        MaxJudge = DesPos.x + transform.localScale.y;
        MinJudge = DesPos.x - transform.localScale.y;

        if (MaxJudge > 8) MaxJudge = 8;
        if (MinJudge < -9) MinJudge = -9;

        isDel = false;
        Ready = true;
    }
    void Update()
    {
        if (Ready)
        {
            setPos();
            CheckHit();
        }
    }

    void setPos()
    {
        Vector3 pos;
        pos.x = StartPos.x + (DesPos.x - StartPos.x) * (Manager.GetComponent<Conductor>().getPos() - spawntime) / (judgetime - spawntime);
        pos.y = StartPos.y + (DesPos.y - StartPos.y) * (Manager.GetComponent<Conductor>().getPos() - spawntime) / (judgetime - spawntime);
        pos.z = StartPos.z + (DesPos.z - StartPos.z) * (Manager.GetComponent<Conductor>().getPos() - spawntime) / (judgetime - spawntime);
        transform.position = pos;
    }

    void CheckHit()
    {
        if (!isDel)
        {
            float songpos = Manager.GetComponent<Conductor>().getPos();

            if(Crusher.GetComponent<CrusherAct>().nextNoteNo != Id)
            {
                if (Crusher.GetComponent<CrusherAct>().nextNoteNo == -1) Crusher.GetComponent<CrusherAct>().nextNoteNo = Id;
            }
            else
            {
                if(Crusher.transform.position.x - Crusher.transform.localScale.x/2 <= MaxJudge && Crusher.transform.position.x + Crusher.transform.localScale.x / 2 >= MinJudge)
                {
                    if (judgetime - songpos <= judgePoint[0] && judgetime - songpos >= -judgePoint[0])
                    {
                        bool notP = false;
                        if (Type == 0)
                        {
                            if (!isHold)
                            {
                                if (Crusher.GetComponent<CrusherAct>().status == 1)
                                {
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
                                if (Crusher.GetComponent<CrusherAct>().Touchend)
                                {
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
                            if (judgetime - songpos <= judgePoint[4])
                            {
                                for (int ji = 1; ji < judgePoint.Length; ji++)
                                {
                                    if (judgetime - songpos <= -judgePoint[ji])
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
                        else if (Type == 2)
                        {
                            if (Crusher.GetComponent<CrusherAct>().status == 3)
                            {
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
                            if (Crusher.GetComponent<CrusherAct>().status == 4)
                            {
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
                }
                if (judgetime - songpos < -0.2)
                {
                    //Debug.Log("miss");
                    Grade.GetComponent<ShowGrade>().Grade(5, songpos - judgetime);
                    isDel = true;

                }
            }

        }
        else
        {
            DelNote();
        }
    }
    void judge(float songpos, int grade)
    {
        Grade.GetComponent<ShowGrade>().Grade(grade, songpos - judgetime);
        isDel = true;
    }

    void DelNote()
    {
        Crusher.GetComponent<CrusherAct>().nextNoteNo = -1;
        Destroy(gameObject);
    }
}
