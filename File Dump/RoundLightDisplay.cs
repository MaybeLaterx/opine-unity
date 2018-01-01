using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class RoundLightDisplay : MonoBehaviour {

    public int totalBatches;
    public int currentBatch = 0; 
    int round = 2; 
    public float gap; 
    public Transform[] displays;
    public Transform display;

    bool waitingForData = true;
    JSONNode json; 

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (HttpGetExample.waiting == false && waitingForData) // wait to return http request 
        {
            json = JSON.Parse(HttpGetExample.www.text);
            waitingForData = false;
            
        }


    }
}
