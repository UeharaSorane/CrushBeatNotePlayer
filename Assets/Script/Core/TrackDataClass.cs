using UnityEditor;
using UnityEngine;

namespace TrackDataClass
{
    public class InformationData
    {
        public string TrackName { get; set; }
        public string Artist { get; set; }
        public string Style { get; set; }
        public float bpm { get; set; }
        public int Tempo { get; set; }
        public float Offset { get; set; }
        public int[] Level { get; set; }
    }

    public class NoteData
    {
        public int id { get; set; }
        public int road { get; set; }
        public int type { get; set; }
        public int length { get; set; }
        public int Start { get; set; }
        public int Des { get; set; }
        public float JudgeTime { get; set; }//以節拍為單位


        public override string ToString()
        {
            string res = "ID:" + this.id + " 類型:";
            switch (this.road)
            {
                case 0:
                    res += "";
                    break;
                case 1:
                    res += "C_";
                    break;
            }
            switch (this.type)
            {
                case 0:
                    res += "Tap";
                    break;
                case 1:
                    res += "Touch";
                    break;
                case 2:
                    res += "LSlide";
                    break;
                case 3:
                    res += "RSlide";
                    break;
                case 4:
                    res += "USlide";
                    break;
                case 5:
                    res += "Hold";
                    break;
            }

            res += " 長度:" + this.length + " 起始:" + this.Start + " 終點:" + this.Des + " 判定時間:" + this.JudgeTime;

            return res;
        }
    }

    public class CommandData
    {
        public int id{ get; set; }
        public int type { get; set; }
        public float parameter { get; set; }
        /* 
         * 0=>BpmChange
         */
        public float time { get; set; }
    }

    public class HoldData
    {
        public int id { get; set; }
        public float[][] points{ get; set; }
        public int StartType { get; set; }
        public int EndType { get; set; }


        public override string ToString()
        {
            return "ID: " + this.id + " 中繼點: " + points.Length;
        }
    }

    public class HoldPointInformation
    {
        public int id { get; set; }
        public float length { get; set; }
        public float Start { get; set; }
        public float Des { get; set; }
        public float StartTime { get; set; }
        public float JudgeTime { get; set; }

        public HoldPointInformation()
        {
            this.id = 0;
            this.Start = 0;
            this.Des = 0;
            this.length = 0;
            this.StartTime = 0;
            this.JudgeTime = 0;
        }
        public HoldPointInformation(int id, float Start, float Des, float length, float StartTime, float JudgeTime)
        {
            this.id = id;
            this.Start = Start;
            this.Des = Des;
            this.length = length;
            this.StartTime = StartTime;
            this.JudgeTime = JudgeTime;
        }
    }
}