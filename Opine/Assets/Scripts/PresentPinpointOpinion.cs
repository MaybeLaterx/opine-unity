using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PresentPinpointOpinion : MonoBehaviour {

    public string nextLevel;

    int round, question;
    public bool versus;

    public Transform cardPrefab, topicPrefab, starPrefab, indicatorPrefab, graphPrefab;
    public Sprite platinumStar, goldStar, silverStar, bronzeStar, cross;
    public Sprite myColour, opponentColour, realColour;

    public float enterTime = 2f, myAnswerTime = 2f, opponentAnswerTime = 2f, realAnswerTime = 2f, starTime = 2f, moveTime = 2f, endTransitionTime = 2f;

    // Use this for initialization
    void Start () {
        round = 3;
        //versus = FetchFullGameTopics.versus;
        versus = true; 

        //string jsonString = FetchFullGameTopics.www.text; 
        string jsonString = "{\"success\":true,\"data\":[[[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667}],[{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364}],[{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257}],[{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327}],[{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"elephants\",\"rating\":0.6986486486486486,\"likes\":1034,\"dislikes\":446}],[{\"id\":\"yachts\",\"rating\":0.2879690538248314,\"likes\":13921,\"dislikes\":34421}],[{\"id\":\"journals\",\"rating\":0.29131989623483007,\"likes\":106797,\"dislikes\":259800}],[{\"id\":\"beavers\",\"rating\":0.20733944954128442,\"likes\":113,\"dislikes\":432}],[{\"id\":\"biscuits\",\"rating\":0.7298632218844985,\"likes\":1921,\"dislikes\":711}]],[[{\"id\":\"yoghurts\",\"rating\":0.7269151478311769,\"likes\":678057,\"dislikes\":254730},{\"id\":\"gerbils\",\"rating\":0.99366921656555,\"likes\":11301,\"dislikes\":72}],[{\"id\":\"panthers\",\"rating\":0.6368086147821831,\"likes\":1301,\"dislikes\":742},{\"id\":\"maggots\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211}],[{\"id\":\"cats\",\"rating\":0.7541899441340782,\"likes\":135,\"dislikes\":44},{\"id\":\"superheroes\",\"rating\":0.5942489666603722,\"likes\":56645,\"dislikes\":38677}],[{\"id\":\"cheese\",\"rating\":0.5339358685215372,\"likes\":95645,\"dislikes\":83487},{\"id\":\"radios\",\"rating\":0.7260589303729257,\"likes\":674357,\"dislikes\":254434}],[{\"id\":\"gravy\",\"rating\":0.6360566134405463,\"likes\":568585,\"dislikes\":325337},{\"id\":\"uni\",\"rating\":0.724858696811347,\"likes\":6797,\"dislikes\":2580}],[{\"id\":\"vanilla\",\"rating\":0.6375423557513622,\"likes\":586655,\"dislikes\":333527},{\"id\":\"noodles\",\"rating\":0.7169585987261147,\"likes\":1801,\"dislikes\":711}],[{\"id\":\"salt\",\"rating\":0.7217311541027351,\"likes\":8655,\"dislikes\":3337},{\"id\":\"cigarettes\",\"rating\":0.6425592625109745,\"likes\":5855,\"dislikes\":3257}],[{\"id\":\"pyjamas\",\"rating\":0.9359046608764835,\"likes\":56465,\"dislikes\":3867},{\"id\":\"deserts\",\"rating\":0.6359325218876215,\"likes\":56825685,\"dislikes\":32532357}],[{\"id\":\"pickles\",\"rating\":0.5339995533720411,\"likes\":9565,\"dislikes\":8347},{\"id\":\"leopards\",\"rating\":0.20764424843807425,\"likes\":1130,\"dislikes\":4312}],[{\"id\":\"beess\",\"rating\":0.4893324156916724,\"likes\":711,\"dislikes\":742},{\"id\":\"santa\",\"rating\":0.9229403868456543,\"likes\":4123921,\"dislikes\":344321}]],[[{\"id\":\"june\",\"rating\":0.6414473684210527,\"likes\":585,\"dislikes\":327},{\"id\":\"fleas\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211},{\"id\":\"lollipops\",\"rating\":0.7259781237122767,\"likes\":67407357,\"dislikes\":25443040},{\"id\":\"chips\",\"rating\":0.5329241071428571,\"likes\":955,\"dislikes\":837}],[{\"id\":\"ladybirds\",\"rating\":0.6360188831215321,\"likes\":56855,\"dislikes\":32537},{\"id\":\"cockrels\",\"rating\":0.14956109188421077,\"likes\":13051,\"dislikes\":74211},{\"id\":\"cows\",\"rating\":0.01342549514821215,\"likes\":101,\"dislikes\":7422},{\"id\":\"buildings\",\"rating\":0.7267935893298914,\"likes\":6757,\"dislikes\":2540}],[{\"id\":\"chocolate\",\"rating\":0.9478433098591549,\"likes\":12921,\"dislikes\":711},{\"id\":\"oceans\",\"rating\":0.6785849894934803,\"likes\":682685,\"dislikes\":323357},{\"id\":\"flamingos\",\"rating\":0.001996007984031936,\"likes\":2,\"dislikes\":1000},{\"id\":\"clocks\",\"rating\":0.6024590163934426,\"likes\":5586,\"dislikes\":3686}]],[[{\"id\":\"rainbows\",\"rating\":0.9892053284336243,\"likes\":12921,\"dislikes\":141}],[{\"id\":\"pencils\",\"rating\":0.7901785714285714,\"likes\":12921,\"dislikes\":3431}],[{\"id\":\"mice\",\"rating\":0.5934873949579832,\"likes\":565,\"dislikes\":387}],[{\"id\":\"houses\",\"rating\":0.7303128371089536,\"likes\":677,\"dislikes\":250}],[{\"id\":\"money\",\"rating\":0.5328576521177283,\"likes\":95665,\"dislikes\":83867}]]]}";
        JSONNode json = JSON.Parse(jsonString);
        int questionsPerRound = json["data"][round].Count;

        float timeToWait = 0; 

        // Create indicators 
        Transform[] indicators = UtilitiesScript.CreateIndicators(round, indicatorPrefab);

        // transition texts 
        StartCoroutine(UpdateAnchors("Title", 0f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("Description", 0f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("MyScore", Mathf.Infinity, -7.25f, Mathf.Infinity, timeToWait));
        if (versus) StartCoroutine(UpdateAnchors("OpponentScore", Mathf.Infinity, -7.25f, Mathf.Infinity, timeToWait));

        float wholeQuestionTime = enterTime + myAnswerTime + (versus ? opponentAnswerTime : 0f) + realAnswerTime + starTime + moveTime;

        for (int question = 0; question < questionsPerRound; question++)
        {
            // Get data for this question
            timeToWait = wholeQuestionTime * question;
            string topic = json["data"][round][question][0]["id"];
            float realAnswer = (json["data"][round][question][0]["rating"]);
            //string myAnswer = FetchFullGameTopics.round1[0]; // stored as array for sake of indicators, just take first entry --- WHAT? SHOULDN'T THIS BE [QUESTION]
            float myAnswer = Random.value;
            float opponentAnswer = Random.value;

            // Update indicator colours 
            // previous to green
            if (question != 0) StartCoroutine(IndicatorColour(indicators[question - 1], "green", timeToWait));
            // current to yellow
            StartCoroutine(IndicatorColour(indicators[question], "yellow", timeToWait));

            // CREATIONS + EASING
            // my graph 
            StartCoroutine(CreateGraph(new Vector3(-4f, -4.5f, 0f), "MyAnswer", myColour, myAnswer, timeToWait)); 
            // opponent graph (if versus)
            if (versus)
            {
                StartCoroutine(CreateGraph(new Vector3(4f, -4.5f, 0f), "OpponentAnswer", opponentColour, opponentAnswer, timeToWait));
            }
            // real graph 
            StartCoroutine(CreateGraph(new Vector3 (0f, -4.5f, 0f), "RealAnswer", realColour, realAnswer, timeToWait));
            // topic card + text
            StartCoroutine(CreateCard(topic, timeToWait));
            timeToWait += enterTime;

            // reveal my answer
            StartCoroutine(RevealGraph("MyAnswer", timeToWait));
            timeToWait += myAnswerTime;

            // reveal opponent answer (if versus)
            if(versus)
            {
                StartCoroutine(RevealGraph("OpponentAnswer", timeToWait));
                timeToWait += opponentAnswerTime;
            }
            
            // reveal real answer
            StartCoroutine(RevealGraph("RealAnswer", timeToWait));
            timeToWait += realAnswerTime;

            // reveal my and opponent stars
            float myErrorMargin = Mathf.Abs(myAnswer - realAnswer);
            int myIncrement = 0; 
            Sprite myStar = DetermineStar(myErrorMargin, ref myIncrement);
            StartCoroutine(CreateStar("MyAnswer", "MyTickOrCross", myStar, timeToWait));
            StartCoroutine(IncrementScore(false, myIncrement, timeToWait));

            float opponentErrorMargin = Mathf.Abs(opponentAnswer - realAnswer);
            int opponentIncrement = 0;
            Sprite opponentStar = DetermineStar(opponentErrorMargin, ref opponentIncrement);
            StartCoroutine(CreateStar("OpponentAnswer", "OpponentTickOrCross", opponentStar, timeToWait));
            StartCoroutine(IncrementScore(true, opponentIncrement, timeToWait));
            timeToWait += starTime;

            // move all elements
            //print("Shift anchors right, time:" + timeToWait);
            StartCoroutine(ShiftAnchorsRight(timeToWait));
            timeToWait += moveTime;

            // delete all elements
            StartCoroutine(DeleteAllElements(timeToWait - 0.1f)); 
        }

        // Transition UI elements away 
        StartCoroutine(UpdateAnchors("Indicator", Mathf.Infinity, 14f, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("Title", 16f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("Description", 16f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("MyScore", Mathf.Infinity, -12.25f, Mathf.Infinity, timeToWait));
        if (versus) StartCoroutine(UpdateAnchors("OpponentScore", Mathf.Infinity, -12.25f, Mathf.Infinity, timeToWait));
        timeToWait += endTransitionTime;

        StartCoroutine(GoToNextScene(timeToWait));
    }

    IEnumerator GoToNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(nextLevel);
    }

    IEnumerator IncrementScore(bool isOpponent, int increase, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isOpponent)
        {
            FetchFullGameTopics.opponentScore += increase;
        }
        else
        {
            FetchFullGameTopics.myScore += increase;
        }
    }

    IEnumerator LoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(nextLevel);
    }

    IEnumerator CreateCard(string topic, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject realGraph = GameObject.FindGameObjectWithTag("RealAnswer"); 

        Transform cardInst = Instantiate(cardPrefab, transform.position, Quaternion.identity);
        cardInst.GetComponent<LockCoordsOffset>().anchor = realGraph.transform;
        cardInst.GetComponent<LockCoordsOffset>().offsetY = 9.5f;
        cardInst.GetComponent<LockCoordsOffset>().offsetZ = 1.5f;
        
        // topic text
        Transform topicInst = Instantiate(topicPrefab, transform.position, Quaternion.identity);
        topicInst.GetComponent<LockCoords>().anchor = cardInst;
        topicInst.GetComponent<TextMesh>().text = topic;
    }

    IEnumerator UpdateAnchors(string tag, float x, float y, float z, float delay)
    {
        yield return new WaitForSeconds(delay);
        print("Moving all elements with tag " + tag);
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objs)
        {
            if (x != Mathf.Infinity)
            {
                obj.GetComponent<Ease>().alignmentX = x;
            }

            if (y != Mathf.Infinity)
            {
                obj.GetComponent<Ease>().alignmentY = y;
            }
        }
    }

    Sprite DetermineStar(float errorMargin, ref int scoreIncrease)
    {
        if (errorMargin < 0.01f) // accounts for the fact that the realAnswer has not been rounded
        {
            scoreIncrease = 500;
            return platinumStar;
        }
        else if (errorMargin < 0.1f)
        {
            scoreIncrease = 250;
            return goldStar;
        }
        else if (errorMargin < 0.2f)
        {
            scoreIncrease = 100;
            return silverStar;
        }
        else if (errorMargin < 0.3f)
        {
            scoreIncrease = 50;
            return bronzeStar;
        }
        else
        {
            scoreIncrease = 0; 
            return cross;
        }
    }

    IEnumerator CreateStar(string graphTag, string myTag, Sprite star, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject graphInst = GameObject.FindGameObjectWithTag(graphTag);
        float h = graphInst.GetComponent<GraphPresent>().h; 
        Vector3 loc = new Vector3(graphInst.transform.position.x, graphInst.transform.position.y + h - 0.9f, graphInst.transform.position.z - 0.5f);
        //loc.y = graphInst.GetComponent<GraphPresent>().h - 0.5f;
        //loc.z -= 0.5f;
        Transform starInst = Instantiate(starPrefab, loc, Quaternion.identity);
        print("Star made!");
        starInst.GetComponent<SpriteRenderer>().sprite = star;
        starInst.tag = myTag; 

    }

    IEnumerator RevealGraph(string tag, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject graphInst = GameObject.FindGameObjectWithTag(tag);
        graphInst.GetComponent<GraphPresent>().holdAtStartingValue = false; 
    }

    IEnumerator CreateGraph(Vector3 endPos, string tag, Sprite colour, float rating, float delay)
    {
        yield return new WaitForSeconds(delay);
        Transform graphInst = Instantiate(graphPrefab, new Vector3(endPos.x - 10f, endPos.y, endPos.z), Quaternion.identity);
        graphInst.tag = tag;
        graphInst.GetComponent<SpriteRenderer>().sprite = colour;
        graphInst.GetComponent<Ease>().alignmentX = endPos.x;
        graphInst.GetComponent<GraphPresent>().rating = rating;
        
    }

    IEnumerator IndicatorColour(Transform indicator, string colour, float delay)
    {
        yield return new WaitForSeconds(delay);
        UtilitiesScript.IndicatorColour(indicator, colour);
    }

    IEnumerator DeleteAllElements(float delay)
    {
        yield return new WaitForSeconds(delay);
        print("Deleting all elements");
        string[] tagsToDestroy =
            {
                "RealAnswer",
                "MyAnswer",
                "OpponentAnswer",
                "MyTickOrCross",
                "OpponentTickOrCross"
                //"PinpointOpinionCard"
            };

        foreach (string tag in tagsToDestroy)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject inst in objs)
            {
                Destroy(inst);
            }
        }
    }

    IEnumerator ShiftAnchorsRight(float delay)
    {
        yield return new WaitForSeconds(delay);
        print("Shifting all anchors right");
        string[] tagsToShift =
            {
                "RealAnswer",
                "MyAnswer",
                "OpponentAnswer",
                "MyTickOrCross",
                "OpponentTickOrCross"
                //"PinpointOpinionCard" //?##
            };

        foreach (string tag in tagsToShift)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject inst in objs)
            {
                inst.GetComponent<Ease>().alignmentX = inst.transform.position.x + 14f; 
            }
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
