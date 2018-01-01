using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour {

    [SerializeField] int rSpeed = 100;
    float ranSpeed = 100; 

	// Use this for initialization
	void Start () {
        do
        {
            ranSpeed = Random.Range(-100f, 100f);
            print(ranSpeed.ToString());
        } while (Mathf.Abs(ranSpeed) < 50);

    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.back * Time.deltaTime * rSpeed);
	}
}
