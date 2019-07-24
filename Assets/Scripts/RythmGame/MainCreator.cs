﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MainCreator : MonoBehaviour
{
    public enum NodeType {BN = 0, LN = 1, AN = 2, HN = 3}
    public enum joint {RH = 0, LH = 1}
    NodeCreation creator;
    List<TimeStamp> track = new List<TimeStamp>();
    MovementFile decoyMove = new MovementFile();
    public int nbJoints = 2;
    public float[] currentRates; //See AddMove()
    static public float[] wantedRates = new float[2] {50,50}; //Wanted joints rates needs to be initialise in inspector
    [HideInInspector] public int numberMoves = 0; //Increases each time a move is added
    List<MovementFile> allMoves = new List<MovementFile>(); //List of all moves, needs to be filled in Start
    List<Action> allMovementFilesLine = new List<Action>();
    List<Action> allMovementFilesBasic = new List<Action>();
    int movePoolSize = 3; //See SelectMove()
    IEnumerator trackCreation;
    float maxSpawnTime = 0f;
    float globalscale = 8;
    float SpawnInterval = 0.5f;
    float tmpTime;
    static public int globalNodeType = 0; 
    public float d = 1;
    public float gameLength; 
    public BodySourceView bodySourceView;
    int totalMoves = 0;
    float startTime;
    

    void Start()
    {
        currentRates = bodySourceView.currentRates;
        creator = new NodeCreation();
        
        decoyMove.AddMovePath("basic1");      //100,0 
        decoyMove.AddMovePath("basic2");      //100,0
        decoyMove.AddMovePath("basic3");      //0,100
        decoyMove.AddMovePath("basic4");      //0,100
        decoyMove.AddMovePath("basic5");      //49.5 50.5
        decoyMove.AddMovePath("basic6");      //46 54
        decoyMove.AddMovePath("basic7");      //62 38
        decoyMove.AddMovePath("basic8");      //84 15
        decoyMove.AddMovePath("basic9");      //19 81*/
        
        totalMoves = decoyMove.allMovementPath.Count;
        Debug.Log(totalMoves);
        decoyMove.SaveUkiDatas();


        
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[0],tmpTime,1.3f*d,globalscale*d,0,-1,0, new TimeStamp(0,0,0,1f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[1],tmpTime,1.3f*d,globalscale*d,0,-1,0, new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[2],tmpTime,1.3f*d,globalscale*d + 3,0,5,0, new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[3],tmpTime,1.3f*d,globalscale*d + 2,0,-1,0, new TimeStamp(0,0,0,1f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[4],tmpTime,1.3f*d,globalscale*d + 2,0,-1,0, new TimeStamp(0,0,0,1f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[5],tmpTime,1.3f*d,globalscale*d + 2,0,-1,0, new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[6],tmpTime,1f*d,globalscale*d + 2,0,-1,0, new TimeStamp(0,0,0,1.3f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[7],tmpTime,1f*d,globalscale*d + 2,0,-1,1, new TimeStamp(0,0,0,1.6f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[8],tmpTime,1.1f*d,globalscale*d,0,2,2, new TimeStamp(0,0,0,1.3f*(1/d),Vector3.zero));
        
        /* 
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[0],tmpTime,1f,globalscale*d + 1.5f,0,-1,0, new TimeStamp(0,1,0,1.5f,3f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[1],tmpTime,1f,globalscale*d,0,-1,0, new TimeStamp(0,1,0,1.5f,4.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[2],tmpTime,1f,globalscale*d + 3,0,5,0, new TimeStamp(0,1,0,1.5f,4f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[3],tmpTime,1f,globalscale*d + 3,0,-1,0, new TimeStamp(0,1,0,1.5f,3f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[4],tmpTime,1f,globalscale*d + 1.5f,0,-1,0, new TimeStamp(0,1,0,1.5f,2.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[5],tmpTime,1f,globalscale*d + 1.5f,0,-1,0, new TimeStamp(0,1,0,1.5f,3.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[6],tmpTime,1f,globalscale*d + 1.5f,0,-1,0, new TimeStamp(0,1,0,1.5f,3.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[7],tmpTime,1f,globalscale*d + 1.5f,0,-1,1, new TimeStamp(0,1,0,1.5f,5.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(decoyMove.SetMoveTimeStamp(decoyMove.allMovementPath[8],tmpTime,1f,globalscale*d,0,0,2, new TimeStamp(0,1,0,1.5f,4f*(1/d),Vector3.zero, new Vector3[0]))));
        */

        trackCreation = TrackCreation();
        StartCoroutine(trackCreation);
        StartCoroutine(ExitGame());
        startTime = Time.time;
    }

    void Update()
    {
        
        //Goes through the track and sees if a node must be spawned. If yes, spawns it and removes it from the list
        int cpt = 0;
        while (cpt < track.Count){
            if (track[cpt].timeSpawn <= Time.time - startTime){
                spawnNode(track[cpt]);
                track.Remove(track[cpt]);
                cpt--;
            }
            cpt++;
        }
        //Debug.Log(currentRates[0]);
    }

    //WIP 
    /* 
    public void ImportMove(string path){
        //decoyMove.AddMovePath("basic1");
        decoyMove.AddMovePath(path);
        allMovementFilesBasic.Add(() => AddMove(decoyMove.GetUkiDatas(decoyMove.GetPath(path),tmpTime,1.3f*d,globalscale*d,0,0,0, new TimeStamp(0,0,0,1f*(1/d),Vector3.zero))));
    }*/

    IEnumerator TrackCreation(){
        ComputeGlobalRate();
        for (int i = 0; i < 100; i++){   //Max number of moves
            int indexChosenMove = SelectMove();
            float r = UnityEngine.Random.Range(0.5f,1.1f);
            tmpTime = maxSpawnTime + r;
            Debug.Log(tmpTime);

            switch(globalNodeType)
            {
                case(0): 
                    AddMove(decoyMove.allMovementTimeStamp[indexChosenMove]);
                    break;
                case(1):
                    allMovementFilesLine[indexChosenMove].Invoke();
                    break;
                default:
                    Debug.Log("Pas normal");
                    break;
            }

            while (track.Count > 0){  //Pause during the move to select a new move with recent movement datas
                yield return null ;
            } 
        }
    }

    IEnumerator ExitGame(){
        yield return new WaitForSeconds(gameLength);
        Debug.Log("End of the game !");
        SceneHistory.LoadPreviousScene();
        //#if UNITY_EDITOR
        //UnityEditor.EditorApplication.ExitPlaymode();
        //#endif
    }

    //Chooses the right constructor according to what is asked
    void spawnNode(TimeStamp ts){
        switch(ts.nodeType)
        {
            case 0 : 
                creator.CreateBasicNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.spawnPosition);
                break;
            case 1 : 
                creator.CreateLineNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.timeLine, ts.spawnPosition, ts.pathPositions);
                break; 
            case 2 : 
                creator.CreateAngleNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.startAngle, ts.spawnPosition);
                break;
            case 3 : 
                creator.CreateHoldNode((NodeCreation.Joint) ts.joint, ts.timeToFinish, ts.timeHold, ts.spawnPosition);
                break;
            default :
                Debug.Log("PAS NORMAL");
                break;
        }
    }

    void AddMove(List<TimeStamp> move){
        TimeStamp tsCopy = new TimeStamp(0,0,0);
        for(int i = 0; i< move.Count; i++){
            tsCopy = tsCopy.DeepCopyTS(move[i]);
            tsCopy.timeSpawn += tmpTime;
            track.Add(tsCopy);
        }
        SetMaxSpawnTime();
        ComputeGlobalRate(); 
        numberMoves += 1;
    }

    //Returns one of the moves to match correctly with the desire rates
    //Randomization is added to vary moves, decrease accuracy to augment variety amongs chosen moves
    //Increase movePoolSize to augment variety among chosen moves
    int SelectMove(){
        int[] movePool = new int[movePoolSize];
        int r;
        int accuracy = 20;
        for (int i = 0; i < movePoolSize; i++){  //Initialize movePool
            movePool[i] = i;
        }
        SortMoves(movePool);
        Array.Reverse(movePool);
        float maxRate = decoyMove.allMovementGlobalRates[movePool[movePoolSize-1]];

        //Chooses movePollSize moves among the lower globalRate ones
        for (int i = 0; i < accuracy; i++){
            r = (int) Math.Floor(UnityEngine.Random.Range(0f, totalMoves));
            if (decoyMove.allMovementGlobalRates[r] <= maxRate && !Array.Exists(movePool, element => element == r)){
                InsertMove(r, movePool);
                maxRate = decoyMove.allMovementGlobalRates[movePool[movePoolSize-1]];
            } 
        }
        r = (int) Math.Floor(UnityEngine.Random.Range(0f, movePoolSize));
        return movePool[r];
    }


    //Sort the moves accroding to their globalRate
    public void SortMoves(int[] moves){
        int temp; 
        for (int i = 0; i < moves.Length - 1; i++){
            for (int j = i + 1; j < moves.Length; j++){
                if (decoyMove.allMovementGlobalRates[i] < decoyMove.allMovementGlobalRates[j]) { 
                    temp = moves[i]; 
                    moves[i] = moves[j]; 
                    moves[j] = temp; 
                } 
            } 
        }
    }

    //insert a move into the array of moves
    public void InsertMove(int move, int[] moves){
        int cpt = 0;
        while(cpt < moves.Length){
            if (decoyMove.allMovementGlobalRates[move] <= decoyMove.allMovementGlobalRates[moves[cpt]]){
                for (int i = moves.Length-1; i > cpt; i--){
                    moves[i] = moves[i-1];
                }
                moves[cpt] = move;
                return;
            }
            cpt++;
        }
    }

    //Compute the score of each move according to the current Rate
    void ComputeGlobalRate(){
        float tmp;
        decoyMove.allMovementGlobalRates.Clear();
        for (int i = 0; i < totalMoves; i++){
            tmp = 0;
            for (int k = 0; k < wantedRates.Length; k++){
                tmp += Math.Abs(wantedRates[k] - (currentRates[k] * numberMoves/(numberMoves+1) + decoyMove.allMovementRates[i][k]/(numberMoves + 1)));
            }
            decoyMove.allMovementGlobalRates.Add(tmp);
            //Debug.Log(tmp);
            //Debug.Log(decoyMove.allMovementGlobalRates[i]);
        }
    }

    void SetMaxSpawnTime(){
        for (int i = 0; i < track.Count; i++){
            if(track[i].nodeType != 1){
                if (track[i].timeSpawn > maxSpawnTime){
                    maxSpawnTime = track[i].timeSpawn + track[i].timeToFinish;
                }
            } else {
                if (track[i].timeSpawn + track[i].timeLine > maxSpawnTime){
                    maxSpawnTime = track[i].timeSpawn + track[i].timeToFinish + track[i].timeLine;
                }
            }
        }
    }
    
}
