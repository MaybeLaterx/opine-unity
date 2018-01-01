using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ease : MonoBehaviour {

    float lerpRatio = 0.3f;
    public float alignmentX, alignmentY, alignmentZ, scaleX, scaleY, scaleZ; 

	// Use this for initialization
	void Awake () {
        alignmentX = Mathf.Infinity;
        alignmentY = Mathf.Infinity;
        alignmentZ = Mathf.Infinity;

        scaleX = Mathf.Infinity;
        scaleY = Mathf.Infinity;
        scaleZ = Mathf.Infinity;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(
            (alignmentX == Mathf.Infinity) ? transform.position.x : Mathf.Lerp(transform.position.x, alignmentX, lerpRatio), 
            (alignmentY == Mathf.Infinity) ? transform.position.y : Mathf.Lerp(transform.position.y, alignmentY, lerpRatio), 
            (alignmentZ == Mathf.Infinity) ? transform.position.z : Mathf.Lerp(transform.position.z, alignmentZ, lerpRatio));

        transform.localScale = new Vector3(
            (scaleX == Mathf.Infinity) ? transform.localScale.x : Mathf.Lerp(transform.localScale.x, scaleX, lerpRatio),
            (scaleY == Mathf.Infinity) ? transform.localScale.y : Mathf.Lerp(transform.localScale.y, scaleY, lerpRatio),
            (scaleZ == Mathf.Infinity) ? transform.localScale.z : Mathf.Lerp(transform.localScale.z, scaleZ, lerpRatio));
    }
}
