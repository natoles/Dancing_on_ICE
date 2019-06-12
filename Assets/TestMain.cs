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
        /*creator.CreateBasicNode(NodeCreation.Joint.Hand, 10);
        creator.CreateBasicNode(NodeCreation.Joint.RightHand, 9, new Vector3(-6,-6,0));
        creator.CreateBasicNode(NodeCreation.Joint.LeftHand, 5);
        creator.CreateBasicNode(NodeCreation.Joint.Hand);*/

        creator.CreateLineNode(NodeCreation.Joint.RightHand);
        creator.CreateLineNode(NodeCreation.Joint.LeftHand, 2, 2, new Vector3(-8,-8,0));
        creator.CreateLineNode(NodeCreation.Joint.RightHand, 8, 8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
