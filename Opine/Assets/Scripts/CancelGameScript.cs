using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class CancelGameScript : MonoBehaviour {

    public Transform controller;
    string defaultID; 

    void Start()
    {
        defaultID = controller.GetComponent<CreateGameScript>().gameID;
    }

    private void OnMouseUp()
    {
        string gameID = controller.GetComponent<CreateGameScript>().gameID;
        if (gameID != defaultID)
        {
            StartCoroutine(CancelGame(gameID)); 
        }
    }

    IEnumerator CancelGame(string gameID)
    {
        string cancelGameUrl = GlobalScript.domain + "/cancelGame";
        JSONNode sendJson = JSON.Parse("{}");
        sendJson["gameID"] = gameID;
        string jsonData = sendJson.ToString();
        Hashtable headers = UtilitiesScript.CreateHeaders();
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(jsonData.ToCharArray());
        WWW www = new WWW(cancelGameUrl, pData, headers);

        // below will never run
        yield return www;
        print(www.text);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
