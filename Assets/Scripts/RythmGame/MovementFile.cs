using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MovementFile
{

    //TODO : an array for jointExclusion with 1's and 0's
    
    int nbJoints = 2;
    float hipCenterX = 0;
    float hipCenterY = 0;
    public List<List<float[]>> allMovementPos;
    public List<float[]> allMovementRates;
    public List<float> allMovementGlobalRates;
    public List<string> allMovementPath;

    public MovementFile(){
        allMovementPos = new List<List<float[]>>();
        allMovementRates = new List<float[]>();
        allMovementGlobalRates = new List<float>();
        allMovementPath = new List<string>();
    }

    public void AddMovePath(string path){
        path = Path.Combine(Application.streamingAssetsPath, "Moves", path + ".csv");
        allMovementPath.Add(path);
    }

    public void SaveUkiDatas(){
        List<string> listRHx = new List<string>();
        List<string> listRHy = new List<string>();
        List<string> listLHx = new List<string>();
        List<string> listLHy = new List<string>();
        bool centered; //Did I get the hipCenter value ?
        string HCX;
        string HCY;
        int cpt;
        float x;
        float y;
        int len;
        StreamReader reader;
        List<float[]> oneMovePos; 
        float[] xPos;
        float[] yPos;

        foreach(string movePath in allMovementPath){
            listRHx.Clear();
            listRHy.Clear();
            listLHx.Clear();
            listLHy.Clear();
            oneMovePos = new List<float[]>();
            HCX = "";
            HCY = "";
            cpt = 0;
            centered = false;
            reader = new StreamReader(File.OpenRead(movePath));
            
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
            len = listRHx.Count;
            float.TryParse(HCX, out hipCenterX);
            float.TryParse(HCY, out hipCenterY);
             
            List<List<string>> jointsPos = new List<List<string>>();
            jointsPos.Add(listRHx);
            jointsPos.Add(listRHy);
            jointsPos.Add(listLHx);
            jointsPos.Add(listLHy);

            //ComputeMoveDistance(jointsPos);

            for(int i = 0; i < nbJoints*2; i+=2){
                xPos = new float[len];
                yPos = new float[len];
                for(int k = 0; k < len; k++){
                    float.TryParse(jointsPos[i][k].Replace(".",","), out x);
                    float.TryParse(jointsPos[i+1][k].Replace(".",","), out y); 
                    xPos[k] = x - hipCenterX;
                    yPos[k] = y - hipCenterY;
                }
                oneMovePos.Add(xPos);
                oneMovePos.Add(yPos);
                
            }
            allMovementPos.Add(oneMovePos);
        }
    InitDistances();
    }

    public List<TimeStamp> GetUkiDatas(string path, float timeSpawn, int jump, float speed, float scale, float offsetX, float offsetY
                ,int jointExclusion, TimeStamp DefaultNode)
    {
        #region Get Datas
        int moveIndex = allMovementPath.IndexOf(path);
        List<float> listRHx = new List<float>();
        List<float> listRHy = new List<float>();
        List<float> listLHx = new List<float>();
        List<float> listLHy = new List<float>();
        //We don't take all the points
        int len = allMovementPos[moveIndex][0].Length;
        for (int i = len-1; i > 0; i -= jump){
            listRHx.Add(allMovementPos[moveIndex][0][i]);
            listRHy.Add(allMovementPos[moveIndex][1][i]);
            listLHx.Add(allMovementPos[moveIndex][2][i]);
            listLHy.Add(allMovementPos[moveIndex][3][i]);
        }
        listRHx.Reverse();
        listRHy.Reverse();
        listLHx.Reverse();
        listLHy.Reverse();
        #endregion

        
        #region Add to list
        List<TimeStamp> listTS = new List<TimeStamp>();

        switch(DefaultNode.nodeType){
            case 1 :
                if (jointExclusion == 1 || jointExclusion == 0)
                    InitAddLineNode(listTS, 0, timeSpawn, scale, listRHx, listRHy, offsetX, offsetY, DefaultNode);
                if (jointExclusion == 2 || jointExclusion == 0)
                    InitAddLineNode(listTS, 1, timeSpawn, scale, listLHx, listLHy, offsetX, offsetY, DefaultNode);
                break;
            default :
                if (jointExclusion == 1 || jointExclusion == 0) 
                    InitAddNode(listTS, 0, timeSpawn, speed, scale, listRHx, listRHy, offsetX, offsetY, DefaultNode);
                if (jointExclusion == 2 || jointExclusion == 0)
                    InitAddNode(listTS, 1, timeSpawn, speed, scale, listLHx, listLHy, offsetX, offsetY, DefaultNode);
                break;
        }
        return listTS;
        #endregion
    }

    public void InitDistances(){
        for (int k = 0; k< allMovementPath.Count; k++){
            float totalDist = 0;   
            for (int i = 0; i< allMovementPos[k].Count; i += nbJoints){
                allMovementRates.Add(new float[nbJoints]);
                for(int j = 0; j < allMovementPos[k][0].Length - 1; j++){
                    allMovementRates[k][i/2] += (float) Math.Sqrt(Math.Pow(allMovementPos[k][i][j+1] - allMovementPos[k][i][j],2) + Math.Pow(allMovementPos[k][i+1][j+1] - allMovementPos[k][i+1][j],2));                }
                
                totalDist += allMovementRates[k][i/2]; 
            }

            for(int i = 0; i < allMovementRates[k].Length; i++){
                allMovementRates[k][i] = allMovementRates[k][i]/totalDist * 100;
            }
            //Debug.Log("DistanceR : " + allMovementRates[k][0]);  //Percentage of a move
            //Debug.Log("DistanceL : " + allMovementRates[k][1]);
        }
        
    }

    //Add any type of node except LineNodes
    void InitAddNode(List<TimeStamp> listTS, int joint, float timeSpawn, float speed, float scale, List<float> PosX, List<float> PosY, float offsetX, float offsetY, TimeStamp DefaultNode){
        for(int i =0; i < PosX.Count; i++){
            TimeStamp ts = DefaultNode.DeepCopyTS(DefaultNode);
            ts.joint = joint;
            ts.timeSpawn = i/speed + timeSpawn;
            ts.spawnPosition = new Vector3(PosX[i]*scale + offsetX, PosY[i]*scale + offsetY,0);
            listTS.Add(ts);
        }
    }

    //Add a LineNode
    void InitAddLineNode(List<TimeStamp> listTS, int joint, float timeSpawn, float scale, List<float> PosX, List<float> PosY, float offsetX, float offsetY, TimeStamp DefaultNode){
        TimeStamp ts = DefaultNode.DeepCopyTS(DefaultNode);
        ts.joint = joint;
        Vector3[] pathPositions = new Vector3[PosX.Count-1];
        for(int i = 0; i < PosX.Count; i++){
            if (i == 0){
                ts.spawnPosition = new Vector3(PosX[i]*scale + offsetX,PosY[i]*scale + offsetY,0);
            } else pathPositions[i-1] = new Vector3(PosX[i]*scale + offsetX,PosX[i]*scale + offsetY,0);
        }
        ts.pathPositions = pathPositions;
        ts.timeSpawn = timeSpawn;
        if (ts.spawnPosition.x < 30)
            listTS.Add(ts);
    }

/* 
    //Computes the total distance of a move
    void ComputeMoveDistance(List<List<string>> jointsPos){
        float x;
        int len = jointsPos[0].Count;
        float[][] jointsPosF = new float[jointsPos.Count][];
        

        for (int i = 0; i < jointsPos.Count; i++){
            jointsPosF[i] = new float[len];
            for(int j = 0; j < len; j++){
                float.TryParse(jointsPos[i][j].Replace(".",","), out x);
                jointsPosF[i][j] = x;
            }
        }
           
        for (int i = 0; i< jointsPos.Count/2; i += 2){
            for(int j = 0; j < len - 1; j++){
                jointsRates[i/2] += (float) Math.Sqrt(Math.Pow(jointsPosF[i][j+1] - jointsPosF[i][j],2) + Math.Pow(jointsPosF[i+1][j+1] - jointsPosF[i+1][j],2));
            }
        }
    }  */  
}
