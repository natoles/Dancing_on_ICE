using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCreator : MonoBehaviour
{
    public enum NodeType {BN = 0, LN = 1, AN = 2, HN = 3}
    public enum joint {H = 0, RH = 1, LH = 2}
    NodeCreation creator;
    List<TimeStamp> track = new List<TimeStamp>();
    public float[] timeValues;
    Movements moves = new Movements();
    float[] currentRates = new float[2];
    public float[] wantedRates = new float[2];
    int numberMoves = 0; //Do not touch
    List<MovementFile> allMoves = new List<MovementFile>();
    int movePoolSize = 1;

    void Start()
    {
        creator = new NodeCreation();

        //string simpleMovePath = @"C:\Users\lindi\Desktop\Movements\Basic1\basic1.csv";
        //allMoves.Add(new MovementFile(@"C:\Users\lindi\Desktop\Movements\Basic1\basic1.csv", 100, 0));
        allMoves.Add(new MovementFile(@"C:\Users\lindi\Desktop\Movements\Test1.csv", 0, 100));
        ComputeGlobalRate(allMoves);
        MovementFile chosenMove = SelectMove();
        AddMove(chosenMove, moves.GetUkiDatas(chosenMove.path ,0,8,0.8f,9,0,-1,1, new TimeStamp(0,0,1,4f,Vector3.zero)));
        


        //Add Moves here
        //AddMove(new List<TimeStamp>(moves.TestMovement(0)));
        //AddMove(new List<TimeStamp>(moves.RLRLRL(3)));
        //AddMove(new List<TimeStamp>(moves.RLRLRL2(10)));

        //AddMove(allMoves[0], moves.GetUkiDatas(allMoves[0].path ,0,8,0.8f,9,0,-1,1, new TimeStamp(0,0,1,4f,Vector3.zero)));
        //AddMove(moves.GetUkiDatas(simpleMovePath,0,3,0,9,0,-1,1, new TimeStamp(0,1,1,4f,7f,Vector3.zero,new Vector3[0])));
        //AddMove(moves.GetUkiDatas(simpleMovePath,0,5,0.8f,10,0,0,0, new TimeStamp(0,3,1,4f,5f,Vector3.zero)));

    }

    void Update()
    {
        //Go through the track and sees if a node must be spawned. If yes, spawns it and removes it from the list
        int cpt = 0;
        while (cpt < track.Count){
            if (track[cpt].timeSpawn <= Time.time){
                spawnNode(track[cpt]);
                track.Remove(track[cpt]);
                cpt--;
            }
            cpt++;
        }
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

    void AddMove(MovementFile MF, List<TimeStamp> move){
        numberMoves += 1;
        if (numberMoves == 1){
            currentRates[0] = MF.jointsRates[0];
            currentRates[1] = MF.jointsRates[1];
        } else {
            currentRates[0] = currentRates[0] * (numberMoves-1)/numberMoves + MF.jointsRates[0]/numberMoves;
            currentRates[1] = currentRates[1] * (numberMoves-1)/numberMoves + MF.jointsRates[1]/numberMoves;
        }

        for(int i = 0; i< move.Count; i++){
            track.Add(move[i]);
        }
    }

    MovementFile SelectMove(){
        MovementFile[] movePool = new MovementFile[movePoolSize];
        int r;
        int accuracy = 50;
        for (int i = 0; i < movePoolSize; i++){  //Initialize movePool
            movePool[i] = allMoves[i];
        }
        Array.Reverse(movePool);
        SortMoves(movePool);
        float maxRate = movePool[movePoolSize-1].globalRate;
        Debug.Log("maxRate : " + maxRate);

        //Choose movePollSize moves among the lower globalRate ones
        for (int i = 0; i < accuracy; i++){
            r = (int) Math.Floor(UnityEngine.Random.Range(0f, allMoves.Count-1));
            if (allMoves[r].globalRate <= maxRate){
                InsertMove(allMoves[r], movePool);
                maxRate = movePool[r].globalRate;
            } 
        }
        r = (int) Math.Floor(UnityEngine.Random.Range(0f, movePoolSize));
        return movePool[r];
    }



    public void SortMoves(MovementFile[] moves){
        MovementFile temp; 
        for (int i = 0; i < moves.Length - 1; i++){
            for (int j = i + 1; j < moves.Length; j++){
                if (moves[i].globalRate < moves[j].globalRate) { 
                    temp = moves[i]; 
                    moves[i] = moves[j]; 
                    moves[j] = temp; 
                } 
            } 
        }
    }

    public void InsertMove(MovementFile move, MovementFile[] moves){
        int cpt = 0;
        while(cpt < moves.Length){
            if (move.globalRate <= moves[cpt].globalRate){
                for (int i = moves.Length-1; i > cpt; i--){
                    moves[i] = moves[i-1];
                }
                moves[cpt] = move;
                return;
            }
            cpt++;
        }
    }

    void ComputeGlobalRate(List<MovementFile> moves){
        for (int i = 0; i < moves.Count; i++){
            moves[i].globalRate = 0;
            for (int k = 0; k < wantedRates.Length; k++){
                moves[i].globalRate += wantedRates[k]/100 * Math.Abs(wantedRates[k] - moves[i].jointsRates[k]);
            }
            //Debug.Log(moves[i].globalRate);
        }
    }
}

