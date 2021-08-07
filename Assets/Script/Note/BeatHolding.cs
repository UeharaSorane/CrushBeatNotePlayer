using System.Collections.Generic;
using TrackDataClass;
using UnityEngine;

public class BeatHolding : MonoBehaviour
{
    GameObject Manager,JudgeLine,SendNote,Pad,Score;
    float songpos;
    GameObject[] NoteObj = new GameObject[5];
    int Id;

    List<RenderPoint> RenderList;
    int Rid;

    List<HoldPointInformation> PointList;
    int StartType, EndType;

    bool isStart, isFinal, holdStart;

    float HoldTime, HoldStartTime, nowHoldTime, TotalHold;
    int holdPoint;

    bool Ready = false;

    public void setHold(int id, GameObject[] no, List<HoldPointInformation> pl, int st, int et,float ht)
    {
        Manager = GameObject.Find("Manager");
        SendNote = GameObject.Find("SendNote");
        JudgeLine = GameObject.Find("JudgeLine");
        Pad = GameObject.Find("Pad");
        Score = GameObject.Find("Score");

        Id = id;
        NoteObj = no;
        PointList = pl;
        StartType = st;
        EndType = et;
        HoldTime = ht;

        holdPoint = 0;
        Ready = true;
    }

    private void Update()
    {
        songpos = Manager.GetComponent<Conductor>().getPos();
        if (Ready)
        {
            createNote();
            setRender();
            Holding();
        }
    }

    Vector3 setPos(Vector3 StartPos, Vector3 DesPos, float StartTime, float JudgeTime)
    {
        Vector3 pos;
        pos.x = getLerpPoint(StartPos.x, DesPos.x, StartTime, JudgeTime);
        pos.z = getLerpPoint(StartPos.z, DesPos.z, StartTime, JudgeTime);

        if (pos.z >= -10 && pos.z <= 40) pos.y = -10.04f + Mathf.Sqrt(156.25f - Mathf.Pow(15 - pos.z, 2) / 4);
        else pos.y = getLerpPoint(StartPos.y, DesPos.y, StartTime, JudgeTime);

        //pos.y = getLerpPoint(StartPos.y, DesPos.y, StartTime, JudgeTime);


        return pos;
    }

    float getLerpPoint(float s, float d, float StartTime, float JudgeTime)
    {
        return s + (d - s) * (songpos - StartTime) / (JudgeTime - StartTime);
    }

