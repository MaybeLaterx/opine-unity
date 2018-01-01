using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpExample : MonoBehaviour {

    public string url = "192.168.1.111:3000/submitvotes";

    /*
    void Start()
    {
        MyTopic[] topicObjects;
        topicObjects[0] = new MyTopic();
        topicObjects[0].id = "giraffes";
        topicObjects[0].vote = true;

        topicObjects[1] = new MyTopic();
        topicObjects[1].id = "llamas";
        topicObjects[1].vote = false;

        MyData dataObject = new MyData();
        dataObject.uuid = "UNITY1212";
        dataObject.topics = topicObjects;

        string json = JsonUtility.ToJson(dataObject);

    }

    
    public WWW POST()
    {
        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(jsonStr);

        www = new WWW(POSTAddUserURL, formData, postHeader);
        StartCoroutine(WaitForRequest(www));
        return www;
    }


    IEnumerator Start()
    {
        WWW www = new WWW(url);
        yield return www;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
    }


    [Serializable]
    public class MyTopic
    {
        public string id; 
        public bool vote;
    }

    public class MyData
    {
        public string uuid;
        public object topics; 
    }
    */
}
