using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ease : MonoBehaviour {

    float lerpRatio = 0.15f;
    public float alignmentX, alignmentY, alignmentZ, scaleX, scaleY, scaleZ;
    public bool isUI = false;
    RectTransform myTransform; 

    // Use this for initialization
    void Awake () {


        alignmentX = Mathf.Infinity;
        alignmentY = Mathf.Infinity;
        alignmentZ = Mathf.Infinity;

        scaleX = Mathf.Infinity;
        scaleY = Mathf.Infinity;
        scaleZ = Mathf.Infinity;

        if (isUI)  myTransform = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isUI)
        {
            myTransform.anchoredPosition = new Vector3(
                (alignmentX == Mathf.Infinity) ? myTransform.anchoredPosition.x : Mathf.Lerp(myTransform.anchoredPosition.x, alignmentX, lerpRatio),
                (alignmentY == Mathf.Infinity) ? myTransform.anchoredPosition.y : Mathf.Lerp(myTransform.anchoredPosition.y, alignmentY, lerpRatio)/*,
                (alignmentZ == Mathf.Infinity) ? myTransform.anchoredPosition.z : Mathf.Lerp(myTransform.anchoredPosition.z, alignmentZ, lerpRatio)*/);


        } else
        {
            transform.position = new Vector3(
               (alignmentX == Mathf.Infinity) ? transform.position.x : Mathf.Lerp(transform.position.x, alignmentX, lerpRatio),
               (alignmentY == Mathf.Infinity) ? transform.position.y : Mathf.Lerp(transform.position.y, alignmentY, lerpRatio),
               (alignmentZ == Mathf.Infinity) ? transform.position.z : Mathf.Lerp(transform.position.z, alignmentZ, lerpRatio));
        }

        transform.localScale = new Vector3(
            (scaleX == Mathf.Infinity) ? transform.localScale.x : Mathf.Lerp(transform.localScale.x, scaleX, lerpRatio),
            (scaleY == Mathf.Infinity) ? transform.localScale.y : Mathf.Lerp(transform.localScale.y, scaleY, lerpRatio),
            (scaleZ == Mathf.Infinity) ? transform.localScale.z : Mathf.Lerp(transform.localScale.z, scaleZ, lerpRatio));

    }
}