    void setRender()
    {
        Vector3[] PointPos = new Vector3[PointList.Count];
        float[] PointLength = new float[PointList.Count];

        for (int i = 0; i < PointList.Count; i++)
        {
            if(songpos >= PointList[i].StartTime && songpos < PointList[i].JudgeTime)
            {
                Vector3 sp = SendNote.transform.GetChild(0).position;
                sp.x = -9 + PointList[i].Start / 2;
                Vector3 dp = JudgeLine.transform.position;
                dp.x = -9 + PointList[i].Des / 2;

                PointLength[i] = PointList[i].length;

                PointPos[i] = setPos(sp, dp, PointList[i].StartTime, PointList[i].JudgeTime);
                //Debug.Log(PointPos[i]);
            }
            else if(songpos < PointList[i].StartTime)
            {
                PointPos[i] = SendNote.transform.GetChild(0).position;
                PointPos[i].x = -9 + PointList[i].Start / 2;
                PointLength[i] = PointList[i].length;
            }
            else if (songpos >= PointList[i].JudgeTime && i<PointList.Count - 1)
            {
                if(songpos >= PointList[i + 1].JudgeTime)
                {
                    PointPos[i] = JudgeLine.transform.position;
                    //PointPos[i].x = PointPos[i + 1].x;
                    PointList[i].length = PointList[i + 1].length;
                    PointLength[i] = PointList[i + 1].length;
                }
                else
                {
                    PointPos[i] = JudgeLine.transform.position;
                    PointPos[i].x = getLerpPoint(-9 + PointList[i].Des / 2, -9 + PointList[i + 1].Des / 2, PointList[i].JudgeTime, PointList[i + 1].JudgeTime);
                    //Debug.Log(PointPos[i].x);
                    PointLength[i] = getLerpPoint(PointList[i].length, PointList[i + 1].length, PointList[i].JudgeTime, PointList[i + 1].JudgeTime);
                }
            }
        }

        RenderList = new List<RenderPoint>();
        float dz = 0.1f;
        Rid = 0;

        for (int i = 0; i < PointList.Count - 1; i++)
        {
            float distanceZ = PointPos[i + 1].z - PointPos[i].z;
            
            int RenderPointCount = Mathf.FloorToInt(distanceZ / dz);

            for(int j = 0; j < RenderPointCount; j++)
            {
                RenderPoint p = new RenderPoint();
                p.id = Rid;
                Rid++;
                Vector3 pos = new Vector3();
                pos.z = PointPos[i].z + j * dz;
                pos.x = PointPos[i].x + (PointPos[i + 1].x - PointPos[i].x) *j /RenderPointCount;

                //Debug.Log(i + "  " + PointPos[i].x);
                pos.y = -10.04f + Mathf.Sqrt(156.25f - Mathf.Pow(15 - pos.z, 2) / 4);

                p.pos = pos;
                p.length = PointLength[i] + (PointLength[i + 1] - PointLength[i]) *j / RenderPointCount;

                RenderList.Add(p);
            }
        }

        

        if (RenderList.Count >= 2)
        {
            //Debug.Log(RenderList[0].pos.x);
            Mesh HoldMesh = new Mesh();
            GetComponent<MeshFilter>().mesh = HoldMesh;

            Vector3[] vertices = new Vector3[RenderList.Count * 2];
            int[] triangles = new int[(RenderList.Count - 1) * 6];
            Vector2[] uv = new Vector2[RenderList.Count * 2];

            for (int i = 0; i < RenderList.Count; i++)
            {
                Vector3 pos = RenderList[i].pos;
                float Length = RenderList[i].length;

                pos.x = pos.x - Length / 2;
                vertices[i * 2] = pos;
                uv[i * 2] = new Vector2(0, (float)i/(float)RenderList.Count);

                pos.x = pos.x + Length;
                vertices[i * 2 + 1] = pos;
                uv[i*2 +1] = new Vector2(1, (float)i / (float)RenderList.Count);
            }

            for (int i = 1; i < RenderList.Count; i++)
            {
                triangles[(i - 1) * 6] = 2 * i;
                triangles[(i - 1) * 6 + 1] = 2 * (i - 1) + 1;
                triangles[(i - 1) * 6 + 2] = 2 * (i - 1);

                triangles[(i - 1) * 6 + 3] = 2 * i;
                triangles[(i - 1) * 6 + 4] = 2 * i + 1;
                triangles[(i - 1) * 6 + 5] = 2 * (i - 1) + 1;
            }

            HoldMesh.Clear();
            HoldMesh.vertices = vertices;
            HoldMesh.triangles = triangles;
            HoldMesh.uv = uv;
            HoldMesh.RecalculateNormals();
        }
    }

