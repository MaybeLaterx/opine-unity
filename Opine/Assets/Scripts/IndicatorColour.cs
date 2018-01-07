using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorColour : MonoBehaviour {

    public bool answered;
    public bool current;

    public string[] myAnswers;
    public string[] opponentAnswers;
    public string[] realAnswers; 
    public Sprite[] colours = new Sprite[4];

    public Sprite answeredSprite;
    public Sprite currentSprite;
    public Sprite unansweredSprite;
    public Sprite timeUpSprite; 

    public bool shouldUpdate;

    public int iteration; // What i value was when it was created

	// Use this for initialization
	void Start () {
        answered = false;
        shouldUpdate = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (shouldUpdate)
        {
            Sprite newSprite;
            if (myAnswers == new string[] { "unanswered" }) newSprite = timeUpSprite;
            if (answered) newSprite = answeredSprite;
            else if (current) newSprite = currentSprite;
            else newSprite = unansweredSprite;

            GetComponent<SpriteRenderer>().sprite = newSprite;
            shouldUpdate = false;
        }
	}
}
