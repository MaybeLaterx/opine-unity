using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON; 

public class GameHandlerVoting : MonoBehaviour {

    //public string[] myTopics;
    JSONNode json;
    int round, question, totalQuestions;
    bool timeUp; 

    public Transform textPrefab, swiperPrefab, indicatorPrefab;
    Transform[] indicators;
    Transform instSwiper;

    public Transform timer, cipy; 

    GameObject title, description;
    GameObject[] buttons; 

	// Use this for initialization
	void Start () {
        round = FetchFullGameTopics.round++;
        question = 0;
        timeUp = false; 

        // Parse data and prepare initial swiper creation
        string jsonString = FetchFullGameTopics.wwwVText;
        //string jsonString = "{\"success\":true,\"data\":[[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667},{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364},{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257},{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327},{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667},{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364},{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257},{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327},{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667},{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364},{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257},{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327},{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667},{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364},{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257},{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327},{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}]]}";
        json = JSON.Parse(jsonString);
        totalQuestions = json["data"]["topics"][round].Count;

        CreateSwiper(json["data"]["topics"][round][0]); 

        // Create initial indicators based on batch size
        indicators = UtilitiesScript.CreateVoteIndicators(round, indicatorPrefab);

        // Transition title and description
        title = GameObject.FindGameObjectWithTag("Title");
        description = GameObject.FindGameObjectWithTag("Description");
        title.GetComponent<Ease>().alignmentX = 0f;
        description.GetComponent<Ease>().alignmentX = 0f;
        //cipy.GetComponent<Ease>().alignmentX = -6.11f; 

        // Transition buttons 
        buttons = GameObject.FindGameObjectsWithTag("BottomButton");
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Ease>().alignmentY = -9f;
        }

    }

    private IEnumerator CreateSwiper(string topic, float _secs)
    {
        yield return new WaitForSeconds(_secs);
        CreateSwiper(topic);
    }

    void CreateSwiper(string topic)
    {
        if (timeUp) return; 
        print("Creating swiper");

        Vector3 loc = new Vector3(0, 10f, 0f);
        instSwiper = Instantiate(swiperPrefab, loc, Quaternion.identity);

        Transform instText;
        instText = Instantiate(textPrefab, loc, Quaternion.identity) as Transform;
        instText.GetComponent<LockCoords>().anchor = instSwiper;
        instText.GetComponent<TextMesh>().text = topic;
        instSwiper.GetComponent<SwipeVote>().topic = topic;
        print("Assigning topic " + topic + " to swiper");

        /*
        int t = 0;
        int _roundSize = GameObject.FindGameObjectsWithTag("Indicator").Length;
        questionNum %= _roundSize;
        Transform indInst = indicators[questionNum];
        int _batchSize = indInst.GetComponent<IndicatorColour>().myAnswers.Length;
        print("Creating swiper");

        Transform instSwiper;
        Vector3 loc = new Vector3(0, 10f + (t * 0.5f), 0f - (t * 1f));
        instSwiper = Instantiate(swiperPrefab, loc, Quaternion.identity) as Transform;

        string topic = indInst.GetComponent<IndicatorColour>().myAnswers[t];
        Sprite colour = indInst.GetComponent<IndicatorColour>().colours[t];
        //string topic = _json["data"][_round][_batch][t]["id"];
        Transform instText;
        instText = Instantiate(textPrefab, loc, Quaternion.identity) as Transform;
        instText.GetComponent<LockCoords>().anchor = instSwiper;
        instText.GetComponent<TextMesh>().text = topic;
        instSwiper.GetComponent<SwipeScript>().topic = topic;
        print("Assigning topic " + topic + " to swiper");
        */

    }

    public void TimeUp()
    {
        //string[] compiledAnswers = new string[indicators.Length];
        int k = 0;
        int i = 0; 
        JSONNode votesJson = JSON.Parse("{}");
        votesJson["uuid"] = GlobalScript.uuid;
        foreach (Transform indicator in indicators)
        {
            if (indicator.GetComponent<IndicatorColour>().answered) 
            {
                string thisAnswer = indicator.GetComponent<IndicatorColour>().myAnswers[0];
                if (thisAnswer == "yay" || thisAnswer == "nay")
                {
                    votesJson["topics"][k]["id"] = json["data"]["topics"][round][i];
                    votesJson["topics"][k]["vote"] = (thisAnswer == "yay");
                    k++;
                }
                //compiledAnswers[k] = thisAnswer;
                i++;
            }
            indicator.GetComponent<IndicatorColour>().answered = true;
        }
        print("Votes Json: " + votesJson);
        StartCoroutine(SendVotesToServer(votesJson));
        //UpdateIndicators("unanswered"); // one is re-assigned unanswered again

        // tell swiper to disappear in appropriate direction
        GameObject[] swipers = GameObject.FindGameObjectsWithTag("VotingCard"); 
        GameObject swiper = swipers[swipers.Length - 1];
        swiper.GetComponent<SwipeVote>().pivot = swiper.GetComponent<SwipeVote>().pivotBottom;

        // Prevent more cards from spawning
        timeUp = true;

        EndRound();

        /*
         * // Load next scene 
                print("Voting round complete! Saving answers");
                //JSONNode votesJson = JSON.Parse("{\"uuid\" = \"12234567890\", \"topics\" = []}"); // is this an empty node? 
                JSONNode votesJson = JSON.Parse("{}");
                votesJson["uuid"] = GlobalScript.uuid; 

                int validAnswers = 0; 
                for (int j = 0; j < totalQuestions; j++) // each batch
                {
                    string thisAnswer = indicators[j].GetComponent<IndicatorColour>().myAnswers[0];
                    if (thisAnswer == "yay" || thisAnswer == "nay")
                    {
                        print("A: " + json["data"]["topics"][round][j]);
                        print("B: " + thisAnswer == "yay");
                        votesJson["topics"][validAnswers]["id"] = json["data"]["topics"][round][j];
                        votesJson["topics"][validAnswers]["vote"] = (thisAnswer == "yay"); 
                        validAnswers++; 
                    }

                    //compiledColours[j, k] = indicators[j].GetComponent<IndicatorColour>().colours[k];
                }
                StartCoroutine(SendVotesToServer(votesJson));
                EndRound(); 
                return true;
                */
    }

    public void Trigger(string boxType)
    {
        // Update lights 
        foreach (GameObject indicator in GameObject.FindGameObjectsWithTag("Indicator"))
        {
            indicator.GetComponent<IndicatorColour>().shouldUpdate = true;
        }

        // tell swiper to disappear in appropriate direction
        switch (boxType)
        {
            case "yay": instSwiper.GetComponent<SwipeVote>().pivot = instSwiper.GetComponent<SwipeVote>().pivotLeft; break;
            case "nay": instSwiper.GetComponent<SwipeVote>().pivot = instSwiper.GetComponent<SwipeVote>().pivotRight; break;
            case "skip": instSwiper.GetComponent<SwipeVote>().pivot = instSwiper.GetComponent<SwipeVote>().pivotBottom; break;
            default: print("Bad response: " + boxType); break;
        }


        // tell controller to update lights and get next
        bool isGameOver = UpdateIndicators(boxType); //"yay", "nay" or "skip"
        if (!isGameOver) StartCoroutine(CreateSwiper(json["data"]["topics"][round][question], 0.6f)); // round, question, 0, id? 

        // update internal cooldown
        ConfirmVote.shouldTransition = true;
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
        if (boxType != "skip") thisLight.GetComponent<IndicatorColour>().myAnswers = new string[] { boxType }; // "yay" or "nay"
        thisLight.GetComponent<IndicatorColour>().shouldUpdate = true;

        // Find next indicator and update
        int i = 0;
        do
        {
            i++;
            if (i > totalQuestions) // every question is answered
            {
                // Load next scene 
                print("Voting round complete! Saving answers");
                //JSONNode votesJson = JSON.Parse("{\"uuid\" = \"12234567890\", \"topics\" = []}"); // is this an empty node? 
                JSONNode votesJson = JSON.Parse("{}");
                votesJson["uuid"] = GlobalScript.uuid; 

                int validAnswers = 0; 
                for (int j = 0; j < totalQuestions; j++) // each batch
                {
                    string thisAnswer = indicators[j].GetComponent<IndicatorColour>().myAnswers[0];
                    if (thisAnswer == "yay" || thisAnswer == "nay")
                    {
                        print("A: " + json["data"]["topics"][round][j]);
                        print("B: " + thisAnswer == "yay");
                        votesJson["topics"][validAnswers]["id"] = json["data"]["topics"][round][j];
                        votesJson["topics"][validAnswers]["vote"] = (thisAnswer == "yay"); 
                        validAnswers++; 
                    }

                    //compiledColours[j, k] = indicators[j].GetComponent<IndicatorColour>().colours[k];
                }
                StartCoroutine(SendVotesToServer(votesJson));
                EndRound(); 
                return true;
            }
            question = (question + 1) % totalQuestions;
        } while (indicators[question].GetComponent<IndicatorColour>().answered == true);
        Transform nextLight = indicators[question];
        nextLight.GetComponent<IndicatorColour>().current = true;

        return false;
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
        timer.GetComponent<Ease>().alignmentX = 32f; 

        StartCoroutine(LoadNextScene(1f)); 
    }

    IEnumerator SendVotesToServer(JSONNode vJson) 
    {
        print("Sending JSON of votes on provided topics");

        string postVotesUrl = GlobalScript.domain + "/submitVotes";
        string jsonData = vJson.ToString(); // does this work? 
        Hashtable headers = new Hashtable();
        headers.Add("Content-Type", "application/json");
        headers.Add("accept-version", GlobalScript.apiVersion);
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(jsonData.ToCharArray());
        WWW www = new WWW(postVotesUrl, pData, headers);

        yield return www; // from here on out will never run 
        print(www.text);
        JSONNode V = JSON.Parse(www.text);

    }

    IEnumerator LoadNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        string nextScene = "";
        switch (round)
        {
            case 0: nextScene = "S_YayOrNay_Results"; break; 
            case 1: nextScene = "S_TopDog_Results"; break;
            case 2: nextScene = "S_PeckingOrder_Results"; break;
            case 3: nextScene = "S_PinpointOpinion_Results"; break;
            default: print("round switch error"); break;
        }
        print("Going to level '" + nextScene + "'");
        Application.LoadLevel(nextScene);
    }

    // Update is called once per frame
    void Update () {

	}
}
