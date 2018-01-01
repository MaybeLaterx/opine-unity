using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class TopDogCard : MonoBehaviour {

    public string topic;
    public string boxType; // "1" or "2"

    public Transform controller; 

	// Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene().name != "S_TopDog")
        {
            Destroy(this); // destroy script from instance
        }
        else
        {
            controller = GameObject.FindGameObjectWithTag("GameController").transform;
        }
    }

    // Release
    private void OnMouseUp()
    {
        controller.GetComponent<GameHandlerTopDog>().InputAnswer(boxType); 
    }


    // Update is called once per frame
    void Update () {
		
	}
}
