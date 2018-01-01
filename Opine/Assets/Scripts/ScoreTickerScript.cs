using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTickerScript : MonoBehaviour {

    public bool isOpponent;

    public int speed; 

    int targetScore, currentScore;

    string subtitle; 
    

	// Use this for initialization
	void Start () {
        if (isOpponent) currentScore = FetchFullGameTopics.opponentScore;
        else currentScore = FetchFullGameTopics.myScore;

        if (FetchFullGameTopics.versus)
        {
            subtitle = (isOpponent ? "OPPONENT" : "YOU"); 
        } else
        {
            subtitle = "SCORE"; // this means the opponent display will be called score too, even though it's not on screen
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isOpponent) targetScore = FetchFullGameTopics.opponentScore;
        else targetScore = FetchFullGameTopics.myScore;

        if (targetScore > currentScore)
        {
            currentScore += speed;
            currentScore = Mathf.Clamp(currentScore, 0, targetScore);
            GetComponent<TextMesh>().text = subtitle + "\n" + currentScore.ToString(); 
        }
	}
}
