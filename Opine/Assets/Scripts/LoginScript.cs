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

        string getUuidUrl = "http://104.131.63.157:3000/users/login"; 
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


        GameObject title = GameObject.FindGameObjectWithTag("Title");
        GameObject description = GameObject.FindGameObjectWithTag("Description");
        GameObject inputField = GameObject.FindGameObjectWithTag("InputField"); 
        title.GetComponent<Ease>().alignmentY = 14f;
        description.GetComponent<Ease>().alignmentY = 13f;
        inputField.GetComponent<Ease>().alignmentY = -300f;
        StartCoroutine(LoadDelay(0.5f));
    }

    IEnumerator LoadDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        Application.LoadLevel(nextLevel); 
    }

    private void Start()
    {

    }

    string[] Reshuffle(string[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            string tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }

        return texts;
    }
}
