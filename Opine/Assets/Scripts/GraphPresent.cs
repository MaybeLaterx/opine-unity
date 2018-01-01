using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPresent : MonoBehaviour {

    public float h;
    float w, ratio, speed, minimumOffset, maximumSize, scalableAmount;
    [HideInInspector] public float rating = 1.00f;

    [HideInInspector] public bool holdAtStartingValue;

    public Transform percentPrefab, bgPrefab;
    Transform percentInst, bgInst;

    GameObject bgObj, percentObj;

    // Use this for initialization
    void Start () {
        w = 2f;
        ratio = 0f; // 0-1
        // rating is assigned by creator
        speed = 0.04f;

        holdAtStartingValue = true;

        minimumOffset = 0.5f;
        maximumSize = GetComponent<SpriteRenderer>().size.y;
        //scalableAmount = maximumSize - minimumOffset;

        GetComponent<SpriteRenderer>().size = new Vector2(w, minimumOffset);
        percentInst = Instantiate(percentPrefab, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        percentObj = percentInst.gameObject;

        bgInst = Instantiate(bgPrefab, transform.position, Quaternion.identity);
        bgInst.GetComponent<LockCoordsOffset>().anchor = gameObject.transform;
        bgInst.GetComponent<LockCoordsOffset>().offsetY = -0.4f;
        bgInst.GetComponent<LockCoordsOffset>().offsetZ = 0.5f;
        bgObj = bgInst.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        // Lerp to new size
        if (!holdAtStartingValue)
        {
            ratio = Mathf.Lerp(ratio, rating, speed);
            //ratio += addition;
            //print("adding!");
        }
        h = (ratio * (maximumSize - minimumOffset)) + minimumOffset;
        //print(h); 
        GetComponent<SpriteRenderer>().size = new Vector2(w, h); // 0.5 - 6.00

        
        UtilitiesScript.UpdatePercentageDisplay(percentInst, ratio, transform.position.x, transform.position.y + h + 0.5f);
    }

    private void OnDestroy()
    {
        Destroy(percentObj);
        Destroy(bgObj);
    }
}
