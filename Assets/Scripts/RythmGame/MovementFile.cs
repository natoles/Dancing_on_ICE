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
    public List<List<TimeStamp>> allMovementTimeStampBasic;
    public List<List<TimeStamp>> allMovementTimeStampLine;
    public List<float[]> allMovementRates;
    public List<float> allMovementGlobalRates;
    public List<string> allMovementPath;
    float nodeDistance = 2.5f; //Distance needed between two nodes (high = less nodes)
    int maxJump = 60;//Max number of frames between two nodes

    public MovementFile(){
        allMovementPos = new List<List<float[]>>();
        allMovementRates = new List<float[]>();
        allMovementGlobalRates = new List<float>();
        allMovementPath = new List<string>();
        allMovementTimeStampBasic = new List<List<TimeStamp>>();
        allMovementTimeStampLine = new List<List<TimeStamp>>();
    }

    //Adds a move 
    public void AddMovePath(string path){
        path = Path.Combine(Application.streamingAssetsPath, "Moves", path + ".csv");
        allMovementPath.Add(path);
    }

    public string GetPath(string path){
        path = Path.Combine(Application.streamingAssetsPath, "Moves", path + ".csv");
        return path;
    }

    //Stores every datas from the csv file into arrays (to call at the start of the game)
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

    //Canges the datas from the arrays to correspond to the parameters
    public void SetMoveTimeStamp(string path, float speed, float scale, float offsetX, float offsetY
                ,int jointExclusion, TimeStamp defaultNode)
    {
        #region Get Datas
        
        int moveIndex = allMovementPath.IndexOf(GetPath(path));
        List<float> listRHx = new List<float>();
        List<float> listRHy = new List<float>();
        List<float> listLHx = new List<float>();
        List<float> listLHy = new List<float>();

        //We don't take all the points
        int len = allMovementPos[moveIndex][0].Length;
        float distR = 0;
        float distL = 0;
        int cptR = 0;
        int cptL = 0;
        
        //We take a point after the distance nodeDistance is travelled(We always have the last node)
        listRHx.Add(allMovementPos[moveIndex][0][len - 1]);
        listRHy.Add(allMovementPos[moveIndex][1][len - 1]);
        listLHx.Add(allMovementPos[moveIndex][2][len - 1]);
        listLHy.Add(allMovementPos[moveIndex][3][len - 1]);
        for (int i = len-2; i > 0; i -= 1){
            cptR++;
            cptL++;
            distR +=(float) Math.Sqrt(Math.Pow(allMovementPos[moveIndex][0][i+1] - allMovementPos[moveIndex][0][i], 2) + Math.Pow(allMovementPos[moveIndex][1][i+1] - allMovementPos[moveIndex][1][i], 2)) * scale;
            distL +=(float) Math.Sqrt(Math.Pow(allMovementPos[moveIndex][2][i+1] - allMovementPos[moveIndex][2][i], 2) + Math.Pow(allMovementPos[moveIndex][3][i+1] - allMovementPos[moveIndex][3][i], 2)) * scale ;
            if (distR >= nodeDistance || cptR > maxJump){
                cptR = 0;
                distR = 0;
                listRHx.Add(allMovementPos[moveIndex][0][i]);
                listRHy.Add(allMovementPos[moveIndex][1][i]);
            }
            if (distL >= nodeDistance || cptL > maxJump){
                cptL = 0;
                distL = 0;
                listLHx.Add(allMovementPos[moveIndex][2][i]);
                listLHy.Add(allMovementPos[moveIndex][3][i]);
            }
        }
        listRHx.Reverse();
        listRHy.Reverse();
        listLHx.Reverse();
        listLHy.Reverse();
        #endregion

        #region Add to list
        List<TimeStamp> listTS = new List<TimeStamp>();

        switch(defaultNode.nodeType){
            case 1 :
                if (jointExclusion == 1 || jointExclusion == 0)
                    InitAddLineNode(listTS, 0, scale, listRHx, listRHy, offsetX, offsetY, defaultNode);
                if (jointExclusion == 2 || jointExclusion == 0)
                    InitAddLineNode(listTS, 1, scale, listLHx, listLHy, offsetX, offsetY, defaultNode);
                break;
            default :
                if (jointExclusion == 1 || jointExclusion == 0) 
                    InitAddNode(listTS, 0, speed, scale, listRHx, listRHy, offsetX, offsetY, defaultNode);
                if (jointExclusion == 2 || jointExclusion == 0)
                    InitAddNode(listTS, 1, speed, scale, listLHx, listLHy, offsetX, offsetY, defaultNode);
                break;
        }
        switch(defaultNode.nodeType){
            case(0):
                allMovementTimeStampBasic.Add(listTS);
                break;
            case(1):
                allMovementTimeStampLine.Add(listTS);
                break;
            default:
                Debug.Log("Unsupported Node Type");
                break;
        }
        #endregion
    }


    //Computes the percentage of disctance travelled for each movement
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
            //Debug.Log("DistanceR : " + allMovementRates[k][0]);  //Percentages of a move
            //Debug.Log("DistanceL : " + allMovementRates[k][1]);
        }
        
    }

    //Init any type of node except LineNodes
    void InitAddNode(List<TimeStamp> listTS, int joint, float speed, float scale, List<float> PosX, List<float> PosY, float offsetX, float offsetY, TimeStamp defaultNode){
        for(int i =0; i < PosX.Count; i++){
            TimeStamp ts = defaultNode.DeepCopyTS(defaultNode);
            ts.joint = joint;
            ts.timeSpawn = i/speed; 
            ts.spawnPosition = new Vector3(PosX[i]*scale + offsetX, PosY[i]*scale + offsetY,0);
            listTS.Add(ts);
        }
    }

    //Add a LineNode
    void InitAddLineNode(List<TimeStamp> listTS, int joint, float scale, List<float> PosX, List<float> PosY, float offsetX, float offsetY, TimeStamp defaultNode){
        TimeStamp ts = defaultNode.DeepCopyTS(defaultNode);
        ts.joint = joint;
        if (PosX.Count > 0){
            Vector3[] pathPositions = new Vector3[PosX.Count-1];
            for(int i = 0; i < PosX.Count; i++){
                if (i == 0){
                    ts.spawnPosition = new Vector3(PosX[i]*scale + offsetX,PosY[i]*scale + offsetY,0);
                } else pathPositions[i-1] = new Vector3(PosX[i]*scale + offsetX,PosY[i]*scale + offsetY,0);
            }
            ts.pathPositions = pathPositions;
            if (ts.spawnPosition.x < 30)
                listTS.Add(ts);
        }
    }

  
}
