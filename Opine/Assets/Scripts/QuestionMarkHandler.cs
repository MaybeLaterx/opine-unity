using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkHandler : MonoBehaviour {

    public float boxHeight;
    public float boxWidth;

    public float minSpeed, maxSpeed, waitTime; 

    public Transform questionMarkPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        CreateQuestionMark();
    }

    private Transform CreateQuestionMark()
    {
        int ran = Random.Range(0, 4); // does not include 4 
        float iX = 0f;
        float iY = 0f;
        Vector3 dir = Vector3.zero;

        switch (ran)
        {
            case 0:
                iX = boxWidth / 2f;
                iY = Random.Range(0f, boxHeight) - (boxHeight / 2f);
                dir = new Vector3(-1f, Random.Range(-1f, 1f), 0f);
                break;
            case 1:
                iY = -(boxHeight / 2f);
                iX = Random.Range(0f, boxWidth) - (boxWidth / 2f);
                dir = new Vector3(Random.Range(-1f, 1f), 1f, 0f);
                break;
            case 2:
                iX = -(boxWidth / 2f);
                iY = Random.Range(0f, boxHeight) - (boxHeight / 2f);
                dir = new Vector3(1f, Random.Range(-1f, 1f), 0f);
                break;
            case 3:
                iY = boxHeight / 2f;
                iX = Random.Range(0f, boxWidth) - (boxWidth / 2f);
                dir = new Vector3(Random.Range(-1f, 1f), -1f, 0f);
                break;
            default:
                print("Random input error");
                break;
        }

        //print("Moving to " + iX + " " + iY);
        Vector3 loc = new Vector3(iX, iY, transform.position.z);

        float newZ = Random.Range(0f, 360f);
        float rSpeed = Random.Range(-1f, 1f);
        Transform inst = Instantiate(questionMarkPrefab, loc, Quaternion.Euler(0f, 0f, RandomAngle()));

        inst.GetComponent<QuestionMarkScript>().mSpeed = Random.Range(minSpeed, maxSpeed);
        inst.GetComponent<QuestionMarkScript>().boxHeight = boxHeight;
        inst.GetComponent<QuestionMarkScript>().boxWidth = boxWidth;
        inst.GetComponent<QuestionMarkScript>().dir = dir;
        inst.GetComponent<QuestionMarkScript>().rSpeed = rSpeed;

        StartCoroutine(WaitTime(waitTime));

        return inst; 
    }

    private float RandomAngle()
    {
        return Random.Range(0f, 360f); 
    }

    IEnumerator WaitTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateQuestionMark(); 
    }
}
