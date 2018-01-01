using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class ConfirmOrder : MonoBehaviour {

    public Transform controller;
    public float secsToWait; 
    static private bool transitioning;
    public bool isSkip;
    GameObject[] boxes;

    // Use this for initialization
    void Start () {
        transitioning = true;
        StartCoroutine(WaitTime());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(secsToWait);
        transitioning = false; 
    }

    void OnMouseUp() // on release
    {
        if (!transitioning)
        {
            // Update lights 
            foreach (GameObject indicator in GameObject.FindGameObjectsWithTag("Indicator"))
            {
                indicator.GetComponent<IndicatorColour>().shouldUpdate = true;
            }

            // tell controller to update lights and get next
            bool isGameOver = controller.GetComponent<GameHandlerPeckingOrder>().UpdateIndicators(!isSkip);
            if (!isGameOver) controller.GetComponent<GameHandlerPeckingOrder>().MakeNextCards();

            // tell boxes to disappear
            boxes = GameObject.FindGameObjectsWithTag("OrderBox");
            foreach (GameObject box in boxes)
            {
                if (isSkip) box.GetComponent<OrderBoxHeight>().endX *= -1;
                box.GetComponent<OrderBoxHeight>().answerGiven = true;
            }

            // update self (internal cooldown)
            transitioning = true; 
            StartCoroutine(WaitTime()); 
        }
    }
}
