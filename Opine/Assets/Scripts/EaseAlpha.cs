using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaseAlpha : MonoBehaviour {

    public float alpha;
    float lerpRatio = 0.2f;

	// Use this for initialization
	void Awake () {
        alpha = Mathf.Infinity;
	}
	
	// Update is called once per frame
	void Update () {
        if (alpha != Mathf.Infinity) {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Lerp(color.a, alpha, lerpRatio);
            GetComponent<SpriteRenderer>().color = color; 
        }
	}
}
