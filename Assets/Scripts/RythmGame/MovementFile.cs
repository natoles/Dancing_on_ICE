using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFile
{
    public string path;
    public float[] jointsRates;

    public float globalRate;

    public MovementFile(string paths1, float RHrate1, float LHrate1 ){
        path = paths1;
        jointsRates = new float[2] {RHrate1, LHrate1};

    }
}
