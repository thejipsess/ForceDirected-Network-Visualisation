using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Attractiveforce : MonoBehaviour {
    public Rigidbody AttractiveNode1;
    public Rigidbody AttractiveNode2;
    public Rigidbody RepellentNode1;
    public Rigidbody RepellentNode2;
    private float pullforce;
    private float pushforce;
    public float IdealConnectedDistance;
    private float distance;
    public float PullSpeed;
    public float PushSpeed;
    List<int> Black = new List<int>();
    List<int> Red = new List<int>();
    List<GameObject> spheres = new List<GameObject>();
    private int CentralBlack;
    private int CentralRed;
    private Vector3 forceDirection;
    private LineRenderer lineRenderer;
    public float MaxSpeed;
    public int NodeNumber;

    void Start()
    {

        //GameObject DataImportObject = GameObject.Find("DataImportObject");
        //DataImporter DataImportScript = DataImportObject.GetComponent<DataImporter>();
        //print(DataImportScript.data);




        for (int i = 0; i < NodeNumber; i++)
        {
            spheres.Add(new GameObject());
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].AddComponent<Rigidbody>();
            spheres[i].AddComponent<SphereCollider>();
            spheres[i].GetComponent<SphereCollider>().radius = spheres[i].GetComponent<SphereCollider>().radius * 3;
            spheres[i].AddComponent<MouseDrag>();
            spheres[i].AddComponent<LineRenderer>();
            spheres[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            lineRenderer = spheres[i].GetComponent<LineRenderer>();
            lineRenderer.material.color = new Color(218, 0, 100, 0.3F);
            lineRenderer.startWidth = 0.4f;
            lineRenderer.endWidth = 0.4f;

            spheres[i].transform.position = new Vector3(Random.Range(-NodeNumber, NodeNumber), Random.Range(-NodeNumber, NodeNumber), Random.Range(-NodeNumber, NodeNumber));
            spheres[i].transform.localScale = new Vector3(5, 5, 5);
            spheres[i].GetComponent<Rigidbody>().mass = 0.0001f;
            var Render = spheres[i].GetComponent<Renderer>();
            if (Random.value > 0.5)
            {
                Render.material.color = Color.clear;
                Black.Add(i);
            }
            else
            {
                Render.material.color = Color.red;
                Red.Add(i);
            }
        }
        spheres[Black[0]].GetComponent<Renderer>().material.color = Color.blue;
        spheres[Red[0]].GetComponent<Renderer>().material.color = Color.magenta;

        if (spheres.Count != 0)
        {
            print("spheres is not empty");
        }
        if (Black.Count != 0)
        {
            print("Black is not empty");
        }
        if (Red.Count != 0)
        {
            print("Red is not empty");
        }

    }

    // Update is called once per frame
    void FixedUpdate() {
        Attract();
        Repel();
        EdgeUpdate();
        SpeedRestriction();
    }

    void SpeedRestriction()
    {
        foreach (var node in spheres)
        {
            if (node.GetComponent<Rigidbody>().velocity.magnitude > MaxSpeed)
            {
                print("Restriction for speeding");
                float reduction = MaxSpeed / node.GetComponent<Rigidbody>().velocity.magnitude;
                node.GetComponent<Rigidbody>().velocity = new Vector3(node.GetComponent<Rigidbody>().velocity[1] * reduction, node.GetComponent<Rigidbody>().velocity[2] * reduction, node.GetComponent<Rigidbody>().velocity[2] * reduction);
            }
        }
    }

    void EdgeUpdate()
    {
        for (int i=1; i < Black.Count; i++)
        {
            lineRenderer = spheres[Black[i]].GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, spheres[Black[i]].GetComponent<Transform>().position);
            lineRenderer.SetPosition(1, spheres[Black[0]].GetComponent<Transform>().position);
        }
        for (int i=1; i < Red.Count; i++)
        {
            lineRenderer = spheres[Red[i]].GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, spheres[Red[i]].GetComponent<Transform>().position);
            lineRenderer.SetPosition(1, spheres[Red[0]].GetComponent<Transform>().position);
        }
    }


    void Attract ()
    {
        // trial node attraction calculation
        distance = Vector3.Distance(spheres[Red[0]].GetComponent<Rigidbody>().position, spheres[Black[0]].GetComponent<Rigidbody>().position);
        if (distance < IdealConnectedDistance * 5 + 1 && distance > IdealConnectedDistance * 5 - 1)
        {
            spheres[Red[0]].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            spheres[Black[0]].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            //spheres[Red[0]].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            print("motion stopped");
        }
        else
        {
           
            forceDirection = spheres[Black[0]].transform.position - spheres[Red[0]].transform.position;
            pullforce = Mathf.Log10(distance / (IdealConnectedDistance*5)) * PullSpeed / 10;
            spheres[Red[0]].GetComponent<Rigidbody>().AddForce((forceDirection.normalized * pullforce) / 20);
            spheres[Black[0]].GetComponent<Rigidbody>().AddForce((-forceDirection.normalized * pullforce) / 20);
        }


        //pullforce = Mathf.Log10(distance / IdealConnectedDistance) * PullSpeed;
        //AttractiveNode1.GetComponent<Rigidbody>().AddForce((forceDirection.normalized * pullforce) / 2 * Time.fixedDeltaTime);
        //AttractiveNode2.GetComponent<Rigidbody>().AddForce((-forceDirection.normalized * pullforce) / 2 * Time.fixedDeltaTime);


        // Black attraction calculation
        for (int i = 1; i<Black.Count; i++)
        {
            distance = Vector3.Distance(spheres[Black[0]].transform.position, spheres[Black[i]].transform.position);
            if (distance < IdealConnectedDistance + 1 && distance > IdealConnectedDistance - 1)
            {
                spheres[Black[i]].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                //continue;
            }
            forceDirection = spheres[Black[i]].transform.position - spheres[Black[0]].transform.position;
            pullforce = Mathf.Log10(distance / IdealConnectedDistance) * PullSpeed;
            spheres[Black[0]].GetComponent<Rigidbody>().AddForce((forceDirection.normalized * pullforce) / 2 * Time.fixedDeltaTime);
            spheres[Black[i]].GetComponent<Rigidbody>().AddForce((-forceDirection.normalized * pullforce) / 2 * Time.fixedDeltaTime);
            //possibly implement Black[0] to stop FreezePosition when all nodes are within 'reach'.
        }
 // Red attraction calculation
        for (int i = 1; i < Red.Count; i++)
        {
            distance = Vector3.Distance(spheres[Red[0]].transform.position, spheres[Red[i]].transform.position);
            if (distance < IdealConnectedDistance + 1 && distance > IdealConnectedDistance - 1)
            {
                spheres[Red[i]].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                //continue;
            }
            forceDirection = spheres[Red[i]].transform.position - spheres[Red[0]].transform.position;
            pullforce = Mathf.Log10(distance / IdealConnectedDistance) * PullSpeed;
            //spheres[Red[0]].GetComponent<Rigidbody>().AddForce((forceDirection.normalized * pullforce) / 2 * Time.fixedDeltaTime);
            spheres[Red[i]].GetComponent<Rigidbody>().AddForce((-forceDirection.normalized * pullforce) / 2 * Time.fixedDeltaTime);
        }
    }
    void Repel ()
    {
        Vector3 forceDirection = RepellentNode2.transform.position - RepellentNode1.transform.position;
        distance = Vector3.Distance(RepellentNode1.position, RepellentNode2.position);
        pushforce = Mathf.Pow(distance, -2) * PushSpeed;
        RepellentNode1.GetComponent<Rigidbody>().AddForce((-forceDirection.normalized * pushforce) / 2 * Time.fixedDeltaTime);
        RepellentNode2.GetComponent<Rigidbody>().AddForce((forceDirection.normalized * pushforce) / 2 * Time.fixedDeltaTime);

        for (int i = 1; i < Black.Count; i++)
        {
            for (int j = 1; j < Red.Count; j++)
            {
                if (i != j)
                {
                distance = Vector3.Distance(spheres[Black[i]].transform.position, spheres[Red[j]].transform.position);
                if (distance > 10 || distance == 0) continue;
                forceDirection = spheres[Red[j]].transform.position - spheres[Black[i]].transform.position;
                pushforce = Mathf.Pow(distance, -2) * PushSpeed;
                spheres[Black[i]].GetComponent<Rigidbody>().AddForce((-forceDirection.normalized * pushforce) / 2 * Time.fixedDeltaTime);
                spheres[Red[j]].GetComponent<Rigidbody>().AddForce((forceDirection.normalized * pushforce) / 2 * Time.fixedDeltaTime);
                }
            }
        }
    }
}
