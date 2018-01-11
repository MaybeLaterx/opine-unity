using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PresentTopDog : MonoBehaviour {

    public string nextLevel;

    int round, question;
    public bool versus;

    public float cardEnterTime = 2f, myAnswerTime = 2f, opponentAnswerTime = 2f, realAnswerTime = 2f, tickOrCrossTime = 2f, moveTime = 2f, endTransitionTime = 2f;
    public Transform indicatorPrefab, cardPrefab, textPrefab, percentPrefab, tickOrCrossPrefab;

    public Sprite sprite1, sprite2, tick, cross, oneSprite, twoSprite, unansweredSprite;

    Transform[] cards;

    // Use this for initialization
    void Start () {
        round = 1;

        versus = FetchFullGameTopics.versus;

        string jsonString = FetchFullGameTopics.wwwText;
        //string jsonString = "{\"success\":true,\"data\":[[[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667}],[{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364}],[{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257}],[{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327}],[{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"elephants\",\"rating\":0.6986486486486486,\"likes\":1034,\"dislikes\":446}],[{\"id\":\"yachts\",\"rating\":0.2879690538248314,\"likes\":13921,\"dislikes\":34421}],[{\"id\":\"journals\",\"rating\":0.29131989623483007,\"likes\":106797,\"dislikes\":259800}],[{\"id\":\"beavers\",\"rating\":0.20733944954128442,\"likes\":113,\"dislikes\":432}],[{\"id\":\"biscuits\",\"rating\":0.7298632218844985,\"likes\":1921,\"dislikes\":711}]],[[{\"id\":\"yoghurts\",\"rating\":0.7269151478311769,\"likes\":678057,\"dislikes\":254730},{\"id\":\"gerbils\",\"rating\":0.99366921656555,\"likes\":11301,\"dislikes\":72}],[{\"id\":\"panthers\",\"rating\":0.6368086147821831,\"likes\":1301,\"dislikes\":742},{\"id\":\"maggots\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211}],[{\"id\":\"cats\",\"rating\":0.7541899441340782,\"likes\":135,\"dislikes\":44},{\"id\":\"superheroes\",\"rating\":0.5942489666603722,\"likes\":56645,\"dislikes\":38677}],[{\"id\":\"cheese\",\"rating\":0.5339358685215372,\"likes\":95645,\"dislikes\":83487},{\"id\":\"radios\",\"rating\":0.7260589303729257,\"likes\":674357,\"dislikes\":254434}],[{\"id\":\"gravy\",\"rating\":0.6360566134405463,\"likes\":568585,\"dislikes\":325337},{\"id\":\"uni\",\"rating\":0.724858696811347,\"likes\":6797,\"dislikes\":2580}],[{\"id\":\"vanilla\",\"rating\":0.6375423557513622,\"likes\":586655,\"dislikes\":333527},{\"id\":\"noodles\",\"rating\":0.7169585987261147,\"likes\":1801,\"dislikes\":711}],[{\"id\":\"salt\",\"rating\":0.7217311541027351,\"likes\":8655,\"dislikes\":3337},{\"id\":\"cigarettes\",\"rating\":0.6425592625109745,\"likes\":5855,\"dislikes\":3257}],[{\"id\":\"pyjamas\",\"rating\":0.9359046608764835,\"likes\":56465,\"dislikes\":3867},{\"id\":\"deserts\",\"rating\":0.6359325218876215,\"likes\":56825685,\"dislikes\":32532357}],[{\"id\":\"pickles\",\"rating\":0.5339995533720411,\"likes\":9565,\"dislikes\":8347},{\"id\":\"leopards\",\"rating\":0.20764424843807425,\"likes\":1130,\"dislikes\":4312}],[{\"id\":\"beess\",\"rating\":0.4893324156916724,\"likes\":711,\"dislikes\":742},{\"id\":\"santa\",\"rating\":0.9229403868456543,\"likes\":4123921,\"dislikes\":344321}]],[[{\"id\":\"june\",\"rating\":0.6414473684210527,\"likes\":585,\"dislikes\":327},{\"id\":\"fleas\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211},{\"id\":\"lollipops\",\"rating\":0.7259781237122767,\"likes\":67407357,\"dislikes\":25443040},{\"id\":\"chips\",\"rating\":0.5329241071428571,\"likes\":955,\"dislikes\":837}],[{\"id\":\"ladybirds\",\"rating\":0.6360188831215321,\"likes\":56855,\"dislikes\":32537},{\"id\":\"cockrels\",\"rating\":0.14956109188421077,\"likes\":13051,\"dislikes\":74211},{\"id\":\"cows\",\"rating\":0.01342549514821215,\"likes\":101,\"dislikes\":7422},{\"id\":\"buildings\",\"rating\":0.7267935893298914,\"likes\":6757,\"dislikes\":2540}],[{\"id\":\"chocolate\",\"rating\":0.9478433098591549,\"likes\":12921,\"dislikes\":711},{\"id\":\"oceans\",\"rating\":0.6785849894934803,\"likes\":682685,\"dislikes\":323357},{\"id\":\"flamingos\",\"rating\":0.001996007984031936,\"likes\":2,\"dislikes\":1000},{\"id\":\"clocks\",\"rating\":0.6024590163934426,\"likes\":5586,\"dislikes\":3686}]],[[{\"id\":\"rainbows\",\"rating\":0.9892053284336243,\"likes\":12921,\"dislikes\":141}],[{\"id\":\"pencils\",\"rating\":0.7901785714285714,\"likes\":12921,\"dislikes\":3431}],[{\"id\":\"mice\",\"rating\":0.5934873949579832,\"likes\":565,\"dislikes\":387}],[{\"id\":\"houses\",\"rating\":0.7303128371089536,\"likes\":677,\"dislikes\":250}],[{\"id\":\"money\",\"rating\":0.5328576521177283,\"likes\":95665,\"dislikes\":83867}]]]}";
        JSONNode json = JSON.Parse(jsonString);
        int questionsPerRound = json["data"][round].Count;

        // Create indicators 
        Transform[] indicators = UtilitiesScript.CreateIndicators(round, indicatorPrefab);

        float timeToWait = 0f; 

        // Transition texts
        StartCoroutine(UpdateAnchors("Title", 0f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("Description", 0f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("MyScore", Mathf.Infinity, -7.25f, Mathf.Infinity, timeToWait));
        if (versus) StartCoroutine(UpdateAnchors("OpponentScore", Mathf.Infinity, -7.25f, Mathf.Infinity, timeToWait));

        float wholeQuestionTime = cardEnterTime + myAnswerTime + (versus ? opponentAnswerTime : 0f) + realAnswerTime + tickOrCrossTime + moveTime;

        for (int question = 0; question < questionsPerRound; question++)
        {
            // Get data for this question
            timeToWait = wholeQuestionTime * question;
            string topic1 = json["data"][round][question][0]["id"];
            string topic2 = json["data"][round][question][1]["id"];
            float rating1 = json["data"][round][question][0]["rating"];
            float rating2 = json["data"][round][question][1]["rating"];
            string myAnswer = FetchFullGameTopics.round2[question]; 
            //string myAnswer = (Random.value > 0.5f ? "1" : "2");
            string opponentAnswer = (Random.value > 0.5f ? "1" : "2");
            string realAnswer = (rating1 > rating2 ? "1" : "2");

            // Update indicator colours 
            // previous to green
            if (question != 0) StartCoroutine(IndicatorColour(indicators[question - 1], "green", timeToWait));

            // current to yellow
            StartCoroutine(IndicatorColour(indicators[question], "yellow", timeToWait));

            // Create Topic Card
            StartCoroutine(CreateCard(topic1, sprite1, topic2, sprite2, timeToWait));
            timeToWait += cardEnterTime;

            // Show my answer 
            StartCoroutine(CreateNumber(false, myAnswer, timeToWait));
            timeToWait += myAnswerTime;

            // Show opponent's answer
            if (versus)
            {
                StartCoroutine(CreateNumber(true, opponentAnswer, timeToWait));
            }
            timeToWait += opponentAnswerTime;

            /*
            // Show real percentages
            StartCoroutine(CreatePercentage(true, rating1, timeToWait));
            StartCoroutine(CreatePercentage(false, rating2, timeToWait));
            timeToWait += realAnswerTime;
            */

            // Change card sizes (reveal answers) 
            //print("Topic1: " + topic1 + ", topic2: " + topic2 + ", realanswer: " + realAnswer);
            StartCoroutine(UpdateSize(1, realAnswer, timeToWait));
            StartCoroutine(UpdateSize(2, realAnswer, timeToWait));
            timeToWait += realAnswerTime;

            // Show ticks and crosses
            StartCoroutine(CreateTickOrCross(false, myAnswer == realAnswer, timeToWait));
            if (myAnswer == realAnswer) StartCoroutine(IncrementScore(false, 100, timeToWait));
            if (versus)
            {
                StartCoroutine(CreateTickOrCross(true, opponentAnswer == realAnswer, timeToWait));
                if (opponentAnswer == realAnswer) StartCoroutine(IncrementScore(true, 100, timeToWait));
            }
            timeToWait += tickOrCrossTime;

            // Move all elements off
            StartCoroutine(UpdateAnchors("MyAnswer", -8.5f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("MyTickOrCross", -8.5f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("OpponentAnswer", 8.5f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("OpponentTickOrCross", 8.5f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("RealAnswer", Mathf.Infinity, -16f, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("TopDogCard", Mathf.Infinity, -15f, Mathf.Infinity, timeToWait));
            timeToWait += moveTime;

            // Delete all elements
            StartCoroutine(DeleteAllElements(timeToWait - 0.1f));
        }

        // Transition UI elements away 
        StartCoroutine(UpdateAnchors("Indicator", Mathf.Infinity, 14f, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("Title", 16f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("Description", 16f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("MyScore", Mathf.Infinity, -12.25f, Mathf.Infinity, timeToWait));
        if (versus) StartCoroutine(UpdateAnchors("OpponentScore", Mathf.Infinity, -12.25f, Mathf.Infinity, timeToWait));
        timeToWait += endTransitionTime;

        // Move to next scene at end
        StartCoroutine(LoadNextLevel(timeToWait));
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

    IEnumerator CreateNumber(bool isOpponent, string answer, float delay)
    {
        yield return new WaitForSeconds(delay);
        print("Displaying coloured answer");
        float startX;
        float newX;
        string newTag;

        if (isOpponent)
        {
            startX = 10f;
            newX = 3f;
            newTag = "OpponentAnswer";
        } else
        {
            startX = -10f;
            newX = -3f;
            newTag = "MyAnswer";
        }

        Vector3 loc = new Vector3(startX, -5.2f, -0.5f);
        Transform thumb = Instantiate(tickOrCrossPrefab, loc, Quaternion.identity);
        Sprite thumbSprite = unansweredSprite;
        if (answer == "1") thumbSprite = oneSprite;
        else if (answer == "2") thumbSprite = twoSprite;

        thumb.GetComponent<SpriteRenderer>().sprite = thumbSprite;

        thumb.GetComponent<Ease>().alignmentX = newX;
        thumb.tag = newTag;
    }

    IEnumerator CreatePercentage(bool isTopic1, float percentage, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 loc = (isTopic1 ? new Vector3(-5f, 11.43f, -5f) : new Vector3(6.1f, 11.66f, -5f));
        Vector3 rot = new Vector3(0, 0, (isTopic1 ? 12f : -12f));
        Transform inst = Instantiate(percentPrefab, loc, Quaternion.Euler(rot));
        inst.GetComponent<TextMesh>().text = UtilitiesScript.DecimalToPercentage(percentage); 
        inst.GetComponent<Ease>().alignmentX = (isTopic1 ? -2.78f : 2.78f);
        inst.GetComponent<Ease>().alignmentY = (isTopic1 ? 1f : -3.92f);
    }

    IEnumerator CreateTickOrCross(bool isOpponent, bool isCorrect, float delay)
    {
        yield return new WaitForSeconds(delay);
        print("Displaying ticks and crosses");
        Vector3 loc = new Vector3((isOpponent ? 10f : -10f), -5.2f, -0.1f);
        Transform thumb = Instantiate(tickOrCrossPrefab, loc, Quaternion.identity);
        thumb.GetComponent<SpriteRenderer>().sprite = (isCorrect ? tick : cross);
        thumb.GetComponent<Ease>().alignmentX = (isOpponent ? 6f : -6f);
        thumb.tag = (isOpponent ? "OpponentTickOrCross" : "MyTickOrCross");
    }

    IEnumerator IndicatorColour(Transform indicator, string colour, float delay)
    {
        yield return new WaitForSeconds(delay);
        UtilitiesScript.IndicatorColour(indicator, colour);
    }


    IEnumerator CreateCard(string topic1, Sprite sprite1, string topic2, Sprite sprite2, float delay)
    {
        yield return new WaitForSeconds(delay);
        print("Creating topic card");
        cards = UtilitiesScript.CreateTopDogCards(cardPrefab, textPrefab, topic1, sprite1, topic2, sprite2);
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

    IEnumerator UpdateSize(int index, string correctIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        float bigX = 1.1f;
        float bigY = 1.1f;
        float smallX = 0.75f;
        float smallY = 0.75f; 

        cards[index - 1].GetComponent<Ease>().scaleX = (correctIndex == index.ToString() ? bigX : smallX);
        cards[index - 1].GetComponent<Ease>().scaleY = (correctIndex == index.ToString() ? bigY : smallY);

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
                "OpponentTickOrCross",
                "TopDogCard",
                "PercentText"
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


    // Update is called once per frame
    void Update () {
		
	}
}