    void createNote()
    {
        if (songpos >= PointList[0].StartTime && !isStart)
        {
            //Debug.Log(songpos);
            Vector3 StartPos = SendNote.transform.GetChild(0).position;
            StartPos.x = -9 + PointList[0].Start / 2;
            Vector3 EndPos = JudgeLine.transform.position;
            EndPos.x = -9 + PointList[0].Des / 2;

            GameObject StartNote = Instantiate(NoteObj[StartType], StartPos, NoteObj[StartType].transform.rotation) as GameObject;
            Vector3 l = NoteObj[StartType].transform.localScale;
            l.x = PointList[0].length;
            StartNote.transform.localScale = l;

            StartNote.GetComponent<BeatAct>().setNote(Id,StartType, StartPos, EndPos, PointList[0].JudgeTime, PointList[0].StartTime,false);
            isStart = true;
        }
        else if (songpos >= PointList[PointList.Count - 1].StartTime && !isFinal)
        {
            //Debug.Log(PointList[PointList.Count - 1].StartTime);
            Vector3 StartPos = SendNote.transform.GetChild(0).position;
            StartPos.x = -9 + PointList[PointList.Count - 1].Start / 2;
            Vector3 EndPos = JudgeLine.transform.position;
            EndPos.x = -9 + PointList[PointList.Count - 1].Des / 2;

            GameObject EndNote = Instantiate(NoteObj[EndType], EndPos, NoteObj[EndType].transform.rotation) as GameObject;
            Vector3 l = NoteObj[EndType].transform.localScale;
            l.x = PointList[PointList.Count - 1].length;
            EndNote.transform.localScale = l;

            EndNote.GetComponent<BeatAct>().setNote(Id + 1, EndType, StartPos, EndPos, PointList[PointList.Count - 1].JudgeTime, PointList[PointList.Count - 1].StartTime, true);

            isFinal = true;
        }
    }

    void Holding()
    {
        if (holdStart)
        {

            //Debug.Log("Now: " + holdPoint + " Next: " + (holdPoint + 1));
            float holdPos = getLerpPoint(-9 + PointList[holdPoint].Des / 2, -9 + PointList[holdPoint + 1].Des / 2, PointList[holdPoint].JudgeTime, PointList[holdPoint + 1].JudgeTime);
            float holdLength = getLerpPoint(PointList[holdPoint].length, PointList[holdPoint + 1].length, PointList[holdPoint].JudgeTime, PointList[holdPoint + 1].JudgeTime);
            //Debug.Log("Pos: " + holdPos + " Length: " + holdLength);

            int judgeL = Mathf.CeilToInt(holdPos - holdLength / 2);
            int judgeR = Mathf.CeilToInt(holdPos + holdLength / 2) - 1;
            //Debug.Log("R: " + judgeR + "L" + judgeL);


            if (judgeR > 8) judgeR = 8;
            else if (judgeR < -9) judgeR = -9;

            if (judgeL < -9) judgeL = -9;
            else if (judgeL > 8) judgeL = 8;

            int[] judgePad = new int[judgeR - judgeL + 1];
            for (int i = 0; i < judgePad.Length; i++)
            {
                judgePad[i] = 9 + judgeL + i;
                //Debug.Log(judgePad[i]);
            }

            if (holdPoint < PointList.Count - 1 && songpos >= PointList[holdPoint + 1].JudgeTime) holdPoint++;
            bool hold = false;
            for (int i = 0; i < judgePad.Length; i++)
            {

                if (Pad.transform.GetChild(judgePad[i]).GetComponent<PadAct>().status != 0)
                {
                    hold = true;
                    break;
                }
            }

            if (hold)
            {
                nowHoldTime = songpos - HoldStartTime;
            }
            else
            {
                TotalHold += nowHoldTime;
                nowHoldTime = 0;
                HoldStartTime = songpos;
            }

            if (songpos >= PointList[PointList.Count - 1].JudgeTime)
            {
                TotalHold += nowHoldTime;

                float TotalBonus = TotalHold / (HoldTime * 0.8f);
                if (TotalBonus > 1) TotalBonus = 1;
                Score.GetComponent<Score>().HoldScore(TotalBonus);
                //Debug.Log("ªøÀ£¼úÀy:" + (Mathf.FloorToInt(TotalBonus * 100)) + "%");
                Destroy(this.gameObject);
            }
        }
        else if(songpos >= PointList[0].JudgeTime)
        {
            holdStart = true;
            HoldStartTime = PointList[0].JudgeTime;
            TotalHold = 0;
        }
    }
    class RenderPoint
    {
        public int id { get; set; }
        public int nowPoint { get; set; }
        public Vector3 pos { get; set; }
        public float length { get; set; }
    }
}
