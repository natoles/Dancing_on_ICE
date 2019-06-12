using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMain : MonoBehaviour
{
    NodeCreation creator;
    public GameObject self;
    void Start()
    {
        creator = self.AddComponent<NodeCreation>();
        creator.CreateBasicNode(NodeCreation.Joint.Hand, 10);
        creator.CreateBasicNode(NodeCreation.Joint.RightHand, 9, new Vector3(-6,-6,0));
        creator.CreateBasicNode(NodeCreation.Joint.LeftHand, 5);
        creator.CreateBasicNode(NodeCreation.Joint.Hand);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
