using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    public int mode;
    public float offset,speed;
    public int getMode()
    {
        return mode;
    }

    public float getOffset()
    {
        return offset;
    }

    public float getSpeed()
    {
        return speed;
    }
}
