using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsiveForce : MonoBehaviour {
    public Rigidbody Repellant;
    public float RepelForce;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forceDirection = transform.position - Repellant.transform.position;
        Repellant.GetComponent<Rigidbody>().AddForce((-forceDirection.normalized * RepelForce) / 2 * Time.fixedDeltaTime);
        GetComponent<Rigidbody>().AddForce((forceDirection.normalized * RepelForce) / 2 * Time.fixedDeltaTime);
    }
}
