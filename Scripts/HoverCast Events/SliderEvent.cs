using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hover.Core.Items.Types;


public class SliderEvent : MonoBehaviour {


    public void OnValueChangeNeg (HoverItemDataSlider slider) {
            GameObject DataImportObject = GameObject.Find("DataImportObject");
            DataImporter DataImportScript = DataImportObject.GetComponent<DataImporter>();
            GameObject ForceDir = GameObject.Find("ForceDirectedGrapher");
            ForceDirectedGraph ForceDirScript = ForceDir.GetComponent<ForceDirectedGraph>();
            Color NegCol = Color.HSVToRGB(slider.RangeValue, 1, 1);
            string[][] ParsedData = DataImportScript.ParsedData;
            float DEGvalue;
            List<GameObject> spheres = ForceDirScript.spheres;

            for (int i =0; i < ParsedData.GetLength(0) - 1; i++)
            {
                float.TryParse(ParsedData[i+1][3], out DEGvalue);
                if (DEGvalue < 0)
                {
                    spheres[i].GetComponent<Renderer>().material.color = NegCol;
                }
            }
	}

    public void OnValueChangePos (HoverItemDataSlider slider)
    {
        GameObject DataImportObject = GameObject.Find("DataImportObject");
        DataImporter DataImportScript = DataImportObject.GetComponent<DataImporter>();
        GameObject ForceDir = GameObject.Find("ForceDirectedGrapher");
        ForceDirectedGraph ForceDirScript = ForceDir.GetComponent<ForceDirectedGraph>();
        Color NegCol = Color.HSVToRGB(slider.RangeValue, 1, 1);
        string[][] ParsedData = DataImportScript.ParsedData;
        float DEGvalue;
        List<GameObject> spheres = ForceDirScript.spheres;

        for (int i = 0; i < ParsedData.GetLength(0) - 1; i++)
        {
            float.TryParse(ParsedData[i + 1][3], out DEGvalue);
            if (DEGvalue > 0)
            {
                spheres[i].GetComponent<Renderer>().material.color = NegCol;
            }
        }
    }
}
