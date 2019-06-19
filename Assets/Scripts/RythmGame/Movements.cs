using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements
{
    public List<TimeStamp> RLRLRL(float t)
    {
        List<TimeStamp> list = new List<TimeStamp>();
        list.Add(new TimeStamp(t + 0, 0, 1, 3, new Vector3(6,6,0)));
        list.Add(new TimeStamp(t + 1, 0, 2, 3, new Vector3(-6,6,0)));
        list.Add(new TimeStamp(t + 2, 0, 1, 3, new Vector3(6,0,0)));
        list.Add(new TimeStamp(t + 3, 0, 2, 3, new Vector3(-6,0,0)));
        list.Add(new TimeStamp(t + 4, 0, 1, 3, new Vector3(6,-6,0)));
        list.Add(new TimeStamp(t + 5, 0, 2, 3, new Vector3(-6,-6,0)));
        return list;
    }

    public List<TimeStamp> RLRLRL2(float t)
    {
        List<TimeStamp> list = new List<TimeStamp>();
        list.Add(new TimeStamp(t + 0, 2, 1, 3, 0, new Vector3(6,6,0)));
        list.Add(new TimeStamp(t + 1, 2, 2, 3, 180,new Vector3(-6,6,0)));
        list.Add(new TimeStamp(t + 2, 2, 1, 3, 0, new Vector3(6,0,0)));
        list.Add(new TimeStamp(t + 3, 2, 2, 3, 180,new Vector3(-6,0,0)));
        list.Add(new TimeStamp(t + 4, 2, 1, 3, 0, new Vector3(6,-6,0)));
        list.Add(new TimeStamp(t + 5, 2, 2, 3, 180,new Vector3(-6,-6,0)));
        return list;
    }

}
