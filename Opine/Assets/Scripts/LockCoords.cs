using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCoords : MonoBehaviour {

    public Transform anchor; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (anchor == null) Destroy(gameObject);
        else
        {
            transform.position = new Vector3(anchor.position.x, anchor.position.y, anchor.position.z - 0.5f);
            transform.rotation = anchor.rotation; 
        }
	}
}
