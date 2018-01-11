using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using SimpleJSON; 

public class JoinGameScript : MonoBehaviour {

    public InputField inputField; 

	// Use this for initialization
	void Start () {
	
	}

    public void GetInput(string gameID)
    {
        StartCoroutine(JoinGame(gameID));
        inputField.text = "";
    }

    public void OnType(string gameID)
    {
        inputField.text = Regex.Replace(gameID, @"[^a-zA-Z0-9]", ""); // alphanumeric
    }

    IEnumerator JoinGame(string gameID)
    {
        string joinGameUrl = GlobalScript.domain + "/joinGame";
        JSONNode sendJson = JSON.Parse("{}");
        sendJson["uuid"] = GlobalScript.uuid;
        sendJson["gameID"] = gameID.ToUpper();
        string jsonData = sendJson.ToString();
        Hashtable headers = UtilitiesScript.CreateHeaders();
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(jsonData.ToCharArray());
        WWW www = new WWW(joinGameUrl, pData, headers);

        yield return www;
        print(www.text);
        JSONNode recJson = JSON.Parse(www.text);
        if (recJson["success"] == true)
        {
            //FetchFullGameTopics.topics = recJson["data"]["id"];        
            // GoToNextScene();
            // GetTopics
        } else
        {
            print("Something went wrong!"); // add message
        }
        

    }

    // Update is called once per frame
    void Update () {
		
	}
}
