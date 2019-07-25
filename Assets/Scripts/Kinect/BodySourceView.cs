using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using System;

public class BodySourceView : MonoBehaviour 
{
    public BodySourceManager mBodySourceManager;
    public GameObject rightHand;
    public GameObject leftHand;
    int cpt = 0;
    bool initBody;
    float[] realJointsMovements;
    float[] previousJointsPos;
    float[] jointPos;
    public float[] currentRates;
    int nbFrames = 0;
    public float maxTrackDistance;
    int nbJoints = 2;
    
    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    //Joints we want to show
    private List<JointType> _joints = new List<JointType>{
        JointType.HandLeft,
        JointType.HandRight,
    };

    private GameObject[] _sprites = new GameObject[4];

    void Start()
    {
        _sprites[0] = leftHand;
        _sprites[1] = rightHand;

        realJointsMovements = new float[nbJoints];
        previousJointsPos = new float[nbJoints*2];
        jointPos = new float[nbJoints*2];
        currentRates = new float[nbJoints];
    }
    void Update () 
    {   
        #region Get Kinect data
        Body[] data = mBodySourceManager.GetData();
        if (data == null)
        {
            return;
        }
        
        //Give an ID to all the bodies we want to track
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
                continue;

            if(body.IsTracked && TrackOnDistance(body)){
                    trackedIds.Add (body.TrackingId);
                    break;
                } 
            
        }
        #endregion
        
        


        #region Delete Kinect bodies
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        
        // if this list contains a bodyID who is not present in the 
        //detected IDs, remove this body
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                //Destroy body Object
                Destroy(mBodies[trackingId]);
                //Remove frome list
                mBodies.Remove(trackingId);
            }
        }
        #endregion

        #region Create Kinect bodies
        //Add a body if it is tracked but not stored

        foreach(var body in data)
        {

            if (body == null)
                continue;

            
            //Debug.Log("saveId : " + saveId);
            if(body.IsTracked)
            {
                if (TrackOnDistance(body)){
                    if(!mBodies.ContainsKey(body.TrackingId))
                        mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    //Update positions
                    UpdateBodyObject(body, mBodies[body.TrackingId]);
                    UpdateCurrentRates(mBodies[body.TrackingId]);
                    
                }
                
            }
        }
        #endregion

        
        nbFrames ++;
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        cpt = 0;
        foreach(JointType joint in _joints)
        {
            //Attaches the gameobject we chose to all the joint we selected previously
            GameObject newJoint = Instantiate(_sprites[cpt++]);
            newJoint.name = joint.ToString();

            //Set the relative position of the joint to it's parent(the body)
            newJoint.transform.parent = body.transform;
        }
        
        return body;
    }
    

    private bool TrackOnDistance(Body body)
    {
        foreach(JointType _joint in _joints)
        {
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            if (targetPosition.z > maxTrackDistance) return false;
        }

        return true;
    }


    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        //Update joints
        foreach(JointType _joint in _joints)
        {
            //Get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            //Debug.Log(targetPosition);
            targetPosition.z = 0; //for 2D

            //Get joint, set new position
            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;
        }
    }
    private static Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    void UpdateCurrentRates(GameObject bodyObject){
        if (nbFrames > 0){
            jointPos[0] = bodyObject.transform.Find((JointType.HandRight).ToString()).position.x;
            jointPos[1] = bodyObject.transform.Find((JointType.HandRight).ToString()).position.y;
            jointPos[2] = bodyObject.transform.Find((JointType.HandLeft).ToString()).position.x;
            jointPos[3] = bodyObject.transform.Find((JointType.HandLeft).ToString()).position.y;
            float totalDist = 0;

            for (int i = 0; i<= previousJointsPos.Length/2; i+=2){
                realJointsMovements[i/2] += (float) Math.Sqrt(Math.Pow(jointPos[i] - previousJointsPos[i],2) + Math.Pow(jointPos[i+1] - previousJointsPos[i+1],2));
                totalDist += realJointsMovements[i/2];
            }
            Debug.Log("hey");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Left,Right,Total: " + realJointsMovements[1] + ", " + realJointsMovements[0] + ", " + totalDist);
            }
    
            for (int i = 0; i<realJointsMovements.Length; i++){
                currentRates[i] = realJointsMovements[i]/totalDist * 100;
            }

            previousJointsPos[0] = jointPos[0];
            previousJointsPos[1] = jointPos[1];
            previousJointsPos[2] = jointPos[2];
            previousJointsPos[3] = jointPos[3];

        }        
    }
}
