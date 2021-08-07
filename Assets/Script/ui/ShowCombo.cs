using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCombo : MonoBehaviour
{
    int combo,NoteCount;

    int MaxCombo;

    public void setCombo(int nc)
    {
        NoteCount = nc;
        GetComponent<Text>().text = "";
        MaxCombo = 0;
        combo = 0;
    }

    public void plusCombo()
    {
        combo++;

        if(combo >= MaxCombo)
        {
            MaxCombo = combo;
        }

        if(combo >= 3)
        {
            GetComponent<Text>().text =combo +  "Combo";
        }
    }

    public void breakCombo()
    {
        combo = 0;
        GetComponent<Text>().text = "";
    }

    public bool CheckFC()
    {
        if (MaxCombo == NoteCount)return true;
        else return false;
    }

    public int getMaxCombo()
    {
        return MaxCombo;
    }
}
