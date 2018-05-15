using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hover.Core.Items.Types;


public class EdgeLengthEvent : MonoBehaviour
{


    public void OnValueChange(HoverItemDataSlider slider)
    {
        GameObject Grapher = GameObject.Find("ForceDirectedGrapher");
        ForceDirectedGraph GrapherScript = Grapher.GetComponent<ForceDirectedGraph>();
        GrapherScript.IdealConnectedDistance = slider.RangeValue;
    }

}
