using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System; 

static public class UtilitiesScript {

    //public static Transform indicator;
    //public static Transform cardPrefab;
    //public static Transform oText;


    public static void HideIndicators()
    {
        GameObject[] indicators = GameObject.FindGameObjectsWithTag("Indicator");

        foreach (GameObject indicator in indicators)
        {
            indicator.GetComponent<Ease>().alignmentY = 14f; 
        }
    }

	public static Transform[] CreateIndicators(int round, Transform indicator)
    {
        // Create initial indicators based on batch size
        string jsonString = FetchFullGameTopics.www.text;
        //string jsonString = "{\"success\":true,\"data\":[[[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667}],[{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364}],[{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257}],[{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327}],[{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"elephants\",\"rating\":0.6986486486486486,\"likes\":1034,\"dislikes\":446}],[{\"id\":\"yachts\",\"rating\":0.2879690538248314,\"likes\":13921,\"dislikes\":34421}],[{\"id\":\"journals\",\"rating\":0.29131989623483007,\"likes\":106797,\"dislikes\":259800}],[{\"id\":\"beavers\",\"rating\":0.20733944954128442,\"likes\":113,\"dislikes\":432}],[{\"id\":\"biscuits\",\"rating\":0.7298632218844985,\"likes\":1921,\"dislikes\":711}]],[[{\"id\":\"yoghurts\",\"rating\":0.7269151478311769,\"likes\":678057,\"dislikes\":254730},{\"id\":\"gerbils\",\"rating\":0.99366921656555,\"likes\":11301,\"dislikes\":72}],[{\"id\":\"panthers\",\"rating\":0.6368086147821831,\"likes\":1301,\"dislikes\":742},{\"id\":\"maggots\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211}],[{\"id\":\"cats\",\"rating\":0.7541899441340782,\"likes\":135,\"dislikes\":44},{\"id\":\"superheroes\",\"rating\":0.5942489666603722,\"likes\":56645,\"dislikes\":38677}],[{\"id\":\"cheese\",\"rating\":0.5339358685215372,\"likes\":95645,\"dislikes\":83487},{\"id\":\"radios\",\"rating\":0.7260589303729257,\"likes\":674357,\"dislikes\":254434}],[{\"id\":\"gravy\",\"rating\":0.6360566134405463,\"likes\":568585,\"dislikes\":325337},{\"id\":\"uni\",\"rating\":0.724858696811347,\"likes\":6797,\"dislikes\":2580}],[{\"id\":\"vanilla\",\"rating\":0.6375423557513622,\"likes\":586655,\"dislikes\":333527},{\"id\":\"noodles\",\"rating\":0.7169585987261147,\"likes\":1801,\"dislikes\":711}],[{\"id\":\"salt\",\"rating\":0.7217311541027351,\"likes\":8655,\"dislikes\":3337},{\"id\":\"cigarettes\",\"rating\":0.6425592625109745,\"likes\":5855,\"dislikes\":3257}],[{\"id\":\"pyjamas\",\"rating\":0.9359046608764835,\"likes\":56465,\"dislikes\":3867},{\"id\":\"deserts\",\"rating\":0.6359325218876215,\"likes\":56825685,\"dislikes\":32532357}],[{\"id\":\"pickles\",\"rating\":0.5339995533720411,\"likes\":9565,\"dislikes\":8347},{\"id\":\"leopards\",\"rating\":0.20764424843807425,\"likes\":1130,\"dislikes\":4312}],[{\"id\":\"beess\",\"rating\":0.4893324156916724,\"likes\":711,\"dislikes\":742},{\"id\":\"santa\",\"rating\":0.9229403868456543,\"likes\":4123921,\"dislikes\":344321}]],[[{\"id\":\"june\",\"rating\":0.6414473684210527,\"likes\":585,\"dislikes\":327},{\"id\":\"fleas\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211},{\"id\":\"lollipops\",\"rating\":0.7259781237122767,\"likes\":67407357,\"dislikes\":25443040},{\"id\":\"chips\",\"rating\":0.5329241071428571,\"likes\":955,\"dislikes\":837}],[{\"id\":\"ladybirds\",\"rating\":0.6360188831215321,\"likes\":56855,\"dislikes\":32537},{\"id\":\"cockrels\",\"rating\":0.14956109188421077,\"likes\":13051,\"dislikes\":74211},{\"id\":\"cows\",\"rating\":0.01342549514821215,\"likes\":101,\"dislikes\":7422},{\"id\":\"buildings\",\"rating\":0.7267935893298914,\"likes\":6757,\"dislikes\":2540}],[{\"id\":\"chocolate\",\"rating\":0.9478433098591549,\"likes\":12921,\"dislikes\":711},{\"id\":\"oceans\",\"rating\":0.6785849894934803,\"likes\":682685,\"dislikes\":323357},{\"id\":\"flamingos\",\"rating\":0.001996007984031936,\"likes\":2,\"dislikes\":1000},{\"id\":\"clocks\",\"rating\":0.6024590163934426,\"likes\":5586,\"dislikes\":3686}]],[[{\"id\":\"rainbows\",\"rating\":0.9892053284336243,\"likes\":12921,\"dislikes\":141}],[{\"id\":\"pencils\",\"rating\":0.7901785714285714,\"likes\":12921,\"dislikes\":3431}],[{\"id\":\"mice\",\"rating\":0.5934873949579832,\"likes\":565,\"dislikes\":387}],[{\"id\":\"houses\",\"rating\":0.7303128371089536,\"likes\":677,\"dislikes\":250}],[{\"id\":\"money\",\"rating\":0.5328576521177283,\"likes\":95665,\"dislikes\":83867}]]]}";
        JSONNode json = JSON.Parse(jsonString);
        int totalBatches = json["data"][round].Count;
        //Debug.Log("Creating " + totalBatches + " indicators");

        Transform[] indicators = new Transform[totalBatches];

        for (int i = 0; i < totalBatches; i++)
        {
            // create and assign to indicators[]
            float gap = 1.5f;
            float newX = gap * (i - ((totalBatches - 1f) / 2f));
            //Debug.Log("New X: " + newX);
            Transform inst = Transform.Instantiate(indicator, new Vector3(newX, 14f, 10), Quaternion.identity) as Transform; 
            indicators[i] = inst;
            inst.GetComponent<IndicatorColour>().current = (i == 0);
            inst.GetComponent<IndicatorColour>().iteration = i;
            inst.GetComponent<Ease>().alignmentY = 9f; 

            int batchSize = json["data"][round][i].Count;
            string[] localAnswers = new string[batchSize];
            for (int t = 0; t < batchSize; t++)
            {
                localAnswers[t] = json["data"][round][i][t]["id"];
                //Debug.Log("Local answer " + t + ": " + localAnswers[t]);
            }

            inst.GetComponent<IndicatorColour>().myAnswers = localAnswers;
        }

        return indicators;
    }

