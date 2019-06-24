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

    public List<TimeStamp> GetUkiDatas(string path, int jump, float scale, float offsetX, float offsetY, TimeStamp DefaultNode)
    {
        #region ReadCSV
        //Get values from CSV file
        StreamReader reader = new StreamReader(File.OpenRead(path));
        List<string> listRHx = new List<string>();
        List<string> listRHy = new List<string>();
        List<string> listLHx = new List<string>();
        List<string> listLHy = new List<string>();
        bool centered = false; //Did I get the hipCenter value ?
        string HCX = "";
        string HCY = "";
        int cpt = 0;
        while (!reader.EndOfStream)
            {
            string line = reader.ReadLine();
            if (!string.IsNullOrWhiteSpace(line) & cpt > 0) //cpt to remove colomn title
                {
                string[] values = line.Split(',');
                listRHx.Add(values[29]);
                listRHy.Add(values[30]);
                listLHx.Add(values[26]);
                listLHy.Add(values[27]);
                if (!centered && cpt == 1){
                    centered = true;
                    HCX = values[35];
                    HCY = values[36]; 
                }
                }
                cpt++;
            }
        string[] arrayRHx = listRHx.ToArray();
        string[] arrayRHy = listRHy.ToArray();
        string[] arrayLHx = listLHx.ToArray();
        string[] arrayLHy = listLHy.ToArray();
        listRHx.Clear();
        listRHy.Clear();
        listLHx.Clear();
        listLHy.Clear();

        //Start from the end to get the last point right
        for(int i = arrayRHx.Length-1; i >= 0; i -= jump){
            listRHx.Add(arrayRHx[i]);
            listRHy.Add(arrayRHy[i]);
            listLHx.Add(arrayLHx[i]);
            listLHy.Add(arrayLHy[i]);
        }
        listRHx.Reverse();
        listRHy.Reverse();
        listLHx.Reverse();
        listLHy.Reverse();
        #endregion
        
        
        List<TimeStamp> listTS = new List<TimeStamp>();
        float x;
        float y;
        float hipCenterX = 0;
        float hipCenterY = 0;
        TimeStamp ts;

        /* 
        ts = DefaultNode.DeepCopyTS(DefaultNode);
        ts.joint = 1;
        Vector3[] pathPositions = new Vector3[listRHx.Count-1];
        for(int i = 0; i < listRHx.Count; i++){
            float.TryParse(listRHx[i].Replace(".",","), out x);
            float.TryParse(listRHy[i].Replace(".",","), out y);
            x -= hipCenterX;
            y -= hipCenterY;
            Debug.Log(x + ", " + y);
            if (i == 0){
                ts.spawnPosition = new Vector3(x*scale,y*scale,0);
            } else pathPositions[i-1] = new Vector3(x*scale,y*scale,0);
            
        }
        ts.pathPositions = pathPositions;
        listTS.Add(ts);*/


        
        for(int i =0; i < listRHx.Count; i++){
            if (float.TryParse(listRHx[i].Replace(".",","), out x) && float.TryParse(listRHy[i].Replace(".",","), out y) &&
                         float.TryParse(HCX.Replace(".",","), out hipCenterX) && float.TryParse(HCY.Replace(".",","), out hipCenterY) )
            {
                x -= hipCenterX;
                y -= hipCenterY;
                ts = DefaultNode.DeepCopyTS(DefaultNode);
                ts.joint = 1;
                ts.timeSpawn = i;
                ts.spawnPosition = new Vector3(x*scale + offsetX, y*scale + offsetY,0);

                listTS.Add(ts);
            } else Debug.Log("Invalid string value");

            if (float.TryParse(listLHx[i].Replace(".",","), out x) && float.TryParse(listLHy[i].Replace(".",","), out y) &&
                         float.TryParse(HCX.Replace(".",","), out hipCenterX) && float.TryParse(HCY.Replace(".",","), out hipCenterY) )
            {
                x -= hipCenterX;
                y -= hipCenterY;
                ts = DefaultNode.DeepCopyTS(DefaultNode);
                ts.timeSpawn = i;
                ts.joint = 2;
                ts.spawnPosition = new Vector3(x*scale + offsetX, y*scale + offsetY,0);
                listTS.Add(ts);
            } else Debug.Log("Invalid string value");   
        }
        return listTS;
    }
    

}
