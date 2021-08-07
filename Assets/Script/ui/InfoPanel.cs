using MyException;
using System.Collections;
using System.Collections.Generic;
using TrackDataClass;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    InformationData info;
    int difficulty;
    Sprite Cover;
    public void setInfo(InformationData i, Sprite c, int d)
    {
        info = i;
        difficulty = d;
        Cover = c;
        transform.GetChild(0).GetComponent<Text>().text = info.TrackName;
        transform.GetChild(1).GetComponent<Text>().text = info.Artist;

        switch (difficulty)
        {
            case 0:
                transform.GetChild(2).GetComponent<Text>().text = "<color=#00DB00>Dot </color>";
                break;
            case 1:
                transform.GetChild(2).GetComponent<Text>().text = "<color=#FFD306>Line </color>";
                break;
            case 2:
                transform.GetChild(2).GetComponent<Text>().text = "<color=#FF0000>Surface </color>";
                break;
            case 3:
                transform.GetChild(2).GetComponent<Text>().text = "<color=#E800E8>Dimension </color>";
                break;
            default:
                transform.GetChild(2).GetComponent<Text>().text = "Special ";
                break;
        }
        transform.GetChild(2).GetComponent<Text>().text += info.Level[difficulty];
        transform.GetChild(3).GetComponent<Image>().sprite = Cover;
    }

    public void setProgess(float p)
    {
        transform.GetChild(4).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(p * 280f, transform.GetChild(4).GetChild(0).GetComponent<RectTransform>().anchoredPosition.y);
    }

    public InformationData getInfo()
    {
        return info;
    }

    public int getDifficulty()
    {
        return difficulty;
    }

    public Sprite getCover()
    {
        return Cover;
    }

    public void show()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }
    public void DestoryThis()
    {
        Destroy(this.gameObject);
    }
}
