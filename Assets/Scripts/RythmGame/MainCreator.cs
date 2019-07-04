using System.Collections;
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
    public float[] wantedRates; //Wanted joints rates needs to be initialise in inspector
    [HideInInspector] public int numberMoves = 0; //Increases each time a move is added
    List<MovementFile> allMoves = new List<MovementFile>(); //List of all moves, needs to be filled in Start
    List<Action> allMovementFilesLine = new List<Action>();
    List<Action> allMovementFilesBasic = new List<Action>();
    int movePoolSize = 3; //See SelectMove()
    IEnumerator trackCreation;
    float maxSpawnTime = 0f;
    float globalscale = 8;
    float tmpTime;
    public int globalNodeType; //TODO : menu deroulant dans l'inspecteur
    public float d = 1;
    public float gameLength; 
    public BodySourceView bodySourceView;
    

    void Start()
    {
        currentRates = bodySourceView.currentRates;
        creator = new NodeCreation();

        allMoves.Add(new MovementFile("basic1"));      //100,0
        allMoves.Add(new MovementFile("basic2"));      //0,100
        allMoves.Add(new MovementFile("basic3"));      //100,0
        allMoves.Add(new MovementFile("basic4"));      //0,100
        allMoves.Add(new MovementFile("basic5"));      //49.5 50.5
        allMoves.Add(new MovementFile("basic6"));      //46 54
        allMoves.Add(new MovementFile("basic7"));      //62 38
        allMoves.Add(new MovementFile("basic8"));      //84 15
        allMoves.Add(new MovementFile("basic9"));      //19 81


        allMovementFilesBasic.Add(() => AddMove(allMoves[0], decoyMove.GetUkiDatas(allMoves[0],tmpTime,8,1.3f*d,globalscale*d,0,-1,0, new TimeStamp(0,0,0,1f*(1/d),Vector3.zero))));
        allMovementFilesBasic.Add(() => AddMove(allMoves[1], decoyMove.GetUkiDatas(allMoves[1],tmpTime,16,1.3f*d,globalscale*d,0,-1,0, new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero))));
        allMovementFilesBasic.Add(() => AddMove(allMoves[2], decoyMove.GetUkiDatas(allMoves[2],tmpTime,15,1.3f*d,globalscale*d + 3,0,5,0, new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero))));
        allMovementFilesBasic.Add(() => AddMove(allMoves[3], decoyMove.GetUkiDatas(allMoves[3],tmpTime,5,1.3f*d,globalscale*d + 2,0,-1,0, new TimeStamp(0,0,0,1f*(1/d),Vector3.zero))));
        allMovementFilesBasic.Add(() => AddMove(allMoves[4], decoyMove.GetUkiDatas(allMoves[4],tmpTime,17,1.3f*d,globalscale*d + 2,0,-1,0, new TimeStamp(0,0,0,1f*(1/d),Vector3.zero))));
        allMovementFilesBasic.Add(() => AddMove(allMoves[5], decoyMove.GetUkiDatas(allMoves[5],tmpTime,15,1.3f*d,globalscale*d + 2,0,-1,0, new TimeStamp(0,0,0,1.2f*(1/d),Vector3.zero))));
        allMovementFilesBasic.Add(() => AddMove(allMoves[6], decoyMove.GetUkiDatas(allMoves[6],tmpTime,12,1f*d,globalscale*d + 2,0,-1,0, new TimeStamp(0,0,0,1.3f*(1/d),Vector3.zero))));
        allMovementFilesBasic.Add(() => AddMove(allMoves[7], decoyMove.GetUkiDatas(allMoves[7],tmpTime,25,1f*d,globalscale*d + 2,0,-1,1, new TimeStamp(0,0,0,1.6f*(1/d),Vector3.zero))));
        allMovementFilesBasic.Add(() => AddMove(allMoves[8], decoyMove.GetUkiDatas(allMoves[8],tmpTime,15,1.1f*d,globalscale*d,0,2,2, new TimeStamp(0,0,0,1.3f*(1/d),Vector3.zero))));



        allMovementFilesLine.Add(() => AddMove(allMoves[0], decoyMove.GetUkiDatas(allMoves[0],tmpTime,8,1f,globalscale*d + 1.5f,0,-1,0, new TimeStamp(0,1,0,1.5f,3f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(allMoves[1], decoyMove.GetUkiDatas(allMoves[1],tmpTime,8,1f,globalscale*d,0,-1,0, new TimeStamp(0,1,0,1.5f,4.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(allMoves[2], decoyMove.GetUkiDatas(allMoves[2],tmpTime,15,1f,globalscale*d + 3,0,5,0, new TimeStamp(0,1,0,1.5f,4f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(allMoves[3], decoyMove.GetUkiDatas(allMoves[3],tmpTime,12,1f,globalscale*d + 3,0,-1,0, new TimeStamp(0,1,0,1.5f,3f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(allMoves[4], decoyMove.GetUkiDatas(allMoves[4],tmpTime,17,1f,globalscale*d + 1.5f,0,-1,0, new TimeStamp(0,1,0,1.5f,2.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(allMoves[5], decoyMove.GetUkiDatas(allMoves[5],tmpTime,17,1f,globalscale*d + 1.5f,0,-1,0, new TimeStamp(0,1,0,1.5f,3.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(allMoves[6], decoyMove.GetUkiDatas(allMoves[6],tmpTime,9,1f,globalscale*d + 1.5f,0,-1,0, new TimeStamp(0,1,0,1.5f,3.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(allMoves[7], decoyMove.GetUkiDatas(allMoves[7],tmpTime,25,1f,globalscale*d + 1.5f,0,-1,1, new TimeStamp(0,1,0,1.5f,5.5f*(1/d),Vector3.zero, new Vector3[0]))));
        allMovementFilesLine.Add(() => AddMove(allMoves[8], decoyMove.GetUkiDatas(allMoves[8],tmpTime,20,1f,globalscale*d,0,0,2, new TimeStamp(0,1,0,1.5f,4f*(1/d),Vector3.zero, new Vector3[0]))));



        for (int i = 0; i< allMoves.Count; i++){
            allMoves[i].InitDistances();
        }
        trackCreation = TrackCreation();
        StartCoroutine(trackCreation);
        StartCoroutine(ExitGame());

        
        

        /* DATABASE */
        //AddMove(allMoves[0], moves.GetUkiDatas(allMoves[0].path ,0,8,0.8f,9,0,-1,1, new TimeStamp(0,0,1,4f,Vector3.zero)));
        //AddMove(moves.GetUkiDatas(simpleMovePath,0,3,0,9,0,-1,1, new TimeStamp(0,1,1,4f,7f,Vector3.zero,new Vector3[0])));
        //AddMove(moves.GetUkiDatas(simpleMovePath,0,5,0.8f,10,0,0,0, new TimeStamp(0,3,1,4f,5f,Vector3.zero)));

    }

    void Update()
    {
        
        //Goes through the track and sees if a node must be spawned. If yes, spawns it and removes it from the list
        int cpt = 0;
        while (cpt < track.Count){
            if (track[cpt].timeSpawn <= Time.time){
                spawnNode(track[cpt]);
                track.Remove(track[cpt]);
                cpt--;
            }
            cpt++;
        }
        //Debug.Log(currentRates[0]);
    }

    IEnumerator TrackCreation(){
        ComputeGlobalRate(allMoves);
        for (int i = 0; i < 100; i++){
            MovementFile chosenMove = SelectMove();
            float r = UnityEngine.Random.Range(0.5f,1.1f);
            tmpTime = maxSpawnTime + r;

            switch(globalNodeType)
            {
                case(0): 
                    allMovementFilesBasic[allMoves.FindIndex(move => move == chosenMove)].Invoke();
                    break;
                case(1):
                    allMovementFilesLine[allMoves.FindIndex(move => move == chosenMove)].Invoke();
                    break;
                default:
                    Debug.Log("Pas normal");
                    break;
            }
            

            //Debug.Log("SpawnT : " + maxSpawnTime);
            yield return new WaitForSeconds(maxSpawnTime - tmpTime + r);//Pause during the move to select with recent datas
        }
    }

    IEnumerator ExitGame(){
        yield return new WaitForSeconds(gameLength);
        Debug.Log("End of the game !");
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

    void AddMove(MovementFile MF, List<TimeStamp> move){
        //Changes the value od the currentRates
        Debug.Log("Rates : " + currentRates[0] + ", " + currentRates[1]);   //Current amount the player has moved
        for(int i = 0; i< move.Count; i++){
            track.Add(move[i]);
        }
        SetMaxSpawnTime();
        ComputeGlobalRate(allMoves); 
        numberMoves += 1;
    }

    //Returns one of the moves to match correctly with the desire rates
    //Randomization is added to vary moves, decrease accuracy to augment variety amongs chosen moves
    //Increase movePoolSize to augment variety among chosen moves
    MovementFile SelectMove(){
        MovementFile[] movePool = new MovementFile[movePoolSize];
        int r;
        int accuracy = 50;
        for (int i = 0; i < movePoolSize; i++){  //Initialize movePool
            movePool[i] = allMoves[i];
        }
        SortMoves(movePool);
        Array.Reverse(movePool);
        float maxRate = movePool[movePoolSize-1].globalRate;

        //Chooses movePollSize moves among the lower globalRate ones
        for (int i = 0; i < accuracy; i++){
            r = (int) Math.Floor(UnityEngine.Random.Range(0f, allMoves.Count));
            if (allMoves[r].globalRate <= maxRate && !Array.Exists(movePool, element => element == allMoves[r])){
                InsertMove(allMoves[r], movePool);
                maxRate = movePool[movePoolSize-1].globalRate;
            } 
        }
        r = (int) Math.Floor(UnityEngine.Random.Range(0f, movePoolSize));
        return movePool[r];
    }


    //Sort the moves accroding to their globalRate
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

    //insert a move into the array of moves
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

    //Compute the score of each move according to the current Rate
    void ComputeGlobalRate(List<MovementFile> moves){

        for (int i = 0; i < moves.Count; i++){
            moves[i].globalRate = 0;
            for (int k = 0; k < wantedRates.Length; k++){
                moves[i].globalRate += Math.Abs(wantedRates[k] - (currentRates[k] * numberMoves/(numberMoves+1) + moves[i].jointsRates[k]/(numberMoves + 1)));
            }
            //Debug.Log(moves[i].path + " : " + moves[i].globalRate); Score of the move
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