    public static Transform[] CreateVoteIndicators(int round, Transform indicator)
    {
        // Create initial indicators based on batch size
        string jsonString = FetchFullGameTopics.wwwV.text;
        //string jsonString = "{\"success\":true,\"data\":[[[{\"id\":\"work\",\"rating\":0.01438905768877832,\"likes\":5645,\"dislikes\":386667}],[{\"id\":\"cheetos\",\"rating\":0.6005595315549772,\"likes\":553836,\"dislikes\":368364}],[{\"id\":\"lemons\",\"rating\":0.6377706617871303,\"likes\":58555,\"dislikes\":33257}],[{\"id\":\"tomatos\",\"rating\":0.72223333500025,\"likes\":86655,\"dislikes\":33327}],[{\"id\":\"foxes\",\"rating\":0.6005384138490669,\"likes\":55378836,\"dislikes\":36836474}],[{\"id\":\"elephants\",\"rating\":0.6986486486486486,\"likes\":1034,\"dislikes\":446}],[{\"id\":\"yachts\",\"rating\":0.2879690538248314,\"likes\":13921,\"dislikes\":34421}],[{\"id\":\"journals\",\"rating\":0.29131989623483007,\"likes\":106797,\"dislikes\":259800}],[{\"id\":\"beavers\",\"rating\":0.20733944954128442,\"likes\":113,\"dislikes\":432}],[{\"id\":\"biscuits\",\"rating\":0.7298632218844985,\"likes\":1921,\"dislikes\":711}]],[[{\"id\":\"yoghurts\",\"rating\":0.7269151478311769,\"likes\":678057,\"dislikes\":254730},{\"id\":\"gerbils\",\"rating\":0.99366921656555,\"likes\":11301,\"dislikes\":72}],[{\"id\":\"panthers\",\"rating\":0.6368086147821831,\"likes\":1301,\"dislikes\":742},{\"id\":\"maggots\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211}],[{\"id\":\"cats\",\"rating\":0.7541899441340782,\"likes\":135,\"dislikes\":44},{\"id\":\"superheroes\",\"rating\":0.5942489666603722,\"likes\":56645,\"dislikes\":38677}],[{\"id\":\"cheese\",\"rating\":0.5339358685215372,\"likes\":95645,\"dislikes\":83487},{\"id\":\"radios\",\"rating\":0.7260589303729257,\"likes\":674357,\"dislikes\":254434}],[{\"id\":\"gravy\",\"rating\":0.6360566134405463,\"likes\":568585,\"dislikes\":325337},{\"id\":\"uni\",\"rating\":0.724858696811347,\"likes\":6797,\"dislikes\":2580}],[{\"id\":\"vanilla\",\"rating\":0.6375423557513622,\"likes\":586655,\"dislikes\":333527},{\"id\":\"noodles\",\"rating\":0.7169585987261147,\"likes\":1801,\"dislikes\":711}],[{\"id\":\"salt\",\"rating\":0.7217311541027351,\"likes\":8655,\"dislikes\":3337},{\"id\":\"cigarettes\",\"rating\":0.6425592625109745,\"likes\":5855,\"dislikes\":3257}],[{\"id\":\"pyjamas\",\"rating\":0.9359046608764835,\"likes\":56465,\"dislikes\":3867},{\"id\":\"deserts\",\"rating\":0.6359325218876215,\"likes\":56825685,\"dislikes\":32532357}],[{\"id\":\"pickles\",\"rating\":0.5339995533720411,\"likes\":9565,\"dislikes\":8347},{\"id\":\"leopards\",\"rating\":0.20764424843807425,\"likes\":1130,\"dislikes\":4312}],[{\"id\":\"beess\",\"rating\":0.4893324156916724,\"likes\":711,\"dislikes\":742},{\"id\":\"santa\",\"rating\":0.9229403868456543,\"likes\":4123921,\"dislikes\":344321}]],[[{\"id\":\"june\",\"rating\":0.6414473684210527,\"likes\":585,\"dislikes\":327},{\"id\":\"fleas\",\"rating\":0.14917108069065144,\"likes\":13011,\"dislikes\":74211},{\"id\":\"lollipops\",\"rating\":0.7259781237122767,\"likes\":67407357,\"dislikes\":25443040},{\"id\":\"chips\",\"rating\":0.5329241071428571,\"likes\":955,\"dislikes\":837}],[{\"id\":\"ladybirds\",\"rating\":0.6360188831215321,\"likes\":56855,\"dislikes\":32537},{\"id\":\"cockrels\",\"rating\":0.14956109188421077,\"likes\":13051,\"dislikes\":74211},{\"id\":\"cows\",\"rating\":0.01342549514821215,\"likes\":101,\"dislikes\":7422},{\"id\":\"buildings\",\"rating\":0.7267935893298914,\"likes\":6757,\"dislikes\":2540}],[{\"id\":\"chocolate\",\"rating\":0.9478433098591549,\"likes\":12921,\"dislikes\":711},{\"id\":\"oceans\",\"rating\":0.6785849894934803,\"likes\":682685,\"dislikes\":323357},{\"id\":\"flamingos\",\"rating\":0.001996007984031936,\"likes\":2,\"dislikes\":1000},{\"id\":\"clocks\",\"rating\":0.6024590163934426,\"likes\":5586,\"dislikes\":3686}]],[[{\"id\":\"rainbows\",\"rating\":0.9892053284336243,\"likes\":12921,\"dislikes\":141}],[{\"id\":\"pencils\",\"rating\":0.7901785714285714,\"likes\":12921,\"dislikes\":3431}],[{\"id\":\"mice\",\"rating\":0.5934873949579832,\"likes\":565,\"dislikes\":387}],[{\"id\":\"houses\",\"rating\":0.7303128371089536,\"likes\":677,\"dislikes\":250}],[{\"id\":\"money\",\"rating\":0.5328576521177283,\"likes\":95665,\"dislikes\":83867}]]]}";
        JSONNode json = JSON.Parse(jsonString);
        int totalBatches = json["data"]["topics"][round].Count;
        Debug.Log("Creating " + totalBatches + " indicators for round " + round);

        Transform[] indicators = new Transform[totalBatches];

        for (int i = 0; i < totalBatches; i++)
        {
            // create and assign to indicators[]
            float gap = 1.5f;
            float newX = gap * (i - ((totalBatches - 1f) / 2f));
            Debug.Log("New X: " + newX);
            Transform inst = Transform.Instantiate(indicator, new Vector3(newX, 14f, 10), Quaternion.identity) as Transform;
            indicators[i] = inst;
            inst.GetComponent<IndicatorColour>().current = (i == 0);
            inst.GetComponent<IndicatorColour>().iteration = i;
            inst.GetComponent<Ease>().alignmentY = 8f;

            int batchSize = json["data"]["topics"][round][i].Count;
            string[] localAnswers = new string[batchSize];
            for (int t = 0; t < batchSize; t++)
            {
                localAnswers[t] = json["data"][round][i][t]["id"];
                Debug.Log("Local answer " + t + ": " + localAnswers[t]);
            }

            inst.GetComponent<IndicatorColour>().myAnswers = localAnswers;
        }

        return indicators;
    }


