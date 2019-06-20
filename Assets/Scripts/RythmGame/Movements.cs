using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Movements
{

    //A function represents an available movement
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

    public List<TimeStamp> GetUkiDatas(string path, int jump)
    {
        #region ReadCsv
        //Get values from CSV file
        StreamReader reader = new StreamReader(File.OpenRead(path));
        List<string> listRHx = new List<string>();
        List<string> listRHy = new List<string>();
        while (!reader.EndOfStream)
            {
            string line = reader.ReadLine();
            if (!string.IsNullOrWhiteSpace(line))
                {
                string[] values = line.Split(',');
                if (values.Length >= 4)
                    {
                    listRHx.Add(values[29]);
                    listRHy.Add(values[30]);
                    }
                }
            }
        string[] arrayRHx = listRHx.ToArray();
        string[] arrayRHy = listRHy.ToArray();
        listRHx.Clear();
        listRHy.Clear();

        //Start from the end to get the last point right
        for(int i = arrayRHx.Length-1; i >= 0; i -= jump){
            listRHx.Add(arrayRHx[i]);
            listRHy.Add(arrayRHy[i]);
        }
        listRHx.Reverse();
        listRHy.Reverse();
        #endregion
        
        List<TimeStamp> listTS = new List<TimeStamp>();
        float x;
        float y;
        for(int i =0; i < listRHx.Count; i++){
            if (float.TryParse(listRHx[i].Replace(".",","), out x) && float.TryParse(listRHy[i].Replace(".",","), out y))
            {
                listTS.Add(new TimeStamp(i/5f, 0, 1, 0.5f, new Vector3(x*10,y*10,0)));
            } else Debug.Log("Invalid string value");
        }
        return listTS;
    }
    

}
