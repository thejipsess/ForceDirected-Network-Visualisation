using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    public float MaxRad;
    public float MinRad;
    public float GravitiationalForce;
    private float pullforce;
    private float Distance;
	
	// Update is called once per frame
	void FixedUpdate () {
        Collider[] colliders = Physics.OverlapSphere(transform.position, MaxRad);
        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb == null) continue;
            Vector3 Direction = transform.position - collider.transform.position;
            Distance = Vector3.Distance(transform.position, rb.transform.position);
            pullforce = Mathf.Log10(Distance / MinRad) * GravitiationalForce;
            
            rb.AddForce(Direction.normalized * pullforce * Time.fixedDeltaTime);
            
        }
	}
}