    /*
    static public IEnumerator CreateCards(int round, int batch, float secs)
    {
        yield return new WaitForSeconds(secs);
        CreateCards(round, batch);
    }
    */

    public static Transform[] CreateTopDogCards(Transform cardPrefab, Transform textPrefab, string topic1, Sprite sprite1, string topic2, Sprite sprite2)
    {
        // Card1
        Vector3 loc1 = new Vector3(-12f, 2f, 0f);
        Vector3 rot1 = new Vector3(0, 0, 12); 
        Transform instCard1 = UnityEngine.Object.Instantiate(cardPrefab, loc1, Quaternion.Euler(rot1)) as Transform;
        instCard1.GetComponent<TopDogCard>().topic = topic1;
        instCard1.GetComponent<Ease>().alignmentX = -2.8f;
        instCard1.GetComponent<TopDogCard>().boxType = "1";
        instCard1.GetComponent<SpriteRenderer>().sprite = sprite1; 

        Transform instText1 = Transform.Instantiate(textPrefab, loc1, Quaternion.Euler(rot1)) as Transform;
        instText1.GetComponent<LockCoords>().anchor = instCard1;
        instText1.GetComponent<TextMesh>().text = topic1;

        // Card2
        Vector3 loc2 = new Vector3(12.1f, -2f, 0f);
        Vector3 rot2 = new Vector3(0, 0, -12);
        Transform instCard2 = UnityEngine.Object.Instantiate(cardPrefab, loc2, Quaternion.Euler(rot2)) as Transform;
        instCard2.GetComponent<TopDogCard>().topic = topic2;
        instCard2.GetComponent<Ease>().alignmentX = 2.8f;
        instCard2.GetComponent<TopDogCard>().boxType = "2";
        instCard2.GetComponent<SpriteRenderer>().sprite = sprite2;

        Transform instText2 = Transform.Instantiate(textPrefab, loc2, Quaternion.Euler(rot2)) as Transform;
        instText2.GetComponent<LockCoords>().anchor = instCard2;
        instText2.GetComponent<TextMesh>().text = topic2;

        // Collate
        Transform[] cards = { instCard1, instCard2 };
        return cards; 
    }
    
