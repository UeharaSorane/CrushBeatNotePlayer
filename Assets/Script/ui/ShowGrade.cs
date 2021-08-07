using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGrade : MonoBehaviour
{
    GameObject SC,Score,Tension;
    Color TextC;
    float shine =0;
    void Start()
    {
        SC = GameObject.Find("Combo");
        Score = GameObject.Find("Score");
        Tension = GameObject.Find("Tension");

        GetComponent<CanvasGroup>().alpha = 0;

    }

    // Update is called once per frame
    public void Grade(int grade,float timerange)
    {
        Score.GetComponent<Score>().PlusScore(grade);
        //Debug.Log(grade);
        Tension.GetComponent<Tension>().plusTension(grade);
        switch (grade)
        {
            case 0:
                TextC.r = 1;
                TextC.g = 0.62745f;
                TextC.b = 0.258823f;
                TextC.a = 1;
                GetComponent<Text>().color = TextC;
                GetComponent<Text>().text = "Perfect+!!!";
                SC.GetComponent<ShowCombo>().plusCombo();
                break;
            case 1:
                TextC.r = 1;
                TextC.g = 0.62745f;
                TextC.b = 0.258823f;
                TextC.a = 1;
                GetComponent<Text>().color = TextC;

                if (timerange > 0) GetComponent<Text>().text =("Perfect!!<color=#FF0000>+" + (timerange) + "</color>");
                else GetComponent<Text>().text = ("Perfect!!<color=#0000FF>" + (timerange) + "</color>");
                SC.GetComponent<ShowCombo>().plusCombo();
                break;
            case 2:
                TextC.r = 1;
                TextC.g = 0;
                TextC.b = 0;
                TextC.a = 1;
                GetComponent<Text>().color = TextC;

                if (timerange > 0) GetComponent<Text>().text = ("Great!<color=#FF0000>+" + (timerange) + "</color>");
                else GetComponent<Text>().text = ("Great!(" + (timerange) + ")");
                SC.GetComponent<ShowCombo>().plusCombo();
                break;
            case 3:
                TextC.r = 0.749019f;
                TextC.g = 0;
                TextC.b = 0.37647f;
                TextC.a = 1;
                GetComponent<Text>().color = TextC;

                if (timerange > 0) GetComponent<Text>().text = ("Good<color=#FF0000>+" + (timerange) + "</color>");
                else GetComponent<Text>().text = ("Good<color=#0000FF>" + (timerange) + "</color>");
                SC.GetComponent<ShowCombo>().plusCombo();
                break;
            case 4:
                TextC.r = 0.682352f;
                TextC.g = 0;
                TextC.b = 0.682352f;
                TextC.a = 1;
                GetComponent<Text>().color = TextC;

                if (timerange > 0) GetComponent<Text>().text = ("Bad...<color=#FF0000>+" + (timerange) + "</color>");
                else GetComponent<Text>().text = ("Bad...<color=#F0000>" + (timerange) + "</color>");
                SC.GetComponent<ShowCombo>().breakCombo();
                break;
            default:
                TextC.r = 0.458823f;
                TextC.g = 0.458823f;
                TextC.b = 0.458823f;
                TextC.a = 1;
                GetComponent<Text>().color = TextC;

                GetComponent<Text>().text = "miss...";
                SC.GetComponent<ShowCombo>().breakCombo();
                break;
        }
        GetComponent<CanvasGroup>().alpha = 1;
        shine = 150;
    }


    void Update()
    {
        if (GetComponent<CanvasGroup>().alpha > 0 && shine == 0)
        {
            GetComponent<CanvasGroup>().alpha -= 0.005f;
        }
        else if(shine > 0)
        {
            shine--;
        }
    }

}

