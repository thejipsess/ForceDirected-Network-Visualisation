using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDirectedGraph : MonoBehaviour {
    private string[][] ParsedData;
    private string[][] ParsedConnectionData;
    private float DEGvalue;
    private LineRenderer lineRenderer;
    public float EdgeSmoothness;
    private Transform position1;
    private Transform position2;
    private Transform position3;
    public List<GameObject> spheres = new List<GameObject>();
    public List<GameObject> Edges = new List<GameObject>();
    List<int []> ExtraLinks = new List<int[]>();
    private int[,] ConnectivityMatrix;
    private int[,] LineRendererConnectivityMatrix;
    private int indexValue;
    private int IDvalue;
    private int count = 0;
    private float distance;
    public float IdealConnectedDistance;
    public float Pullspeed;
    public float MaxSpeed;
    public float MaxRepelDistance;
    public float RepelSpeed;
    public Color NegExColor;
    public Color PosExColor;
    public float EdgeDiameter; // can add functionality to change edge diamter..
    private GameObject ListOfNodesAndEdges; // use this to create hierarchy
    

    // Use this for initialization
    void Start () {


        // Load parsed data from the DataImporter and ConnectionDataImporter
        GameObject DataImportObject = GameObject.Find("DataImportObject");
        DataImporter DataImportScript = DataImportObject.GetComponent<DataImporter>();
        ConnectionDataImporter ConnectionDataImportScript = DataImportObject.GetComponent<ConnectionDataImporter>();
        ParsedData = DataImportScript.ParsedData;
        ParsedConnectionData = ConnectionDataImportScript.ParsedConnectionData;
        ConnectivityMatrix = new int[ParsedConnectionData.Length - 1, 2];
        LineRendererConnectivityMatrix = new int[ParsedConnectionData.Length-1,2];

        // create array of spheres for all identifiers in the data, also add some components
        for (int i = 1; i < ParsedData.Length; i++)
        {
            spheres.Add(new GameObject());
            spheres[i - 1] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i - 1].AddComponent<Rigidbody>();
            spheres[i - 1].AddComponent<SphereCollider>();
            spheres[i - 1].GetComponent<SphereCollider>().radius = spheres[i - 1].GetComponent<SphereCollider>().radius * 3;
            spheres[i - 1].AddComponent<MouseDrag>();
            spheres[i - 1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            spheres[i - 1].name = ParsedData[i][0];

            spheres[i - 1].transform.position = new Vector3(Random.Range(-ParsedData.Length, ParsedData.Length), Random.Range(-ParsedData.Length, ParsedData.Length), Random.Range(-ParsedData.Length, ParsedData.Length));
            spheres[i - 1].transform.localScale = new Vector3(5, 5, 5);
            spheres[i - 1].GetComponent<Rigidbody>().mass = 0.00001f;
            var Render = spheres[i - 1].GetComponent<Renderer>();
            //find DEG value and base the node color on this (make changeable by user)
            float.TryParse(ParsedData[i][3], out DEGvalue);
            if (DEGvalue > 0)
            {
                Render.material.color = PosExColor;
            }
            else if (DEGvalue < 0)
            {
                Render.material.color = NegExColor;
            }

            // compose the ConnectivityMatrix with indeces from 'spheres' instead of ID's
            for (int j = 1; j < ParsedConnectionData.Length; j++)
            {
                //print(int.TryParse(ParsedConnectionData[j][0], out IDvalue));
                if (ParsedConnectionData[j][0] == spheres[i - 1].name)
                {
                    ConnectivityMatrix[count, 0] = i - 1;
                    LineRendererConnectivityMatrix[count, 0] = i - 1;
                    int.TryParse(ParsedConnectionData[j][1], out IDvalue);
                    ConnectivityMatrix[count, 1] = IDvalue;
                    LineRendererConnectivityMatrix[count, 1] = IDvalue;

                    count++;
                }
            }
        }
        for (int j = 1; j < ParsedConnectionData.Length; j++)
        {
            for (int i = 0; i < ParsedData.Length - 1; i++)
            {
                if (ConnectivityMatrix[j - 1, 1].ToString() == spheres[i].name)
                {
                    ConnectivityMatrix[j - 1, 1] = i;
                    LineRendererConnectivityMatrix[j - 1, 1] = i;
                }
            }
        }

        //create Empty GameObject with LineRenderer for each extra required LineRenderer.Replace Source index from ConnectivityMatrix with new GameObject index.Also initilize ExtrLinks array to keep track of empty GameObjects.
        count = 0;
        
        for (int i = 0; i < ConnectivityMatrix.GetLength(0); i++)
        {
            if (spheres[ConnectivityMatrix[i, 0]].tag == "LineRendererOccupied")
            {
                spheres.Add(new GameObject());
                int[] temp = new int[2];
                temp[0] = ConnectivityMatrix[i, 0]; //assign old and new indices to new array so this can be used in Update() to assign the position of the original to the Empty GameObject.
                temp[1] = spheres.Count - 1;
                ExtraLinks.Add(temp);
                spheres[spheres.Count - 1].AddComponent<LineRenderer>();
                lineRenderer = spheres[spheres.Count - 1].GetComponent<LineRenderer>();
                lineRenderer.material.color = new Color(218, 0, 100, 0.3F);
                lineRenderer.startWidth = 0.4f;
                lineRenderer.endWidth = 0.4f;

                LineRendererConnectivityMatrix[i, 0] = spheres.Count - 1; // here we change the official sphere index in ConnectivityMatrix to the index of the new Empty GameObject with Linerenderer.
                count++; // don't think count is actually used
            }
            else
            {
                spheres[ConnectivityMatrix[i, 0]].AddComponent<LineRenderer>();
                spheres[ConnectivityMatrix[i, 0]].tag = "LineRendererOccupied";
                lineRenderer = spheres[ConnectivityMatrix[i, 0]].GetComponent<LineRenderer>();
                lineRenderer.material.color = new Color(218, 0, 100, 0.3F);
                lineRenderer.startWidth = 0.4f;
                lineRenderer.endWidth = 0.4f;
            }
        }

        for (int i = 0; i < ConnectivityMatrix.GetLength(0); i++)
        {
            //print(ConnectivityMatrix[i, 0] + " and " + ConnectivityMatrix[i, 1]);
        }

        // Create Cylinders that will act as Connectivity Edges
        for (int i = 0; i < ConnectivityMatrix.Length; i++)
        {
            Edges.Add(new GameObject());
            Edges[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Destroy(Edges[i].GetComponent<CapsuleCollider>());
            Edges[i].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            // possibly set edge color.
        }



    }


    void FixedUpdate ()
    {


        int[,] DrawnEdges = new int[ConnectivityMatrix.GetLength(0),2];
        for (int i = 0; i < ConnectivityMatrix.GetLength(0); i++)
        {
            //if (ConnectivityMatrix[i,0] == ConnectivityMatrix[i, 1])
            //{
                //BezierCurveLinerendererupdate(i);
                //continue;
            //}
            // following nested loop for checking overlap is too complex to do in real-time!!!
            //   for (int j = 0; j < DrawnEdges.GetLength(0); j++)
            //   {
            //       print("i = " + i + " and j = " + j);
            //
            //       if (ConnectivityMatrix[i,0] == DrawnEdges[j,0] && ConnectivityMatrix[i,1] == DrawnEdges[j, 1])
            //       {
            //           //BezierCurveLinerendererupdate(i);
            //           continue;
            //       }
            //   }

            
            EdgeUpdate(i);
            DrawnEdges[i, 0] = ConnectivityMatrix[i, 0];
            DrawnEdges[i, 1] = ConnectivityMatrix[i, 1];
        }

        Attract();
        Repel();
        SpeedRestriction();
        Connect();
    }

    private void Connect()
    {
        for (int i = 0; i < ConnectivityMatrix.GetLength(0); i++)
        {
            var NodeStart = spheres[ConnectivityMatrix[i, 0]].transform;
            var NodeEnd = spheres[ConnectivityMatrix[i, 1]].transform;
            var edge = transform.localScale;
            edge.x = (NodeEnd.localScale.x + NodeStart.localScale.x) / 20;
            edge.z = (NodeEnd.localScale.z + NodeStart.localScale.z) / 20;
            edge.y = (NodeEnd.position - NodeStart.position).magnitude / 2;
            Edges[i].transform.position = (NodeEnd.position - NodeStart.position) / 2.0f + NodeStart.position;
            Edges[i].transform.localScale = edge;
            Edges[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, NodeEnd.position - NodeStart.position);
        }
    }

    //calculate Attractive force for all connected nodes  -- Since Repel goes through all the nodes, we use this loop to change the color as well
    void Attract()
    {
        for (int i = 0; i < ConnectivityMatrix.GetLength(0); i++)
        {
            if (ConnectivityMatrix[i,0] != ConnectivityMatrix[i, 1])
            {
                int index1 = ConnectivityMatrix[i, 0];
                int index2 = ConnectivityMatrix[i, 1];
                distance = Vector3.Distance(spheres[index1].GetComponent<Rigidbody>().position, spheres[index2].GetComponent<Rigidbody>().position);
                Vector3 ForceDirection = spheres[index2].GetComponent<Rigidbody>().position - spheres[index1].GetComponent<Rigidbody>().position;
                float pullforce = Mathf.Log10(distance / (IdealConnectedDistance)) * Pullspeed;
                spheres[index1].GetComponent<Rigidbody>().AddForce((ForceDirection.normalized * pullforce) / 2 * Time.fixedDeltaTime);
                spheres[index2].GetComponent<Rigidbody>().AddForce((-ForceDirection.normalized * pullforce) / 2 * Time.fixedDeltaTime);
            }
        }
    }

    //calculate repulsive force between all nodes within a certain distance
    void Repel()
    {

        for (int i = 0; i < ParsedData.Length-1; i++)
        {
            for (int j = 0; j< ParsedData.Length-1; j++)
            {
                if (i == j) continue;
                distance = Vector3.Distance(spheres[i].transform.position, spheres[j].transform.position);
                if (distance > MaxRepelDistance) continue;
                Vector3 forceDirection = spheres[j].transform.position - spheres[i].transform.position;
                float pushForce = (-Mathf.Pow(IdealConnectedDistance, 2) / distance) * RepelSpeed;
                //float pushForce = Mathf.Pow(distance, -2) * RepelSpeed;
                spheres[i].GetComponent<Rigidbody>().AddForce((-forceDirection.normalized * pushForce) / 2 * Time.fixedDeltaTime);
                spheres[j].GetComponent<Rigidbody>().AddForce((forceDirection.normalized * pushForce) / 2 * Time.fixedDeltaTime);

                
            }
        }
    }

    void SpeedRestriction()
    {
        for (int i = 1; i < ParsedData.Length; i++)
        {
            if (spheres[i-1].GetComponent<Rigidbody>().velocity.magnitude > MaxSpeed)
            {
                //print("restricting speed, speed is now: " + spheres[i - 1].GetComponent<Rigidbody>().velocity.magnitude);
                float reduction = MaxSpeed / spheres[i - 1].GetComponent<Rigidbody>().velocity.magnitude;
                spheres[i - 1].GetComponent<Rigidbody>().velocity = new Vector3(spheres[i - 1].GetComponent<Rigidbody>().velocity[1] * reduction, spheres[i - 1].GetComponent<Rigidbody>().velocity[2] * reduction, spheres[i - 1].GetComponent<Rigidbody>().velocity[2] * reduction);
                //print("after speed correction, speed is: " + spheres[i - 1].GetComponent<Rigidbody>().velocity.magnitude);
            }
        }
    }

    //genereate edges between connected nodes
    void EdgeUpdate(int index)
    {
        lineRenderer = spheres[LineRendererConnectivityMatrix[index, 0]].GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, spheres[ConnectivityMatrix[index, 0]].GetComponent<Transform>().position);
        lineRenderer.SetPosition(1, spheres[ConnectivityMatrix[index, 1]].GetComponent<Transform>().position);
    }

    //unused function for creating curved edges in case of duplicate/self-regulatory connections
    void BezierCurveLinerendererupdate(int index)
    {
        // point 2 is exactly on the line between point 1 & 3. this should be changed to create an arc effect
        var pointlist = new List<Vector3>();
            for (float ratio = 0; ratio <= 1; ratio += 1.0f / EdgeSmoothness)
            {
                var TangentLineVertex1 = Vector3.Lerp(spheres[ConnectivityMatrix[index, 0]].transform.position, (spheres[ConnectivityMatrix[index, 0]].transform.position+ spheres[ConnectivityMatrix[index, 1]].transform.position)/2, ratio);
                var TangentLineVertex2 = Vector3.Lerp((spheres[ConnectivityMatrix[index, 0]].transform.position + spheres[ConnectivityMatrix[index, 1]].transform.position) / 2, spheres[ConnectivityMatrix[index, 1]].transform.position, ratio);
                var bezierpoint = Vector3.Lerp(TangentLineVertex1, TangentLineVertex2, ratio);
                pointlist.Add(bezierpoint);
            }
            lineRenderer = spheres[ConnectivityMatrix[index, 0]].GetComponent<LineRenderer>();
            lineRenderer.positionCount = pointlist.Count;
            lineRenderer.SetPositions(pointlist.ToArray());
        
    }


}