    // Pecking order
    public static Transform[] CreateCards(Transform cardPrefab, Transform textPrefab, string[] topics, Sprite[] colours, string tag)
    {
        /*
        GameObject[] gIndicators = GameObject.FindGameObjectsWithTag("Indicator");
        Transform[] indicators = new Transform[gIndicators.Length];
        int _roundSize = gIndicators.Length;
        batch %= _roundSize;
        Transform indInst = gIndicators[batch].GetComponent<Transform>();
        int _batchSize = indInst.GetComponent<IndicatorColour>().myAnswers.Length;
        */
        int _batchSize = topics.Length; 
        Transform[] cards = new Transform[_batchSize];
        Debug.Log("Creating " + _batchSize + " cards");
        for (int t = 0; t < _batchSize; t++)
        {
            cards[t] = CreateCard(cardPrefab, textPrefab, topics, colours, t, tag);
            //CreateCard(indInst, t); 
        }
        return cards;
    }

    // Pinpoint
    public static Transform CreatePinpointOpinionGraph(Transform graphPrefab, string topic, string percentage) // percentage = "0.55"
    {
        Vector3 loc = new Vector3(-10, -6.5f, 0);
        Transform graph = Transform.Instantiate(graphPrefab, loc, Quaternion.identity);
        graph.GetComponent<GraphSelect>().topic = topic;
        graph.GetComponent<GraphSelect>().ratio = float.Parse(percentage, System.Globalization.CultureInfo.InvariantCulture);
        graph.GetComponent<Ease>().alignmentX = 0; 
        
        return graph; 

    }

