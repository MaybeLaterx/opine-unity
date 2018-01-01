using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuestionMarks : MonoBehaviour {

    public Transform qMark;
    private float timer = 0;
    public float minTimer = 2;
    public float maxTimer = 3; 

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timer <= 0)
        {
            timer = Random.Range(minTimer,maxTimer);
            Instantiate(qMark, new Vector3(0, 0, 0), Quaternion.identity);
        }

        timer -= Time.deltaTime ; 
        
	}
}
