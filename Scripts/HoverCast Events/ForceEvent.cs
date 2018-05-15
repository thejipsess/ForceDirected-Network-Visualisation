using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hover.Core.Items.Types;


public class ForceEvent : MonoBehaviour
{


    public void OnValueChangeAttr(HoverItemDataSlider slider)
    {
        GameObject ForceDir = GameObject.Find("ForceDirectedGrapher");
        ForceDirectedGraph ForceDirScript = ForceDir.GetComponent<ForceDirectedGraph>();
        ForceDirScript.Pullspeed = slider.RangeValue;
    }

    public void OnValueChangeRepel(HoverItemDataSlider slider)
    {
        GameObject ForceDir = GameObject.Find("ForceDirectedGrapher");
        ForceDirectedGraph ForceDirScript = ForceDir.GetComponent<ForceDirectedGraph>();
        ForceDirScript.RepelSpeed = slider.RangeValue;
    }

    public void OnValueChangeRadius(HoverItemDataSlider slider)
    {
        GameObject ForceDir = GameObject.Find("ForceDirectedGrapher");
        ForceDirectedGraph ForceDirScript = ForceDir.GetComponent<ForceDirectedGraph>();
        ForceDirScript.MaxRepelDistance = slider.RangeValue;
    }
}
