using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON; 

public class LoginScript : MonoBehaviour {


    public string nextLevel; 
    public InputField inputField;
    [HideInInspector] public string uuid; 

    public void GetInput(string username)
    {
        print("yo!");
        StartCoroutine(GetUuid(username));
        inputField.text = ""; 
    }

    IEnumerator GetUuid(string username) {
        Debug.Log("Logging in as: " + username);
        print("Retrieving UUID from server (or creating one)");

        string getUuidUrl = "http://192.168.1.111:3000/users/login"; 
        string jsonData = "{\"username\": \"" + username + "\"}"; 
        Hashtable headers = new Hashtable();
        headers.Add("Content-Type", "application/json");
        headers.Add("Cookie", "Our session cookie");
        byte[] pData = System.Text.Encoding.UTF8.GetBytes(jsonData.ToCharArray());
        WWW www = new WWW(getUuidUrl, pData, headers);

        yield return www;
        JSONNode json = JSON.Parse(www.text);
        uuid = json["data"]["uuid"];
        GlobalScript.uuid = uuid;
        //print(uuid + " DELETE THIS WARNING SOON"); 

        Application.LoadLevel(nextLevel); 
    }
}
