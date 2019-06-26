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

    public List<TimeStamp> GetUkiDatas(string path, float timeSpawn, int jump, float speed, float scale, float offsetX, float offsetY, TimeStamp DefaultNode)
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
             
         
        int length = listRHx.Count;
        for (int i = 1; i <= length/(jump+1); i++){
            for (int j = 0; j < jump; j++){
                listRHx.Remove(listRHx[i]);
                listRHy.Remove(listRHy[i]);
                listLHx.Remove(listLHx[i]);
                listLHy.Remove(listLHy[i]);
            }
        }
        #endregion
        
        #region Add to list
        List<TimeStamp> listTS = new List<TimeStamp>();
        float hipCenterX = 0;
        float hipCenterY = 0;
        float.TryParse(HCY.Replace(".",","), out hipCenterY);
        float.TryParse(HCX.Replace(".",","), out hipCenterX);

        switch(DefaultNode.nodeType){
            case 1 :
                InitAddLineNode(listTS, 1, timeSpawn, scale, listRHx, listRHy, hipCenterX, hipCenterY, offsetX, offsetY, DefaultNode);
                InitAddLineNode(listTS, 2, timeSpawn, scale, listLHx, listLHy, hipCenterX, hipCenterY, offsetX, offsetY, DefaultNode);
                break;
            default : 
                InitAddNode(listTS, 1, timeSpawn, speed, scale, listRHx, listRHy, hipCenterX, hipCenterY, offsetX, offsetY, DefaultNode);
                InitAddNode(listTS, 2, timeSpawn, speed, scale, listLHx, listLHy, hipCenterX, hipCenterY, offsetX, offsetY, DefaultNode);
                break;
        }
        return listTS;
        #endregion
    }

    //Add any type of node except LineNodes
    void InitAddNode(List<TimeStamp> listTS, int joint, float timeSpawn, float speed, float scale, List<string> PosX, List<string> PosY, float hipCenterX, float hipCenterY, float offsetX, float offsetY, TimeStamp DefaultNode){
        float x = 0;
        float y = 0;
        for(int i =0; i < PosX.Count; i++){
            float.TryParse(PosX[i].Replace(".",","), out x);
            float.TryParse(PosY[i].Replace(".",","), out y); 
            x -= hipCenterX;
            y -= hipCenterY;
            TimeStamp ts = DefaultNode.DeepCopyTS(DefaultNode);
            ts.joint = joint;
            ts.timeSpawn = i/speed + timeSpawn;
            ts.spawnPosition = new Vector3(x*scale + offsetX, y*scale + offsetY,0);
            listTS.Add(ts);
        }
    }

    //Add a LineNode
    void InitAddLineNode(List<TimeStamp> listTS, int joint, float timeSpawn, float scale, List<string> PosX, List<string> PosY, float hipCenterX, float hipCenterY, float offsetX, float offsetY, TimeStamp DefaultNode){
        TimeStamp ts = DefaultNode.DeepCopyTS(DefaultNode);
        ts.joint = joint;
        Vector3[] pathPositions = new Vector3[PosX.Count-1];
        float x = 0;
        float y = 0;
        for(int i = 0; i < PosX.Count; i++){
            float.TryParse(PosX[i].Replace(".",","), out x);
            float.TryParse(PosY[i].Replace(".",","), out y);
            x -= hipCenterX;
            y -= hipCenterY;
            if (i == 0){
                ts.spawnPosition = new Vector3(x*scale + offsetX,y*scale + offsetY,0);
            } else pathPositions[i-1] = new Vector3(x*scale + offsetX,y*scale + offsetY,0);
        }
        ts.pathPositions = pathPositions;
        ts.timeSpawn = timeSpawn;
        listTS.Add(ts);
    }
    

}
