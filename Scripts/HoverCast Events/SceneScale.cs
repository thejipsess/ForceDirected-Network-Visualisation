using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;


public class SceneScale : MonoBehaviour {
    
    float InitialDistance;
    float CurrentDistance;
    public GameObject Network;
    public Hand Lhand;
    public Hand Rhand;
    public Frame frame;
    public float change;
    public Vector3 SpaceChange;
   


    private void Start()
    {
        enabled = false;
        Network = GameObject.Find("ForceDirectedGrapher");
        //Controller controller = new Controller();
        //frame = controller.Frame();

     
        
    }

    private void FixedUpdate()
    {
        Controller controller = new Controller();
        frame = controller.Frame();
        List<Hand> hands = frame.Hands;

        if (hands.Count > 1)
        {
            Lhand = hands [0];
            Rhand = hands [1];
        }

        CurrentDistance = Lhand.PalmPosition.DistanceTo(Rhand.PalmPosition);
        print("intitals distance is: " + InitialDistance);
        print("Current Distance is: " + CurrentDistance);
        change = CurrentDistance - InitialDistance;
        SpaceChange = new Vector3(change, change, change);
        Network.transform.localScale = Network.transform.localScale + (SpaceChange * 0.00005f);
    }

    public void Scale()
    {
        enabled = true;
        InitialDistance = Lhand.PalmPosition.DistanceTo(Rhand.PalmPosition);

    }

    public void StopScaling()
    {
        enabled = false;
    }
}
