using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour {

    private Texture2D CrossHairPic ;
    private Texture2D copyCrossHairPic;

    // Use this for initialization
    void Start ()
    {
        CrossHairPic = Resources.Load("CustomCrossHair") as Texture2D;
        copyCrossHairPic = Instantiate(CrossHairPic);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        float xMin = (Screen.width / 2) - (copyCrossHairPic.width / 2);
        float yMin = (Screen.height / 2) - (copyCrossHairPic.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, CrossHairPic.width, CrossHairPic.height), CrossHairPic);
    }
}
