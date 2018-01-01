using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkScript : MonoBehaviour {

    public float mSpeed, rSpeed;
    public float boxHeight;
    public float boxWidth;
    public Vector3 dir;

    float buffer = 0.2f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        boxHeight += buffer;
        boxWidth += buffer;  
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(dir * Time.deltaTime * mSpeed, Space.World);
        transform.Rotate(0f, 0f, rSpeed);
        //transform.Translate(Vector3.right * Time.deltaTime * hSpeed, Space.World);

        if (Mathf.Abs(transform.position.x) > boxWidth/2f || Mathf.Abs(transform.position.y) > boxHeight/2f)
        {
            //print("X: " + transform.position.x + ", w: " + boxWidth / 2f);
            //print("Question mark too far away, deleted!");
            Destroy(gameObject);
        }
    }
}
