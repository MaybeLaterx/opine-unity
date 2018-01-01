using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmVote : MonoBehaviour {

    static public Transform controller;
    static public float secsToWait;
    static public bool transitioning;
    static public bool shouldTransition; 
    public string boxType; 

	// Use this for initialization
	void Start () {
        transitioning = true;
        StartCoroutine(WaitTime());
        controller = GameObject.FindGameObjectWithTag("GameController").transform;
        secsToWait = 0.75f; 
	}

    static public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(secsToWait);
        transitioning = false;
    }

    void OnMouseUp() // on release
    {
        if (!transitioning) controller.GetComponent<GameHandlerVoting>().Trigger(boxType);
    }

    // Update is called once per frame
    void Update () {
		if (shouldTransition)
        {
            transitioning = true; 
            StartCoroutine(WaitTime());
            shouldTransition = false; 
        }
	}
}
