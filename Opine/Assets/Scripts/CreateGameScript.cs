using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON; 

public class CreateGameScript : MonoBehaviour {

    public string gameID = "LOADING";
    public GameObject waitingText; 
    public float checkFrequency = 5f;
    bool isGameReady = false; 

	// Use this for initialization
	void Start () {
        GetComponent<TextMesh>().text = gameID;
	}
	
    IEnumerator GetGameID()
    {
        print("Creating new game in lobby collection");

        string getGameUrl = GlobalScript.domain + "/createGame";
        JSONNode sendJson = JSON.Parse("{}");
        sendJson["uuid"] = GlobalScript.uuid;
        sendJson["gameMode"] = "all";
        sendJson["playerCount"] = 2;
        string jsonData = sendJson.ToString(); 
        Hashtable headers = UtilitiesScript.CreateHeaders();
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(jsonData.ToCharArray());
        WWW www = new WWW(getGameUrl, pData, headers);

        yield return www;
        print(www.text);
        JSONNode recJson = JSON.Parse(www.text);
        gameID = recJson["data"]["id"];
        GetComponent<TextMesh>().text = gameID;
        waitingText.GetComponent<DotDotDot>().baseText = "Waiting for friend to join";
        StartCoroutine(LoopCheck());
    }

    IEnumerator LoopCheck()
    {
        yield return new WaitForSeconds(checkFrequency);
        StartCoroutine(LoopCheck());
        StartCoroutine(CheckGameReady());
    }

    IEnumerator CheckGameReady()
    {
        if (!isGameReady)
        {
            string checkGameUrl = GlobalScript.domain + "/checkGameReady";
            JSONNode sendJson = JSON.Parse("{}");
            sendJson["uuid"] = GlobalScript.uuid;
            sendJson["gameMode"] = "all";
            string jsonData = sendJson.ToString();
            Hashtable headers = UtilitiesScript.CreateHeaders();
            byte[] pData = System.Text.Encoding.UTF8.GetBytes(jsonData.ToCharArray());
            WWW www = new WWW(checkGameUrl, pData, headers);

            yield return www;
            print(www.text);
            JSONNode recJson = JSON.Parse(www.text);
            if (recJson["success"] == true)
            {
                //FetchFullGameTopics.topics = recJson["data"]["topics"];
                //TransitionToGame(); 
            }

        }
        

    }

	// Update is called once per frame
	void Update () {
		
	}
}
