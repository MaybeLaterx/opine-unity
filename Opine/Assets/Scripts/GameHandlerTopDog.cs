﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class GameHandlerTopDog : MonoBehaviour {

    public string nextLevel;


    int round, question;
    bool readyForNext;
    JSONNode json;

    int totalQuestions; 

    public Transform cardPrefab, textPrefab;
    public Transform indicator;
    private Transform controller; 
    Transform[] cards; 

    public Sprite sprite1, sprite2; 

    Transform[] indicators;
    GameObject title, description;
    GameObject[] buttons; 

	// Use this for initialization
	void Start () {
        round = 1; // 0 indexed; 
        question = 0;

        controller = GameObject.FindGameObjectWithTag("GameController").transform;

        // Parse data and prepare initial swiper creation
        string jsonString = FetchFullGameTopics.www.text;
        //string jsonString = "{\"success\":true,\"data\":[[[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667}],[{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364}],[{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257}],[{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327}],[{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"elephants\",\"rating\":0.6986486486486486,\"likes\":1034,\"dislikes\":446}],[{\"id\":\"yachts\",\"rating\":0.2879690538248314,\"likes\":13921,\"dislikes\":34421}],[{\"id\":\"journals\",\"rating\":0.29131989623483007,\"likes\":106797,\"dislikes\":259800}],[{\"id\":\"beavers\",\"rating\":0.20733944954128442,\"likes\":113,\"dislikes\":432}],[{\"id\":\"biscuits\",\"rating\":0.7298632218844985,\"likes\":1921,\"dislikes\":711}]],[[{\"id\":\"yoghurts\",\"rating\":0.7269151478311769,\"likes\":678057,\"dislikes\":254730},{\"id\":\"gerbils\",\"rating\":0.99366921656555,\"likes\":11301,\"dislikes\":72}],[{\"id\":\"panthers\",\"rating\":0.6368086147821831,\"likes\":1301,\"dislikes\":742},{\"id\":\"maggots\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211}],[{\"id\":\"cats\",\"rating\":0.7541899441340782,\"likes\":135,\"dislikes\":44},{\"id\":\"superheroes\",\"rating\":0.5942489666603722,\"likes\":56645,\"dislikes\":38677}],[{\"id\":\"cheese\",\"rating\":0.5339358685215372,\"likes\":95645,\"dislikes\":83487},{\"id\":\"radios\",\"rating\":0.7260589303729257,\"likes\":674357,\"dislikes\":254434}],[{\"id\":\"gravy\",\"rating\":0.6360566134405463,\"likes\":568585,\"dislikes\":325337},{\"id\":\"uni\",\"rating\":0.724858696811347,\"likes\":6797,\"dislikes\":2580}],[{\"id\":\"vanilla\",\"rating\":0.6375423557513622,\"likes\":586655,\"dislikes\":333527},{\"id\":\"noodles\",\"rating\":0.7169585987261147,\"likes\":1801,\"dislikes\":711}],[{\"id\":\"salt\",\"rating\":0.7217311541027351,\"likes\":8655,\"dislikes\":3337},{\"id\":\"cigarettes\",\"rating\":0.6425592625109745,\"likes\":5855,\"dislikes\":3257}],[{\"id\":\"pyjamas\",\"rating\":0.9359046608764835,\"likes\":56465,\"dislikes\":3867},{\"id\":\"deserts\",\"rating\":0.6359325218876215,\"likes\":56825685,\"dislikes\":32532357}],[{\"id\":\"pickles\",\"rating\":0.5339995533720411,\"likes\":9565,\"dislikes\":8347},{\"id\":\"leopards\",\"rating\":0.20764424843807425,\"likes\":1130,\"dislikes\":4312}],[{\"id\":\"beess\",\"rating\":0.4893324156916724,\"likes\":711,\"dislikes\":742},{\"id\":\"santa\",\"rating\":0.9229403868456543,\"likes\":4123921,\"dislikes\":344321}]],[[{\"id\":\"june\",\"rating\":0.6414473684210527,\"likes\":585,\"dislikes\":327},{\"id\":\"fleas\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211},{\"id\":\"lollipops\",\"rating\":0.7259781237122767,\"likes\":67407357,\"dislikes\":25443040},{\"id\":\"chips\",\"rating\":0.5329241071428571,\"likes\":955,\"dislikes\":837}],[{\"id\":\"ladybirds\",\"rating\":0.6360188831215321,\"likes\":56855,\"dislikes\":32537},{\"id\":\"cockrels\",\"rating\":0.14956109188421077,\"likes\":13051,\"dislikes\":74211},{\"id\":\"cows\",\"rating\":0.01342549514821215,\"likes\":101,\"dislikes\":7422},{\"id\":\"buildings\",\"rating\":0.7267935893298914,\"likes\":6757,\"dislikes\":2540}],[{\"id\":\"chocolate\",\"rating\":0.9478433098591549,\"likes\":12921,\"dislikes\":711},{\"id\":\"oceans\",\"rating\":0.6785849894934803,\"likes\":682685,\"dislikes\":323357},{\"id\":\"flamingos\",\"rating\":0.001996007984031936,\"likes\":2,\"dislikes\":1000},{\"id\":\"clocks\",\"rating\":0.6024590163934426,\"likes\":5586,\"dislikes\":3686}]],[[{\"id\":\"rainbows\",\"rating\":0.9892053284336243,\"likes\":12921,\"dislikes\":141}],[{\"id\":\"pencils\",\"rating\":0.7901785714285714,\"likes\":12921,\"dislikes\":3431}],[{\"id\":\"mice\",\"rating\":0.5934873949579832,\"likes\":565,\"dislikes\":387}],[{\"id\":\"houses\",\"rating\":0.7303128371089536,\"likes\":677,\"dislikes\":250}],[{\"id\":\"money\",\"rating\":0.5328576521177283,\"likes\":95665,\"dislikes\":83867}]]]}";
        json = JSON.Parse(jsonString);
        totalQuestions = json["data"][round].Count;

        // Create initial cards
            //print(json["data"][round][question][0]["id"]);
            //print(json["data"][round][question][1]["id"]);
        cards = UtilitiesScript.CreateTopDogCards(cardPrefab, textPrefab, json["data"][round][question][0]["id"], sprite1, json["data"][round][question][1]["id"], sprite2);
        ConfirmTopDog.cards = cards; 

        // Create initial indicators based on batch size
        indicators = UtilitiesScript.CreateIndicators(round, indicator);
        ConfirmTopDog.indicators = indicators;

        // Transition title and description
        title = GameObject.FindGameObjectWithTag("Title");
        description = GameObject.FindGameObjectWithTag("Description");
        title.GetComponent<Ease>().alignmentX = 0f;
        description.GetComponent<Ease>().alignmentX = 0f;

        // Transition buttons 
        buttons = GameObject.FindGameObjectsWithTag("BottomButton");
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Ease>().alignmentY = -8.6f;
        }
    }


    public bool UpdateIndicators(string boxType) // Returns whether the game is over
    {
        // Update current indicator
        Transform thisLight = indicators[question % totalQuestions];
        thisLight.GetComponent<IndicatorColour>().current = false;

        // Skipping or answering?
        if (boxType == "skip") print("Skipping!");
        else
        {
            print("Answers selected! Saving!");
            thisLight.GetComponent<IndicatorColour>().answered = true;

        }
        if (boxType != "skip") thisLight.GetComponent<IndicatorColour>().myAnswers = new string[] { boxType }; // "1" or "2"
        thisLight.GetComponent<IndicatorColour>().shouldUpdate = true;

        // Find next indicator and update
        int i = 0;
        do
        {
            i++;
            if (i > totalQuestions) // every question is answered
            {
                // Load next scene 
                print("Pecking Order round complete! Saving answers");
                string[] compiledAnswers = new string[totalQuestions];
                //Sprite[] compiledColours = new Sprite[totalQuestions];
                for (int j = 0; j < totalQuestions; j++) // each batch
                {
                    compiledAnswers[j] = indicators[j].GetComponent<IndicatorColour>().myAnswers[0];
                }
                FetchFullGameTopics.round2 = compiledAnswers;
                //FetchFullGameTopics.round3Colours = compiledColours;
                EndRound(); 
                return true;
            }
            question = (question + 1) % totalQuestions;
        } while (indicators[question].GetComponent<IndicatorColour>().answered == true);
        Transform nextLight = indicators[question];
        nextLight.GetComponent<IndicatorColour>().current = true;

        return false;
    }

    public void InputAnswer(string response)            // send answer, update indicators, move both cards off, force transition wait
    {
        print("Answer chosen: " + response);

        // Move cards away
        cards[0].GetComponent<Ease>().alignmentX = -12f;
        cards[1].GetComponent<Ease>().alignmentX = 12.1f;
        StartCoroutine(DeleteCards(0.5f)); 

        // Update lights 
        foreach (Transform indicator in indicators)
        {
            indicator.GetComponent<IndicatorColour>().shouldUpdate = true;
        }

        // tell controller to update lights and get next
        bool isGameOver = UpdateIndicators(response); //"1", "2" or "skip"          question++ happens in this script
        if (!isGameOver) StartCoroutine(CreateCards(cardPrefab, textPrefab, json["data"][round][question][0]["id"], sprite1, json["data"][round][question][1]["id"], sprite2, 0.6f));

        // update transition timers (internal cooldown)
        ConfirmTopDog.transitioning = true;
        StartCoroutine(ConfirmTopDog.TransitionTimer());

    }

    IEnumerator CreateCards(Transform cardPrefab, Transform textPrefab, string topic1, Sprite sprite1, string topic2, Sprite sprite2, float delay)
    {
        yield return new WaitForSeconds(delay); 
        cards = UtilitiesScript.CreateTopDogCards(cardPrefab, textPrefab, topic1, sprite1, topic2, sprite2);
    }

    IEnumerator DeleteCards(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (GameObject _card in GameObject.FindGameObjectsWithTag("TopDogCard")) 
        {
            Destroy(_card); 
        }
    }

    void EndRound()
    {
        foreach (Transform indicator in indicators)
        {
            indicator.GetComponent<Ease>().alignmentY = 14f;
        }

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Ease>().alignmentY = -11.5f;
        }

        title.GetComponent<Ease>().alignmentX = 16f;
        description.GetComponent<Ease>().alignmentX = 16f;

        StartCoroutine(LoadLevelDelay(nextLevel, 0.75f));
    }

    IEnumerator LoadLevelDelay(string nextLevel, float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(nextLevel);
    }

}
