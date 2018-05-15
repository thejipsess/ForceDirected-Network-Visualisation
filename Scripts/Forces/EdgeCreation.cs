using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCreation : MonoBehaviour {
    private LineRenderer lineRenderer;
    private float counter;

    public Transform origin;
    public Transform destination;
	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = .45f;
        lineRenderer.endWidth = .45f;
	}
	
	// Update is called once per frame
	void Update () {
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, destination.position);


    }
}
