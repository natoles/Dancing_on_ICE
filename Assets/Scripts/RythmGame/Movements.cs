using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements : MonoBehaviour
{
    public List<TimeStamp> RLRLRL = new List<TimeStamp>();

    void Start(){
        RLRLRL.Add(new TimeStamp(0, 0, 1, 3, new Vector3(6,6,0)));
        RLRLRL.Add(new TimeStamp(1, 0, 2, 3, new Vector3(-6,6,0)));
        RLRLRL.Add(new TimeStamp(2, 0, 1, 3, new Vector3(6,0,0)));
        RLRLRL.Add(new TimeStamp(3, 0, 2, 3, new Vector3(-6,0,0)));
        RLRLRL.Add(new TimeStamp(4, 0, 1, 3, new Vector3(6,-6,0)));
        RLRLRL.Add(new TimeStamp(5, 0, 2, 3, new Vector3(-6,-6,0)));
    }

    


}
