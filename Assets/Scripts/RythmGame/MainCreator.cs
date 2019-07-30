﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MainCreator : MonoBehaviour
{
    NodeCreation creator;
    List<TimeStamp> track = new List<TimeStamp>();
    MovementFile decoyMove = new MovementFile();
    public float[] currentRates; //See AddMove()
    [SerializeField] static public float[] wantedRates = new float[2] {50,50}; //Wanted joints rates needs to be initialise in inspector
    [HideInInspector] public int numberMoves = 0; //Increases each time a move is added
    int movePoolSize = 9; //See SelectMove()
    IEnumerator trackCreation;
    float maxSpawnTime = 0f;
    float globalscale = 8;
    float tmpTime;
    public enum Mode {Random = 0, Basic, Line}
    static public Mode globalNodeType = Mode.Random; 
    public float d = 1;
    public float gameLength; 
    public BodySourceView bodySourceView;
    int totalMoves = 0;
    float startTime;
    public float holdPause = 0; //We pause the level generation to let time for the holdnode to finish
    public MoveInfo[] moveInfo =  new MoveInfo[2];
    

    void Start()
    {
        currentRates = bodySourceView.currentRates;
        creator = new NodeCreation();

        for(int i=0; i<moveInfo.Length; i++){
            decoyMove.AddMovePath(moveInfo[i].path);
        }

        totalMoves = decoyMove.allMovementPath.Count;
        if (totalMoves < movePoolSize) movePoolSize = totalMoves; 

        decoyMove.SaveUkiDatas();

        //Custom Moves : Speed (Basic) or TimeLine (Line) have to be set to 0 to use custom parameters
        decoyMove.SetMoveTimeStamp("basic3",1.3f*d,globalscale*d+3,0,5,4,0,new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero));
        decoyMove.SetMoveTimeStamp("basic9",1.1f*d,globalscale*d,0,2,4,2,new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero));
        
        decoyMove.SetMoveTimeStamp("basic3",1f,globalscale*d + 3,0,5,4,0, new TimeStamp(0,1,0,1.5f,4f*(1/d),Vector3.zero, new Vector3[0]));


        for(int i=0; i<moveInfo.Length; i++){
            if((globalNodeType == Mode.Basic || globalNodeType == Mode.Random) && moveInfo[i].speed != 0){
                SetMoveTimeStampBasic(moveInfo[i].path,moveInfo[i].speed,moveInfo[i].scaleChange,moveInfo[i].jointExclusion);
            }
            if((globalNodeType == Mode.Line || globalNodeType == Mode.Random) && moveInfo[i].timeLine != 0){ 
                SetMoveTimeStampLine(moveInfo[i].path,moveInfo[i].timeLine,moveInfo[i].scaleChange,moveInfo[i].jointExclusion);
            }
        }

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
                if (track[cpt].nodeType == 3) holdPause = track[cpt].timeHold + track[cpt].timeToFinish;
                track.Remove(track[cpt]);
                cpt--;
            }
            cpt++;
        }
        //Debug.Log(currentRates[0]);
    }

    //Shorten verson of SetMoveTimeStamp for BasicNode
    void SetMoveTimeStampBasic(string path, float speed, float scaleChange, int jointExclusion){
        decoyMove.SetMoveTimeStamp(path,speed*d,globalscale*d + scaleChange,0,-1,0,jointExclusion, new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero));                                 
    }
    //Shorten verson of SetMoveTimeStamp for LineNode
    void SetMoveTimeStampLine(string path, float timeLine, float scaleChange, int jointExclusion){
        decoyMove.SetMoveTimeStamp(path,1f,globalscale*d + scaleChange,0,-1,0,jointExclusion, new TimeStamp(0,1,0,1.5f,timeLine*(1/d),Vector3.zero, new Vector3[0]));                                  
    }

    

    IEnumerator TrackCreation(){
        ComputeGlobalRate();
        while(true){    
            int indexChosenMove = SelectMove();
            float r = UnityEngine.Random.Range(0.5f,1.1f);
            tmpTime = maxSpawnTime + r;

            if(holdPause != 0){
                yield return new WaitForSeconds(holdPause);
                tmpTime += holdPause;
                holdPause = 0;
            }

            switch(globalNodeType)
            {
                case(Mode.Random):
                    float rMode = UnityEngine.Random.Range(0f,1f);
                    if (rMode > 0.5f) AddMove(decoyMove.allMovementTimeStampBasic[indexChosenMove]);
                    else AddMove(decoyMove.allMovementTimeStampLine[indexChosenMove]); 
                    break;
                case(Mode.Basic): 
                    AddMove(decoyMove.allMovementTimeStampBasic[indexChosenMove]);
                    break;
                case(Mode.Line):
                    AddMove(decoyMove.allMovementTimeStampLine[indexChosenMove]);
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
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
        #endif
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

    //Add a Move in the track
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

    //Get the timeSpawn of the Node of the track who which will spanw last
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

[System.Serializable]
public class MoveInfo{
    public string path;
    public float speed;
    public float timeLine;
    public float scaleChange;
    public int jointExclusion;
}
