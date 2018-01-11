using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class TimeScript : MonoBehaviour {

    Image image;
    float startTime, endTime, timeRemaining; 
    public float roundDuration;
    public Transform textPrefab;
    public Sprite redRing;
    public Transform controller;
    bool ended;
    string levelName;

    float timePerQuestion; 

	// Use this for initialization
	void Start () {
        levelName = SceneManager.GetActiveScene().name;
        string jsonString = (levelName == "S_VotingTime" ? FetchFullGameTopics.wwwVText : FetchFullGameTopics.wwwText);
        JSONNode json = JSON.Parse(jsonString);

        int round = FetchFullGameTopics.round;
        if (levelName == "S_VotingTime")
        {
            round--;
            print("Round on time script = " + round);
            
        }

        switch (levelName)
        {
            case "S_YayOrNay": timePerQuestion = 5f; break;
            case "S_TopDog": timePerQuestion = 7f; break;
            case "S_PeckingOrder": timePerQuestion = 10f; break;
            case "S_PinpointOpinion": timePerQuestion = 5f; break;
            case "S_VotingTime": timePerQuestion = 7f; break;
            default: print("Time up level error"); break;
        }
        int totalQuestions = (levelName == "S_VotingTime" ? json["data"]["topics"][round].Count : json["data"][round].Count);
        roundDuration = (float) totalQuestions * timePerQuestion; 

        image = GetComponent<Image>();
        startTime = Time.time; 
        endTime = Time.time + roundDuration;


        GetComponent<Ease>().alignmentX = -35; 
    }
	
	// Update is called once per frame
	void Update () {
        if (endTime < Time.time && !ended) // Round is over!
        {
            switch (levelName)
            {
                case "S_YayOrNay": controller.GetComponent<GameHandlerYayOrNay>().TimeUp(); break;
                case "S_TopDog": controller.GetComponent<GameHandlerTopDog>().TimeUp(); break;
                case "S_PeckingOrder": controller.GetComponent<GameHandlerPeckingOrder>().TimeUp(); break;
                case "S_PinpointOpinion": controller.GetComponent<GameHandlerPinpointOpinion>().TimeUp(); break;
                case "S_VotingTime": controller.GetComponent<GameHandlerVoting>().TimeUp(); break; 
                default: print("Time up level error"); break; 
            }
            ended = true;

            GetComponent<Ease>().alignmentX = 35; 



        }

        timeRemaining = Mathf.Max(endTime - Time.time, 0);
        if (timeRemaining < 11f) GetComponent<Image>().sprite = redRing; 
        textPrefab.GetComponent<Text>().text = Mathf.FloorToInt(timeRemaining).ToString();

        float percentLeft = (Time.time - startTime) / (endTime - startTime);
        
        image.fillAmount = Mathf.Clamp01(1f - percentLeft);
        

	}
}
