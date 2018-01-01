using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;

public class HttpGetExample : MonoBehaviour {

    public string getUrl = "http://uinames.com/api/?region=germany";
    //public string getUrl = "https://randomuser.me/api/";
    public string postTopicsUrl = "http://192.168.1.111:3000/getTopicsAndAnswers";

    static string[] allTopics;
    static int[] allRatings; 

    //string[][][] rounds;
    //float[][][] ratings;

    //string[,,] myRounds = new string[,,]{};
    //float[,,] myRatings = new float[,,]{};

    public static JSONNode N;
    public static WWW www; 

    public static bool waiting;

    // Use this for initialization
    void Start() {
        waiting = true; 
        StartCoroutine("LoadName");
    }

    private IEnumerator LoadName()
    {

        //print("getting name from " + getUrl);
        WWW getName = new WWW(getUrl);
        yield return getName;
        print(getName.text);
        
        UInameObject nameObject = JsonUtility.FromJson<UInameObject>(getName.text);

        /*
        print("Name: " + nameObject.name);
        print("Surname: " + nameObject.surname);
        print("Gender: " + nameObject.gender);
        print("Region: " + nameObject.region);
        */



        //C#
        string jsonData = "{ \"rounds\": [{\"batches\":10,\"size\":1},{\"batches\":10,\"size\":2},{\"batches\":3,\"size\":4},{\"batches\":5,\"size\":1}]}";


        Hashtable headers = new Hashtable();
        headers.Add("Content-Type", "application/json");
        headers.Add("Cookie", "Our session cookie");

        byte[] pData = System.Text.Encoding.UTF8.GetBytes(jsonData.ToCharArray()); 
 
        www = new WWW(postTopicsUrl, pData, headers);

        yield return www;
        print(www.text);
        

        //InitialObject returnedData = JsonUtility.FromJson<InitialObject>(www.text);

        //print("Success: " + returnedData.success);
        //print("Data: " + returnedData.data);


        JSONNode N = JSON.Parse(www.text);
        waiting = false;
        //print(N);
        //// example
        //string val = N["data"][0][0][0]["id"];
        // asignment: N["data"][0][0][0]["id"] = "LEL";
        

        

        /*
        print(roundCount);

        int batches = N["data"][0].Count;
        print(batches);

        int topics = N["data"][0][0].Count;
        print(topics);
        */







        /*
        rounds = new string[][][] {};

        ArrayList firstDimension = new ArrayList();
        int roundCount = N["data"].Count;
        for (int i = 0; i < roundCount; i++)
        {
            ArrayList secondDimensionChild = new ArrayList();
            int batchCount = N["data"][i].Count;
            for (int j = 0; j < batchCount; j++)
            {
                ArrayList thirdDimensionChild = new ArrayList();
                int topicCount = N["data"][i][j].Count;
                for (int k = 0; k < topicCount; k++)
                {
                    print("assigning to array");
                    //rounds[i][j][k] = "hi";
                    //print(rounds[i][j][k]);

                    //rounds[i][j][k] = N["data"][i][j][k]["id"];
                    //ratings[i][j][k] = Mathf.Floor(N["data"][i][j][k]["rating"] + 0.5f); // Round to nearest %
                    //print("Round " + i + ", Question " + j + ", Topic " + k + ": " + rounds[i][j][k] + " " + ratings[i][j][k] + "%");
                    //   thirdDimensionChild.Add(N["data"][i][j][k]["id"]);
                    game1 = ["hi", "man", "people"];
                }
                secondDimensionChild.Add(thirdDimensionChild);
                
            }
            firstDimension.Add(secondDimensionChild);
        }
        string value3 = (string) firstDimension[0][0][0];
        */

        //ArrayObject rounds = JsonUtility.FromJson<ArrayObject>(www.text);

        //print(rounds.success);

        /*
        string jsonData = "{rounds:[{\"batches\":10,\"size\":1},{\"batches\":10,\"size\":2},{\"batches\":3,\"size\":4},{\"batches\":5,\"size\":1}]}";
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest www = UnityWebRequest.Put(postTopicsUrl, myData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.downloadHandler.text); 
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
        */






        /*
        WWWForm form = new WWWForm();
        // Username - this is irrelevent - needed for post request
        form.AddField("useless", "useless");

        UnityWebRequest wwwPostTopics = UnityWebRequest.Post(postTopicsUrl, form);
        Debug.Log("URL:" + postTopicsUrl);

        string jsonData = "";
        jsonData = "{rounds:[{\"batches\":10,\"size\":1},{\"batches\":10,\"size\":2},{\"batches\":3,\"size\":4},{\"batches\":5,\"size\":1}]}";

        if (jsonData != null)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);

            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            upHandler.contentType = "application/json";
            wwwPostTopics.uploadHandler = upHandler;
        }

        yield return wwwPostTopics.SendWebRequest();

        if (wwwPostTopics.isNetworkError)
        {
            Debug.Log(wwwPostTopics.error);
        }
        else
        {
            // Show results as text
            Debug.Log("Returning:" + wwwPostTopics.downloadHandler.text);


            //JSONObject j = new JSONObject(wwwSignin.downloadHandler.text);

        }
        */



        /*
        // Create json request
        string jsonString = "{rounds:[{\"batches\":10,\"size\":1},{\"batches\":10,\"size\":2},{\"batches\":3,\"size\":4},{\"batches\":5,\"size\":1}]}";
        //string jsonJson = Json

        WWWForm form = new WWWForm();
        //form.AddField("rounds", jsonString);
        form.AddField("rounds", "batches");
        Dictionary<string,string> headers = form.headers;
       // Hashtable headers = form.headers; 
        byte[] rawData = form.data;

        // Post a request to an URL with our custom headers
        WWW getTopics = new WWW(postTopicsUrl, rawData, form.headers);
        yield return getTopics;
        //.. process results from WWW request here...
        print(getTopics.text);
        */

        /*
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        Hashtable postHeader = new Hashtable();

        postHeader.Add("Content-Type", "text/json"); // application/json? 
        postHeader.Add("Content-Length", jsonString.Length);

        //byte[] body = encoding.UTF8.GetBytes(jsonString);

        print("jsonString: " + jsonString);

        */
        //WWW getTopics = new WWW(postTopicsUrl, encoding.GetBytes(jsonString), postHeader);


        //WWW getTopics = new WWW(postTopicsUrl, jsonRequest);

        /*
        yield return getTopics;
        print(getTopics.text);

        ArrayObject rounds = JsonUtility.FromJson<ArrayObject>(getTopics.text);
        print("rounds"); 

        */
        /*
        for (int i = 0; i < rounds.contents.Length; i++)
        {
            object[] batches = rounds.contents[i];
            for (int j = 0; j < batches.length; j++) {

            }
        }
        


        TopicObject topics = JsonUtility.FromJson<TopicObject>(getTopics.text); 
        */

        /*
        JArray array = JArray.Parse(json);

        foreach (JObject content in array.Children<JObject>())
        {
            foreach (JProperty prop in content.Properties())
            {
                Console.WriteLine(prop.Name);
            }
        }*/

    }

    private class TopicObject
    {
        public string id;
        public string likes;
        public string dislikes;
        public string rating;
    }


    [System.Serializable]
    private class UInameObject {
        public string name;
        public string surname;
        public string gender;
        public string region; 
    }


    [System.Serializable]
    private class Topic
    {
        public string id;
        public float rating; 
        public float likes;
        public int dislikes; 
    }

    [System.Serializable]
    private class ArrayObject
    {
        public Topic[][][] contents; 
    }

    [System.Serializable]
    private class InitialObject
    {
        public bool success;
        public ArrayObject data;
    }

	// Update is called once per frame
	void Update () {
        //print("I'm still running!");	
	}
}
