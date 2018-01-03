using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSelect : MonoBehaviour {

    public float ratio = 0.5f; // overwritten by instantiation
    float w, h, speed, rating, minimumOffset, maximumSize, scalableAmount;
    bool mouseDown;

    RaycastHit hit;
    public Transform percentPrefab, bgPrefab, cardPrefab, textPrefab;
    Transform percentInst, bgInst, cardInst, topicInst;

    GameObject bgObj, percentObj, cardObj, topicObj; 

    public string topic; 

	// Use this for initialization
	void Start () {
        w = GetComponent<SpriteRenderer>().size.x;
        
        minimumOffset = 0.5f;
        maximumSize = GetComponent<SpriteRenderer>().size.y;
        //scalableAmount = maximumSize - minimumOffset;

        h = (ratio * (maximumSize - minimumOffset)) + minimumOffset; // start at specified ratio

        // percent
        percentInst = Instantiate(percentPrefab, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        GameObject percentObj = percentInst.gameObject;

        // background
        bgInst = Instantiate(bgPrefab, transform.position, Quaternion.identity);
        bgInst.GetComponent<LockCoordsOffset>().anchor = gameObject.transform;
        bgInst.GetComponent<LockCoordsOffset>().offsetY = -0.4f;
        bgInst.GetComponent<LockCoordsOffset>().offsetZ = 0.5f;
        GameObject bgObj = bgInst.gameObject;

        // topic card
        cardInst = Instantiate(cardPrefab, transform.position, Quaternion.identity);
        cardInst.GetComponent<LockCoordsOffset>().anchor = gameObject.transform;
        cardInst.GetComponent<LockCoordsOffset>().offsetX = -6.85f;//0f;
        cardInst.GetComponent<LockCoordsOffset>().offsetY = 5.25f;//12f;
        cardInst.GetComponent<LockCoordsOffset>().offsetZ = 1.5f;
        GameObject cardObj = cardInst.gameObject; 

        // topic text
        topicInst = Instantiate(textPrefab, transform.position, Quaternion.identity);
        topicInst.GetComponent<LockCoords>().anchor = cardInst;
        topicInst.GetComponent<TextMesh>().text = topic;
        GameObject topicObj = topicInst.gameObject; 
    }

    private void OnMouseDown()
    {
        mouseDown = true; 
    }

    private void OnMouseUp()
    {
        mouseDown = false; 
    }

    // Update is called once per frame
    void Update () {
        if (mouseDown)
        {
            // translate raycast y to the correct size value
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);


            //ratio = Mathf.Clamp((hit.point.y - transform.position.y) / scalableAmount, minimumOffset, maximumSize);
            h = Mathf.Clamp(hit.point.y - transform.position.y, minimumOffset, maximumSize);
            ratio = (h - minimumOffset) / (maximumSize - minimumOffset);
        }

        //GetComponent<SpriteRenderer>().size = new Vector2(w, (scalableAmount * ratio) + minimumOffset);
        GetComponent<SpriteRenderer>().size = new Vector2(w, h);

        UtilitiesScript.UpdatePercentageDisplay(percentInst, ratio, transform.position.x, transform.position.y + h + 0.5f);

    }

    private void OnDestroy()
    {
        if (percentInst != null) Destroy(percentInst.gameObject); // this is the correct way - make uniform if you can be bothered. 
        Destroy(bgObj);
        Destroy(cardObj);
        Destroy(topicObj);
    }
}
