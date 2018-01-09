using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour {

    public static string uuid;

    public static string domain; 
    public static string apiVersion; 

	// Use this for initialization
	void Start () {
        domain = "http://104.131.63.157:3000/api/opine"; // "https://maybelatergames.co.uk/api/opine";
        apiVersion = "1.0.0"; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
