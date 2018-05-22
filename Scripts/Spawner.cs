using UnityEngine;
using UnityEditor;

public class Spawner : MonoBehaviour
{
    [MenuItem("GameObject/Create Empty Child #&n")]
    private void Start()
    {
        BringMeBackFromTheEdgeOfMadness();
        print("started");

    }
    public static void BringMeBackFromTheEdgeOfMadness()
    {
        print("doing some bad parenting");
        GameObject go = new GameObject("ogogogogogogogogo");
        if (Selection.activeTransform != null)
            go.transform.parent = Selection.activeTransform;
    }
}