    // Pinpoint     e.g. 0.34 > "34%"
    public static string DecimalToPercentage(float ratio)
    {
        float percentFloat = ratio * 100f;
        int percentInt = Mathf.FloorToInt(percentFloat + 0.5f);
        string percentText = percentInt.ToString() + "%";
        return percentText;
    }

    // Pinpoint
    public static void UpdatePercentageDisplay(Transform textInst, float ratio, float x, float y)
    {
        textInst.GetComponent<TextMesh>().text = DecimalToPercentage(ratio);
        Vector3 percentInstPos = textInst.GetComponent<Transform>().position;
        percentInstPos.x = x; 
        percentInstPos.y = y;
        textInst.GetComponent<Transform>().position = percentInstPos;
    }

    /*
    public static Transform CreateCard(Transform cardPrefab, Transform textPrefab, Transform indInst, int t)
    {
        Transform instCard;
        Vector3 loc = new Vector3(0, 10f + (t * 0.5f), 0f - (t * 1f));
        instCard = UnityEngine.Object.Instantiate(cardPrefab, loc, Quaternion.identity) as Transform;

        string topic = indInst.GetComponent<IndicatorColour>().myAnswers[t];
        Sprite colour = indInst.GetComponent<IndicatorColour>().colours[t];
        //string topic = _json["data"][_round][_batch][t]["id"];
        //instCard.GetComponentInChildren<TextMesh>().text = topic; 
        Transform instText;
        instText = Transform.Instantiate(textPrefab, loc, Quaternion.identity) as Transform;
        instText.GetComponent<LockCoords>().anchor = instCard;
        instText.GetComponent<TextMesh>().text = topic;
        instCard.GetComponent<OrderBoxHeight>().topic = topic;
        instCard.GetComponent<SpriteRenderer>().sprite = colour;
        Debug.Log("Assigning topic " + topic + " to card " + t);

        return instCard; 
    }
    */

    public static Transform CreateCard(Transform cardPrefab, Transform textPrefab, string[] topics, Sprite[] colours, int t, string tag)
    {
        Transform instCard;
        Vector3 loc = new Vector3(0, 10f + (t * 0.5f), 0f - (t * 1f));
        instCard = UnityEngine.Object.Instantiate(cardPrefab, loc, Quaternion.identity) as Transform;

        //string topic = indInst.GetComponent<IndicatorColour>().myAnswers[t];
        //Sprite colour = indInst.GetComponent<IndicatorColour>().colours[t];
        //string topic = _json["data"][_round][_batch][t]["id"];
        //instCard.GetComponentInChildren<TextMesh>().text = topic; 
        string topic = topics[t];
        Sprite colour = colours[t];

        Transform instText;
        instText = Transform.Instantiate(textPrefab, loc, Quaternion.identity) as Transform;
        instText.GetComponent<LockCoords>().anchor = instCard;
        instText.GetComponent<TextMesh>().text = topic;
        instCard.GetComponent<OrderBoxHeight>().topic = topic;
        instCard.GetComponent<SpriteRenderer>().sprite = colour;
        instCard.tag = tag; 
        Debug.Log("Assigning topic " + topic + " to card " + t);

        return instCard;
    }

    public static Transform CreateYayOrNayCard(Transform cardPrefab, Transform textPrefab, string topic)
    {
        Vector3 loc = new Vector3(0, 15f, -0.2f);

        Transform instCard = UnityEngine.Object.Instantiate(cardPrefab, loc, Quaternion.identity) as Transform;
        instCard.GetComponent<SwipeScript>().topic = topic;

        Transform instText = Transform.Instantiate(textPrefab, loc, Quaternion.identity) as Transform;
        instText.GetComponent<LockCoords>().anchor = instCard;
        instText.GetComponent<TextMesh>().text = topic;

        return instCard;
    }

    public static void IndicatorColour(Transform indicator, string colour)
    {
        var script = indicator.GetComponent<IndicatorColour>();
        if (colour == "green") script.answered = true;
        if (colour == "yellow") script.current = true;
        script.shouldUpdate = true;
    }
}
