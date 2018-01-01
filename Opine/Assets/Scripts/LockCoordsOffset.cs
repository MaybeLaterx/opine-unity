using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCoordsOffset : MonoBehaviour {

    public Transform anchor;

    public float offsetX = Mathf.Infinity, offsetY = Mathf.Infinity, offsetZ = Mathf.Infinity; 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (anchor == null) Destroy(gameObject);
        else
        {
            transform.position = new Vector3(anchor.position.x + (offsetX == Mathf.Infinity ? 0f : offsetX), anchor.position.y + (offsetY == Mathf.Infinity ? 0f : offsetY), anchor.position.z + (offsetZ == Mathf.Infinity ? 0f : offsetZ)); // -0.4f y, + 0.5f z for graph bg 
            transform.rotation = anchor.rotation;
        }

    }
}
