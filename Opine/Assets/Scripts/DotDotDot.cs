using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDotDot : MonoBehaviour {

    public string baseText;
    public float delayTime;
    float endTime; 

	// Use this for initialization
	void Start () {
        baseText = GetComponent<TextMesh>().text;
        endTime = Time.time + delayTime; 
	}
	
	// Update is called once per frame
	void Update () {
        if (endTime < Time.time)
        {
            endTime = Time.time + delayTime; 
            string thisText = GetComponent<TextMesh>().text;
            thisText += ".";
            if (thisText.EndsWith("...."))
            {
                GetComponent<TextMesh>().text = baseText;
            }
            else
            {
                GetComponent<TextMesh>().text = thisText;
            }
        }
        
	}
}
