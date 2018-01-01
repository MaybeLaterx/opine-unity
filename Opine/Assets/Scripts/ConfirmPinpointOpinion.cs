using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPinpointOpinion : MonoBehaviour {

    static public bool transitioning; 

    public bool isSkip; 
    static float delay = 0.75f;

    Transform controller;

    // Use this for initialization
    void Start () {
        transitioning = true;
        StartCoroutine(WaitTime());
        controller = GameObject.FindGameObjectWithTag("GameController").transform;
        delay = 0.75f;
    }

    static public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(delay);
        transitioning = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    static public IEnumerator TransitionTimer()
    {
        print("timer started");
        yield return new WaitForSeconds(delay);
        transitioning = false;
    }

    private void OnMouseUp()
    {
        if (!transitioning)
        {
            // Update transition timers 
            transitioning = true;
            StartCoroutine(TransitionTimer());

            // Send answer
            controller.GetComponent<GameHandlerPinpointOpinion>().InputAnswer(isSkip);
        }
    }
}
