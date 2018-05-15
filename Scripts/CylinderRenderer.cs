using System.Collections.Generic;
using UnityEngine;

public class CylinderRenderer : MonoBehaviour
{
    public GameObject goStart;
    public GameObject goEnd;
 
public void Start()
    {
        var v3Start = goStart.transform;
        var v3End = goEnd.transform;
        var v3T = transform.localScale;
        v3T.x = (v3End.localScale.x + v3Start.localScale.x) / 20;
        v3T.z = (v3End.localScale.z + v3Start.localScale.z) / 20;
    }

    public void Update()
    {
        {
            var v3Start = goStart.transform;
            var v3End = goEnd.transform;

            transform.position = (v3End.position - v3Start.position) / 2.0f + v3Start.position;

            var v3T = transform.localScale;
            v3T.y = (v3End.position - v3Start.position).magnitude /2;
            v3T.x = (v3End.localScale.x + v3Start.localScale.x) / 20;
            v3T.z = (v3End.localScale.z + v3Start.localScale.z) / 20;
            transform.localScale = v3T;

            transform.rotation = Quaternion.FromToRotation(Vector3.up, v3End.position - v3Start.position);
        }
    }
}

