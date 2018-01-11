using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON; 

public class GameHandlerPeckingOrder : MonoBehaviour {

    public string nextLevel; 

    public string[] myTopics;
    JSONNode json;
    bool timeUp = false; 

    public Transform card;
    public Transform oText; 
    int round;
    int currentBatch; 
    bool readyForNext;
    bool waitingForData;
    public Sprite[] colours; 

    public float gap = 1.5f;
    public Transform[] indicators;
    public Transform indicator;
    public int totalBatches;

    GameObject title, description;
    GameObject[] buttons; 

    // Use this for initialization
    void Start () {
        waitingForData = true;
        round = 2; // 0 indexed
        currentBatch = 0; // 0 indexed
        readyForNext = false; 
    }
	

	// Update is called once per frame
	void Update () {
        if (waitingForData) // wait to return http request 
        {
            // Parse data and prepare initial card creation
            string jsonString = FetchFullGameTopics.wwwText;
            //string jsonString = "{\"success\":true,\"data\":[[[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667}],[{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364}],[{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257}],[{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327}],[{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"elephants\",\"rating\":0.6986486486486486,\"likes\":1034,\"dislikes\":446}],[{\"id\":\"yachts\",\"rating\":0.2879690538248314,\"likes\":13921,\"dislikes\":34421}],[{\"id\":\"journals\",\"rating\":0.29131989623483007,\"likes\":106797,\"dislikes\":259800}],[{\"id\":\"beavers\",\"rating\":0.20733944954128442,\"likes\":113,\"dislikes\":432}],[{\"id\":\"biscuits\",\"rating\":0.7298632218844985,\"likes\":1921,\"dislikes\":711}]],[[{\"id\":\"yoghurts\",\"rating\":0.7269151478311769,\"likes\":678057,\"dislikes\":254730},{\"id\":\"gerbils\",\"rating\":0.99366921656555,\"likes\":11301,\"dislikes\":72}],[{\"id\":\"panthers\",\"rating\":0.6368086147821831,\"likes\":1301,\"dislikes\":742},{\"id\":\"maggots\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211}],[{\"id\":\"cats\",\"rating\":0.7541899441340782,\"likes\":135,\"dislikes\":44},{\"id\":\"superheroes\",\"rating\":0.5942489666603722,\"likes\":56645,\"dislikes\":38677}],[{\"id\":\"cheese\",\"rating\":0.5339358685215372,\"likes\":95645,\"dislikes\":83487},{\"id\":\"radios\",\"rating\":0.7260589303729257,\"likes\":674357,\"dislikes\":254434}],[{\"id\":\"gravy\",\"rating\":0.6360566134405463,\"likes\":568585,\"dislikes\":325337},{\"id\":\"uni\",\"rating\":0.724858696811347,\"likes\":6797,\"dislikes\":2580}],[{\"id\":\"vanilla\",\"rating\":0.6375423557513622,\"likes\":586655,\"dislikes\":333527},{\"id\":\"noodles\",\"rating\":0.7169585987261147,\"likes\":1801,\"dislikes\":711}],[{\"id\":\"salt\",\"rating\":0.7217311541027351,\"likes\":8655,\"dislikes\":3337},{\"id\":\"cigarettes\",\"rating\":0.6425592625109745,\"likes\":5855,\"dislikes\":3257}],[{\"id\":\"pyjamas\",\"rating\":0.9359046608764835,\"likes\":56465,\"dislikes\":3867},{\"id\":\"deserts\",\"rating\":0.6359325218876215,\"likes\":56825685,\"dislikes\":32532357}],[{\"id\":\"pickles\",\"rating\":0.5339995533720411,\"likes\":9565,\"dislikes\":8347},{\"id\":\"leopards\",\"rating\":0.20764424843807425,\"likes\":1130,\"dislikes\":4312}],[{\"id\":\"beess\",\"rating\":0.4893324156916724,\"likes\":711,\"dislikes\":742},{\"id\":\"santa\",\"rating\":0.9229403868456543,\"likes\":4123921,\"dislikes\":344321}]],[[{\"id\":\"june\",\"rating\":0.6414473684210527,\"likes\":585,\"dislikes\":327},{\"id\":\"fleas\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211},{\"id\":\"lollipops\",\"rating\":0.7259781237122767,\"likes\":67407357,\"dislikes\":25443040},{\"id\":\"chips\",\"rating\":0.5329241071428571,\"likes\":955,\"dislikes\":837}],[{\"id\":\"ladybirds\",\"rating\":0.6360188831215321,\"likes\":56855,\"dislikes\":32537},{\"id\":\"cockrels\",\"rating\":0.14956109188421077,\"likes\":13051,\"dislikes\":74211},{\"id\":\"cows\",\"rating\":0.01342549514821215,\"likes\":101,\"dislikes\":7422},{\"id\":\"buildings\",\"rating\":0.7267935893298914,\"likes\":6757,\"dislikes\":2540}],[{\"id\":\"chocolate\",\"rating\":0.9478433098591549,\"likes\":12921,\"dislikes\":711},{\"id\":\"oceans\",\"rating\":0.6785849894934803,\"likes\":682685,\"dislikes\":323357},{\"id\":\"flamingos\",\"rating\":0.001996007984031936,\"likes\":2,\"dislikes\":1000},{\"id\":\"clocks\",\"rating\":0.6024590163934426,\"likes\":5586,\"dislikes\":3686}]],[[{\"id\":\"rainbows\",\"rating\":0.9892053284336243,\"likes\":12921,\"dislikes\":141}],[{\"id\":\"pencils\",\"rating\":0.7901785714285714,\"likes\":12921,\"dislikes\":3431}],[{\"id\":\"mice\",\"rating\":0.5934873949579832,\"likes\":565,\"dislikes\":387}],[{\"id\":\"houses\",\"rating\":0.7303128371089536,\"likes\":677,\"dislikes\":250}],[{\"id\":\"money\",\"rating\":0.5328576521177283,\"likes\":95665,\"dislikes\":83867}]]]}";
            json = JSON.Parse(jsonString);
            totalBatches = json["data"][round].Count;
            readyForNext = true;
            waitingForData = false;

            // Create initial indicators based on batch size
            indicators = UtilitiesScript.CreateIndicators(round, indicator);

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
        
		if (readyForNext)
        {
            CreateCards(round, currentBatch);
            readyForNext = false; 
        }
	}

    public void TimeUp()
    {
        //string[] compiledAnswers = new string[indicators.Length];
        foreach (Transform indicator in indicators)
        {

            IndicatorColour cmp = indicator.GetComponent<IndicatorColour>();
            if (indicator.GetComponent<IndicatorColour>().answered == false)
            {
                cmp.myAnswers = new string[] { "answer",
                                        "submit",
                                    "not",
                                "did" };
                cmp.answered = true;
            }
            //compiledAnswers[k] = cmp.myAnswers[0];
        }

        print("Pecking Order round complete! Saving answers");
        string[,] compiledAnswers = new string[indicators.Length, 4];
        for (int j = 0; j < indicators.Length; j++) // each question
        {
            for (int k = 0; k < 4; k++) // each answer
            {

                compiledAnswers[j, k] = indicators[j].GetComponent<IndicatorColour>().myAnswers[k];
            }
        }
        FetchFullGameTopics.round3 = compiledAnswers;

        //UpdateIndicators("unanswered"); // one is re-assigned unanswered again

        // tell boxes to disappear
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("OrderBox");
        foreach (GameObject box in boxes)
        {
            box.GetComponent<OrderBoxHeight>().endX *= -1; // same direction as a skip
            box.GetComponent<OrderBoxHeight>().answerGiven = true;
        }

        // Prevent more cards from spawning
        timeUp = true;

        EndRound();
    }

    public void MakeNextCards()
    {
        StartCoroutine(CreateCards(round, currentBatch, 0.6f));
    }

    public bool UpdateIndicators(bool _answered) // Returns whether the game is over
    {
        // Update current indicator
        Transform thisLight = indicators[currentBatch%totalBatches];
        thisLight.GetComponent<IndicatorColour>().current = false;
        
        // Skipping or answering?
        if (!_answered) print("Skipping! Storing answers!"); 
        else
        {
            print("Answers selected! Saving!");
            thisLight.GetComponent<IndicatorColour>().answered = true;
        }

        // Store answers and colours
        int batchSize = thisLight.GetComponent<IndicatorColour>().myAnswers.Length;
        string[] localAnswers = new string[batchSize];
        Sprite[] localColours = new Sprite[batchSize];
        print("Ordering topics:");
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("OrderBox");
        foreach (GameObject box in boxes)
        {
            int numAbove = 0;
            float thisY = box.transform.position.y;
            foreach (GameObject box2 in boxes)
            {
                float bY = box2.transform.position.y;
                if (bY < thisY)
                {
                    numAbove++;
                }
            }
            localAnswers[numAbove] = box.GetComponent<OrderBoxHeight>().topic;
            localColours[numAbove] = box.GetComponent<SpriteRenderer>().sprite; 
            print(numAbove + ": " + localAnswers[numAbove] + " (" + localColours[numAbove].name + ")");
        }
        thisLight.GetComponent<IndicatorColour>().myAnswers = localAnswers;
        thisLight.GetComponent<IndicatorColour>().colours = localColours;

        // Find next indicator and update
        int i = 0; 
        do
        {
            i++; 
            if (i > totalBatches) // every question is answered
            {
                // Load next scene 
                print("Pecking Order round complete! Saving answers");
                string[,] compiledAnswers = new string[totalBatches, batchSize];
                Sprite[,] compiledColours = new Sprite[totalBatches, batchSize];
                for (int j =  0; j < totalBatches; j++) // each batch
                {
                    for (int k = 0; k < batchSize; k++) // each answer
                    {

                        compiledAnswers[j, k] = indicators[j].GetComponent<IndicatorColour>().myAnswers[k];
                        compiledColours[j, k] = indicators[j].GetComponent<IndicatorColour>().colours[k];
                    }
                }
                FetchFullGameTopics.round3 = compiledAnswers;
                FetchFullGameTopics.round3Colours = compiledColours;

                EndRound();
                return true;
            }
            currentBatch = (currentBatch + 1) % totalBatches;
        } while (indicators[currentBatch].GetComponent<IndicatorColour>().answered == true);
        Transform nextLight = indicators[currentBatch];
        nextLight.GetComponent<IndicatorColour>().current = true;
        return false; 
    }

    private IEnumerator CreateCards(int _round, int _batch, float _secs)
    {
        yield return new WaitForSeconds(_secs);
        CreateCards(_round, _batch);
    }

    void CreateCards(int _round, int _batch)
    {
        if (!timeUp)
        {
            int _roundSize = GameObject.FindGameObjectsWithTag("Indicator").Length;
            _batch %= _roundSize;
            Transform indInst = indicators[_batch];
            int _batchSize = indInst.GetComponent<IndicatorColour>().myAnswers.Length;
            print("Creating " + _batchSize + " cards");
            for (int t = 0; t < _batchSize; t++)
            {
                Transform instCard;
                Vector3 loc = new Vector3(0, 10f + (t * 0.5f), 0f - (t * 1f));
                instCard = Instantiate(card, loc, Quaternion.identity) as Transform;

                string topic = indInst.GetComponent<IndicatorColour>().myAnswers[t];
                Sprite colour = indInst.GetComponent<IndicatorColour>().colours[t];
                //string topic = _json["data"][_round][_batch][t]["id"];
                //instCard.GetComponentInChildren<TextMesh>().text = topic; 
                Transform instText;
                instText = Instantiate(oText, loc, Quaternion.identity) as Transform;
                instText.GetComponent<LockCoords>().anchor = instCard;
                instText.GetComponent<TextMesh>().text = topic;
                instCard.GetComponent<OrderBoxHeight>().topic = topic;
                instCard.GetComponent<SpriteRenderer>().sprite = colour;
                print("Assigning topic " + topic + " to card " + t);
            }
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
