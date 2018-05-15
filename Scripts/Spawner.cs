using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	
	void Start () {
        // create random spheres that float about and color them
        List<GameObject> spheres = new List<GameObject>();
        for(int i = 0; i <1500; i++)
        {
            spheres.Add(new GameObject());
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].AddComponent<Rigidbody>();
            spheres[i].AddComponent<Renderer>();
            spheres[i].AddComponent<SphereCollider>();
            spheres[i].transform.position = new Vector3(Random.Range(-100,100), Random.Range(-100, 100), Random.Range(-100, 100));
            spheres[i].transform.localScale = new Vector3(5,5,5);
            var Render = spheres[i].GetComponent<Renderer>();
            if (Random.value > 0.5)
            {
                Render.material.color = Color.clear;
            }
            else
            {
                Render.material.color = Color.red;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
