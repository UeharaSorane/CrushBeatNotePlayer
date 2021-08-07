using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tension : MonoBehaviour
{
    int mode;
    float gauge;
    int nowgauge;
    float increase,damage;
    bool isReady = false;
    bool isFailed = false;

    GameObject damageBar, gaugeBar,Pause,Manager;

    private void Update()
    {
        if(isReady)setBar();
    }

    public void setTension(int m,int nc)
    {
        damageBar = transform.GetChild(0).GetChild(0).gameObject;
        gaugeBar = transform.GetChild(0).GetChild(1).gameObject;

        mode = m;
        if (mode == 0)
        {
            gauge = 30;
            gaugeBar.GetComponent<Image>().color = new Color(1, 1, 0);

            if (nc < 200)
            {
                increase = 40 / nc + 0.2f;
            }
            else if (nc >= 200 && nc < 600)
            {
                increase = 30 / nc + 0.05f;
            }
            else if (nc > 600)
            {
                increase = 16 / nc + 0.04f;
            }

            damage = 4;
        }

        gaugeBar.GetComponent<RectTransform>().sizeDelta = new Vector2(gauge, gaugeBar.GetComponent<RectTransform>().sizeDelta.y);
        damageBar.GetComponent<RectTransform>().sizeDelta = new Vector2(gauge, damageBar.GetComponent<RectTransform>().sizeDelta.y);

        isReady = true;
    }

    void setBar()
    {
        int gaugeInt = Mathf.RoundToInt(gauge);
        GetComponent<Text>().text = gaugeInt + "%";

        if (gaugeInt * 3 != nowgauge)
        {
            if (gaugeInt * 3 > nowgauge)
            {
                nowgauge = gaugeInt * 3;
                gaugeBar.GetComponent<RectTransform>().sizeDelta = new Vector2(nowgauge, gaugeBar.GetComponent<RectTransform>().sizeDelta.y);
                damageBar.GetComponent<RectTransform>().sizeDelta = new Vector2(nowgauge, damageBar.GetComponent<RectTransform>().sizeDelta.y);
            }
            else if (gaugeInt * 3 < nowgauge)
            {
                nowgauge = gaugeInt * 3;
                gaugeBar.GetComponent<RectTransform>().sizeDelta = new Vector2(nowgauge, gaugeBar.GetComponent<RectTransform>().sizeDelta.y);
            }
        }

        if (damageBar.GetComponent<RectTransform>().sizeDelta != gaugeBar.GetComponent<RectTransform>().sizeDelta)
            damageBar.GetComponent<RectTransform>().sizeDelta += new Vector2(-1, 0) * Time.deltaTime * 20;
        
        if (mode == 0)
        {
            if (nowgauge >= 210)
                gaugeBar.GetComponent<Image>().color = new Color(0, 1, 0);
            else
                gaugeBar.GetComponent<Image>().color = new Color(1, 1, 0);
        }
    }

    public void plusTension(int grade)
    {
        if (!isFailed)
        {
            switch (grade)
            {
                case 0:
                case 1:
                    gauge += increase * 2;
                    break;
                case 2:
                    gauge += increase;
                    break;
                case 3:
                    gauge += 0;
                    break;
                case 4:
                case 5:
                    gauge -= damage;
                    break;
            }
        }
        if (gauge > 100) gauge = 100;
        if(gauge < 0)
        {
            if (mode == 0)
            {
                gauge = 0;
            }
        }
    }

    public bool checkClear()
    {
        if(mode == 0)
        {
            if (gauge >= 70) return true;
            return false;
        }else if(mode == 1)
        {
            if (isFailed)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public int getMode()
    {
        return mode;
    }
    public int getTension()
    {
        return Mathf.RoundToInt(gauge);
    }
}
