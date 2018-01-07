using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PresentPeckingOrder : MonoBehaviour
{

    public string nextLevel;


    int round;
    int question;
    public bool versus;

    public float ownAnswerTime = 2f, opponentAnswerTime = 2f, realAnswerTime = 2f, endTransitionTime = 2f, moveTime = 2f;

    public Transform cardPrefab;
    public Transform textPrefab;
    public Transform tickOrCross;
    public Transform indicatorPrefab; 

    public Sprite[] colourOrder;
    public Sprite unansweredSprite; 

    public Sprite tick, cross; 


    // Use this for initialization
    void Start()
    {
        round = 2;
        question = 0;
        versus = FetchFullGameTopics.versus;
        //versus = true; 


        string jsonString = FetchFullGameTopics.www.text;
        //string jsonString = "{\"success\":true,\"data\":[[[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667}],[{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364}],[{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257}],[{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327}],[{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"elephants\",\"rating\":0.6986486486486486,\"likes\":1034,\"dislikes\":446}],[{\"id\":\"yachts\",\"rating\":0.2879690538248314,\"likes\":13921,\"dislikes\":34421}],[{\"id\":\"journals\",\"rating\":0.29131989623483007,\"likes\":106797,\"dislikes\":259800}],[{\"id\":\"beavers\",\"rating\":0.20733944954128442,\"likes\":113,\"dislikes\":432}],[{\"id\":\"biscuits\",\"rating\":0.7298632218844985,\"likes\":1921,\"dislikes\":711}]],[[{\"id\":\"yoghurts\",\"rating\":0.7269151478311769,\"likes\":678057,\"dislikes\":254730},{\"id\":\"gerbils\",\"rating\":0.99366921656555,\"likes\":11301,\"dislikes\":72}],[{\"id\":\"panthers\",\"rating\":0.6368086147821831,\"likes\":1301,\"dislikes\":742},{\"id\":\"maggots\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211}],[{\"id\":\"cats\",\"rating\":0.7541899441340782,\"likes\":135,\"dislikes\":44},{\"id\":\"superheroes\",\"rating\":0.5942489666603722,\"likes\":56645,\"dislikes\":38677}],[{\"id\":\"cheese\",\"rating\":0.5339358685215372,\"likes\":95645,\"dislikes\":83487},{\"id\":\"radios\",\"rating\":0.7260589303729257,\"likes\":674357,\"dislikes\":254434}],[{\"id\":\"gravy\",\"rating\":0.6360566134405463,\"likes\":568585,\"dislikes\":325337},{\"id\":\"uni\",\"rating\":0.724858696811347,\"likes\":6797,\"dislikes\":2580}],[{\"id\":\"vanilla\",\"rating\":0.6375423557513622,\"likes\":586655,\"dislikes\":333527},{\"id\":\"noodles\",\"rating\":0.7169585987261147,\"likes\":1801,\"dislikes\":711}],[{\"id\":\"salt\",\"rating\":0.7217311541027351,\"likes\":8655,\"dislikes\":3337},{\"id\":\"cigarettes\",\"rating\":0.6425592625109745,\"likes\":5855,\"dislikes\":3257}],[{\"id\":\"pyjamas\",\"rating\":0.9359046608764835,\"likes\":56465,\"dislikes\":3867},{\"id\":\"deserts\",\"rating\":0.6359325218876215,\"likes\":56825685,\"dislikes\":32532357}],[{\"id\":\"pickles\",\"rating\":0.5339995533720411,\"likes\":9565,\"dislikes\":8347},{\"id\":\"leopards\",\"rating\":0.20764424843807425,\"likes\":1130,\"dislikes\":4312}],[{\"id\":\"beess\",\"rating\":0.4893324156916724,\"likes\":711,\"dislikes\":742},{\"id\":\"santa\",\"rating\":0.9229403868456543,\"likes\":4123921,\"dislikes\":344321}]],[[{\"id\":\"june\",\"rating\":0.6414473684210527,\"likes\":585,\"dislikes\":327},{\"id\":\"fleas\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211},{\"id\":\"lollipops\",\"rating\":0.7259781237122767,\"likes\":67407357,\"dislikes\":25443040},{\"id\":\"chips\",\"rating\":0.5329241071428571,\"likes\":955,\"dislikes\":837}],[{\"id\":\"ladybirds\",\"rating\":0.6360188831215321,\"likes\":56855,\"dislikes\":32537},{\"id\":\"cockrels\",\"rating\":0.14956109188421077,\"likes\":13051,\"dislikes\":74211},{\"id\":\"cows\",\"rating\":0.01342549514821215,\"likes\":101,\"dislikes\":7422},{\"id\":\"buildings\",\"rating\":0.7267935893298914,\"likes\":6757,\"dislikes\":2540}],[{\"id\":\"chocolate\",\"rating\":0.9478433098591549,\"likes\":12921,\"dislikes\":711},{\"id\":\"oceans\",\"rating\":0.6785849894934803,\"likes\":682685,\"dislikes\":323357},{\"id\":\"flamingos\",\"rating\":0.001996007984031936,\"likes\":2,\"dislikes\":1000},{\"id\":\"clocks\",\"rating\":0.6024590163934426,\"likes\":5586,\"dislikes\":3686}]],[[{\"id\":\"rainbows\",\"rating\":0.9892053284336243,\"likes\":12921,\"dislikes\":141}],[{\"id\":\"pencils\",\"rating\":0.7901785714285714,\"likes\":12921,\"dislikes\":3431}],[{\"id\":\"mice\",\"rating\":0.5934873949579832,\"likes\":565,\"dislikes\":387}],[{\"id\":\"houses\",\"rating\":0.7303128371089536,\"likes\":677,\"dislikes\":250}],[{\"id\":\"money\",\"rating\":0.5328576521177283,\"likes\":95665,\"dislikes\":83867}]]]}";
        JSONNode json = JSON.Parse(jsonString);
        int batchSize = json["data"][round].Count; 
        int questionSize = json["data"][round][question].Count;

        // Create indicators
        Transform[] indicators = UtilitiesScript.CreateIndicators(round, indicatorPrefab);

        float timeToWait = 0f;

        // Transition texts
        StartCoroutine(UpdateAnchors("Title", 0f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("Description", 0f, Mathf.Infinity, Mathf.Infinity, timeToWait));
        StartCoroutine(UpdateAnchors("MyScore", Mathf.Infinity, -7.25f, Mathf.Infinity, timeToWait));
        if (versus) StartCoroutine(UpdateAnchors("OpponentScore", Mathf.Infinity, -7.25f, Mathf.Infinity, timeToWait));

        float wholeQuestionTime = ownAnswerTime + (versus ? opponentAnswerTime : 0f) + (realAnswerTime * (questionSize + 2f) + moveTime);

        for (int question = 0; question < batchSize; question++)
        {
            // Get data for this question
            timeToWait = wholeQuestionTime * question;
            string[] originalAnswersQuestion = new string[questionSize];
            float[] ratings = new float[questionSize];
            //Sprite[] realColours = new Sprite[questionSize];

            for (int t = 0; t < questionSize; t++)
            {
                originalAnswersQuestion[t] = json["data"][round][question][t]["id"];
                ratings[t] = json["data"][round][question][t]["rating"];
                //realColours[t] = colourOrder[t];
            }

            //SortAnswers(ref realAnswers, ref realColours, ratings);
            SortAnswers(originalAnswersQuestion, ratings);
            string[] realAnswers = SortAnswers(originalAnswersQuestion, ratings);
            Sprite[] realColours = DetermineColours(originalAnswersQuestion, realAnswers);

            string[,] myAnswers = FetchFullGameTopics.round3;
            //Sprite[,] myColours = FetchFullGameTopics.round3Colours;
            string[] myAnswersQuestion = new string[questionSize];
            //Sprite[] myColoursQuestion = new Sprite[questionSize];

            //string[,] opponentAnswers = original;
            //Sprite[,] opponentColours = myColours;

            string[] opponentAnswersQuestion = originalAnswersQuestion;
            //Sprite[] opponentColoursQuestion = new Sprite[questionSize];

            for (int i = 0; i < questionSize; i++)
            {
                //print("iteration: " + i); 
                myAnswersQuestion[i] = myAnswers[question, i];
                //myColoursQuestion[i] = DetermineColour(myAnswers, question, myAnswersQuestion[i]);
                //opponentAnswersQuestion[i] = opponentAnswersQuestion[i];
                //opponentColoursQuestion[i] = opponentColours[question, i];
            }
            Sprite[] myColoursQuestion = DetermineColours(originalAnswersQuestion, myAnswersQuestion);
            Sprite[] opponentColoursQuestion = DetermineColours(originalAnswersQuestion, opponentAnswersQuestion);

            Reshuffle(ref opponentAnswersQuestion, ref opponentColoursQuestion);

            // Update indicator colours 
            // previous to green
            if (question != 0) StartCoroutine(IndicatorColour(indicators[question - 1], "green", timeToWait));

            // current to yellow
            StartCoroutine(IndicatorColour(indicators[question], "yellow", timeToWait));

            // Create MyCards
            StartCoroutine(CreateCards(myAnswersQuestion, myColoursQuestion, "MyAnswer", timeToWait));
            timeToWait += ownAnswerTime; 
            StartCoroutine(UpdateAnchors("MyAnswer", -10.5f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            
            // Create OpponentCards
            if (versus)
            {
                StartCoroutine(CreateCards(opponentAnswersQuestion, opponentColoursQuestion, "OpponentAnswer", timeToWait));
                timeToWait += opponentAnswerTime;
                StartCoroutine(UpdateAnchors("OpponentAnswer", 10.5f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            }

            // Creat RealCards one by one, followed by tick/cross UI
            for (int i = 0; i < questionSize; i++)
            {
                StartCoroutine(CreateCard(realAnswers, realColours, i, "RealAnswer", timeToWait));
                timeToWait += realAnswerTime * 0.5f; 

                // add tick/cross UI
                bool myCorrect = (realAnswers[i] == myAnswersQuestion[i]);
                //print("Correct: " + myCorrect + ", real: " + realAnswers[i] + ", me: " + myAnswers[question, i]);
                StartCoroutine(MakeTickOrCross(myCorrect, i, false, timeToWait));
                if (myCorrect) StartCoroutine(IncrementScore(false, 125, timeToWait));
                if (versus)
                {
                    bool opponentCorrect = (realAnswers[i] == opponentAnswersQuestion[i]);
                    //print("Correct: " + opponentCorrect + ", real: " + realAnswers[i] + ", opponent: " + opponentAnswers[question, i]);
                    StartCoroutine(MakeTickOrCross(opponentCorrect, i, true, timeToWait));
                    if (opponentCorrect) StartCoroutine(IncrementScore(true, 125, timeToWait));
                }
                timeToWait += realAnswerTime * 0.5f;
            }
            timeToWait += realAnswerTime * 0.5f;

            // Move and destroy
            StartCoroutine(UpdateAnchors("MyAnswer", -12f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("OpponentAnswer", 12f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("RealAnswer", Mathf.Infinity, -13f, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("MyTickOrCross", -8.2f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            StartCoroutine(UpdateAnchors("OpponentTickOrCross", 8.2f, Mathf.Infinity, Mathf.Infinity, timeToWait));
            timeToWait += moveTime; 

            // Remove elements for this question
            StartCoroutine(DeleteAllCards(timeToWait - 0.1f));
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

    Sprite[] DetermineColours(string[] originalAnswers, string[] theseAnswers)
    {
        Sprite[] newCols = new Sprite[theseAnswers.Length];
        for (int j = 0; j < theseAnswers.Length; j++)
        {
            newCols[j] = DetermineColour(originalAnswers, theseAnswers[j]);
        }
        return newCols; 
    }

    Sprite DetermineColour(string[] originalAnswers, string thisAnswer)
    {
        for (int i = 0; i < originalAnswers.Length; i++)
        {
            if (originalAnswers[i] == thisAnswer) return colourOrder[i];
        }
        print("Colour problem: No answer match");
        return unansweredSprite;
    }

    void Reshuffle(ref string[] texts, ref Sprite[] sprites)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            string tmpT = texts[t];
            Sprite tmpS = sprites[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmpT;
            sprites[t] = sprites[r];
            sprites[r] = tmpS;
        }
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

    /*
    void MakeTickOrCrossDelay(bool correct, int i, bool isOpponent, float delay)
    {
        StartCoroutine(MakeTickOrCross(correct, i, isOpponent, delay));
    }
    */

    IEnumerator IndicatorColour(Transform indicator, string colour, float delay)
    {
        yield return new WaitForSeconds(delay);
        UtilitiesScript.IndicatorColour(indicator, colour);
    }

    IEnumerator MakeTickOrCross(bool correct, int i, bool isOpponent, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 feedbackPos = new Vector3((isOpponent ? 5.25f : -5.25f), -4f + ((float)i * 2.5f), 0f);
        Transform feedback = Instantiate(tickOrCross, feedbackPos, Quaternion.identity);
        feedback.GetComponent<SpriteRenderer>().sprite = (correct ? tick : cross);
        if (isOpponent) feedback.tag = "OpponentTickOrCross";
    }

    /*
    void GoToNextSceneDelay(float delay)
    {
        StartCoroutine(GoToNextScene(delay)); 
    }
    */

    IEnumerator GoToNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(nextLevel);
    }

    /*
    void DeleteAllCardsDelay(float delay)
    {
        StartCoroutine(DeleteAllCards( delay));
    }
    */

    IEnumerator DeleteAllCards(float delay)
    {
        yield return new WaitForSeconds(delay);
        string[] tagsToDestroy =
            {
                "RealAnswer",
                "MyAnswer",
                "OpponentAnswer",
                "MyTickOrCross",
                "OpponentTickOrCross"
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

    /*
    void CreateCardsDelay(string[] _answers, Sprite[] _colours, string _tag, float delay)
    {
        StartCoroutine(CreateCards(_answers, _colours, _tag,  delay));
    }
    */

    private IEnumerator CreateCards(string[] _answers, Sprite[] _colours, string _tag, float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < _answers.Length; i++)
        {
            UtilitiesScript.CreateCard(cardPrefab, textPrefab, _answers, _colours, i, _tag); 
        }
    }

    /*
    void CreateCardDelay(string[] _answers, Sprite[] _colours, int i, string _tag, float delay)
    {
        StartCoroutine(CreateCard(_answers, _colours, i, _tag, delay));
    }
    */

    private IEnumerator CreateCard(string[] _answers, Sprite[] _colours, int i, string _tag, float delay)
    {
        yield return new WaitForSeconds(delay);
        UtilitiesScript.CreateCard(cardPrefab, textPrefab, _answers, _colours, i, _tag);
    }



    private IEnumerator UpdateAnchors(string tag, float x, float y, float z, float delay)
    {
        yield return new WaitForSeconds(delay);
        // if any values are negative infinity, treat them as the transform.positional value 
        GameObject[] cards = GameObject.FindGameObjectsWithTag(tag);
        //print("Moving!"); 
        foreach (GameObject card in cards)
        {
            if (x != Mathf.Infinity)
            {
                if (card.tag.Contains("TickOrCross") || card.tag.Contains("Score") || card.tag.Contains("Description") || card.tag.Contains("Title") || card.tag.Contains("Indicator"))
                {
                    card.GetComponent<Ease>().alignmentX = x; 
                }
                else
                {
                    card.GetComponent<OrderBoxHeight>().alignmentX = x;
                }
            }

            if (y != Mathf.Infinity)
            {
                if (card.tag.Contains("TickOrCross") || card.tag.Contains("Score") || card.tag.Contains("Description") || card.tag.Contains("Title") || card.tag.Contains("Indicator"))
                {
                    card.GetComponent<Ease>().alignmentY = y;
                }
                else
                {
                    card.GetComponent<OrderBoxHeight>().alignmentY = y;
                }
            }
        }
    }


    string[] SortAnswers(string[] answers, /*ref Sprite[] colours,*/ float[] ratings)
    {
        string[] newAnswers = new string[answers.Length];
        //Sprite[] newColours = new Sprite[colours.Length];
        for (int i = 0; i < answers.Length; i++)
        {
            float myRating = ratings[i];
            int numAbove = 0;
            for (int j = 0; j < answers.Length; j++) 
            {
                float otherRating = ratings[j];
                if (i != j && otherRating < myRating)
                {
                    numAbove++;
                }
                
            }
            newAnswers[numAbove] = answers[i];
            //newColours[numAbove] = colours[i]; 
        }
        return newAnswers;
        //colours = newColours; 
        // purple, red, blue, greem
    }

    /*
    private void UpdateAnchorsDelay(string tag, float x, float y, float z, float secs)
    {
        StartCoroutine(UpdateAnchors(tag, x, y, z, secs)); 
        
        //UpdateAnchors(tag, x, y, z);
    }
    */
}
