using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    readonly int PerfectScore = 1000000;

    int NoteScore,showScore,NoteCount, HoldCount;

    float TotalBonus;

    int NowScore, PpCount, Pcount, GrCount, GdCount, BdCount, MiCount;

    bool isFailed = false;

    public void setNoteScore(int nc,int hc)
    {
        NoteCount = nc;
        HoldCount = hc;
        NoteScore = Mathf.RoundToInt(PerfectScore / (NoteCount + HoldCount));

        PpCount = 0;
        Pcount = 0;
        GrCount = 0;
        GdCount = 0;
        BdCount = 0;
        MiCount = 0;
        TotalBonus = 0;
        NowScore = 0;
        showScore = 0;
    }
    void Update()
    {
        if (showScore / 10000 < NowScore / 10000)
        {
            showScore += 10000;
        }
        if (showScore / 1000 < NowScore / 1000)
        {
            showScore += 1000;
        }
        if (showScore/100 < NowScore/100)
        {
            showScore+=100;
        }
        if (showScore/10 < NowScore/10) 
        {
            showScore += 10;
        }
        if (showScore < NowScore)
        {
            showScore++;
        }
        
        if(showScore > NowScore)
        {
            showScore = NowScore;
            
        }
            GetComponent<Text>().text = string.Format("{0:0000000}",Convert.ToInt32(showScore));
    }

    public int checkPerfect()
    {
        if (NoteCount == (PpCount + Pcount) && (HoldCount == 0||TotalBonus/HoldCount == 1)) 
        {
            NowScore = PerfectScore;
            if (NoteCount == (PpCount))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 2;
        }
    }

    public void PlusScore(int grade)
    {
        if (!isFailed)
        {
            if (PpCount + Pcount + GrCount + GdCount + BdCount + MiCount == NoteCount - 1) NoteScore += PerfectScore - NoteScore * (NoteCount + HoldCount);
            switch (grade)
            {
                case 0:
                    NowScore += NoteScore;
                    PpCount++;
                    break;
                case 1:
                    NowScore += NoteScore;
                    Pcount++;
                    break;
                case 2:
                    NowScore += Mathf.RoundToInt(NoteScore * 0.8f);
                    GrCount++;
                    break;
                case 3:
                    NowScore += Mathf.RoundToInt(NoteScore * 0.4f);
                    GdCount++;
                    break;
                case 4:
                    BdCount++;
                    NowScore += 0;
                    break;
                case 5:
                    NowScore += 0;
                    MiCount++;
                    break;

            }
        }
    }

    public void HardFail()
    {
        isFailed = true;
        MiCount = NoteCount - (PpCount + Pcount + GrCount + GdCount + BdCount);
    }

    public void HoldScore(float Bonus)
    {
        NowScore += Mathf.RoundToInt(NoteScore * Bonus);
        TotalBonus += Bonus;
    }

    public int getHoldBonus()
    {
        int holdBonus = Mathf.RoundToInt(TotalBonus / (float)HoldCount * 100);
        if (holdBonus > 100) holdBonus = 100;
        return holdBonus;
    }

    public int[] getNoteGrade()
    {
        return new int[] { PpCount, Pcount, GrCount, GdCount, BdCount, MiCount };
    }

    public int getScore()
    {
        return NowScore;
    }
}
