using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class BodySourceView : MonoBehaviour 
{
    public BodySourceManager mBodySourceManager;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject rightFoot;
    public GameObject leftFoot;
    int cpt = 0;
    float near = 0;
    bool initBody;
    
    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    //Joints we want to show
    private List<JointType> _joints = new List<JointType>{
        JointType.HandLeft,
        JointType.HandRight,
        JointType.FootLeft,
        JointType.FootRight,
    };

    private GameObject[] _sprites = new GameObject[4];
    /* 
    private List<GameObject> _sprites = new List<GameObject>{
        rightHand,
        leftHand,
        rightFoot,
        leftFoot,
    };
    */
    void Start()
    {
        _sprites[0] = leftHand;
        _sprites[1] = rightHand;
        _sprites[2] = leftFoot;
        _sprites[3] = rightFoot;
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

            if(body.IsTracked){
                    trackedIds.Add (body.TrackingId);
                    //near = mBodies[body.TrackingId].transform.position.z;
                    break;
                } 
                
            /* 
            if(!initBody){
                initBody = true;
                if(body.IsTracked){
                    trackedIds.Add (body.TrackingId);
                    near = mBodies[body.TrackingId].transform.position.z;
                    break;
                } 
            } else {
                if(body.IsTracked &&  mBodies[body.TrackingId].transform.position.z <= near + 10 &&  mBodies[body.TrackingId].transform.position.z >= near - 10){
                    trackedIds.Add (body.TrackingId);
                    break;
                }
            }*/
            
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
            
            if(body.IsTracked)
            {
                if(!mBodies.ContainsKey(body.TrackingId))
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                //Update positions
                UpdateBodyObject(body, mBodies[body.TrackingId]);
            }
        }
        #endregion
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
    
    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        //Update joints
        foreach(JointType _joint in _joints)
        {
            //Get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
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
}
