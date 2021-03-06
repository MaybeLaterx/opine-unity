﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkHandler : MonoBehaviour {

    public float boxHeight;
    public float boxWidth;

    public float minSpeed, maxSpeed, waitTime;

    public int startingQMarks; 

    public Transform questionMarkPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        CreateInitialQuestionMarks(startingQMarks); 
        CreateQuestionMark();
    }

    private Transform[] CreateInitialQuestionMarks(int quantity)
    {
        Transform[] insts = new Transform[quantity];
        for (int i = 0; i < quantity; i++)
        {
            float iX = Random.Range(-boxWidth / 2, boxWidth / 2);
            float iY = Random.Range(-boxHeight / 2, boxHeight / 2);
            bool isXLocked = (Random.Range(0, 2) == 0);
            int negation = (Random.Range(0, 2) == 0 ? -1 : 1);
            Vector3 dir;
            if (isXLocked)
            {
                dir = new Vector3(1f * negation, Random.Range(-1f, 1f), 0f);
            }
            else
            {
                dir = new Vector3(Random.Range(-1f, 1f), 1f * negation, 0f);
            }

            Vector3 loc = new Vector3(iX, iY, transform.position.z);

            float rSpeed = Random.Range(-1f, 1f);
            float buffer = 0.2f;
            //Transform inst = Instantiate(questionMarkPrefab, loc, Quaternion.Euler(0f, 0f, RandomAngle()));
            Transform inst = Instantiate(questionMarkPrefab, loc, Quaternion.identity);

            inst.GetComponent<QuestionMarkScript>().mSpeed = Random.Range(minSpeed, maxSpeed);
            inst.GetComponent<QuestionMarkScript>().boxHeight = boxHeight + buffer;
            inst.GetComponent<QuestionMarkScript>().boxWidth = boxWidth + buffer;
            inst.GetComponent<QuestionMarkScript>().dir = dir;
            inst.GetComponent<QuestionMarkScript>().rSpeed = rSpeed;
            inst.GetComponent<RectTransform>().transform.position = loc;

            insts[i] = inst;

        }
        return insts; 

    }

    private Transform CreateQuestionMark()
    {
        int ran = Random.Range(0, 4); // does not include 4 
        //print("Ran: " + ran);
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
        float buffer = 0.2f;
        //Transform inst = Instantiate(questionMarkPrefab, loc, Quaternion.Euler(0f, 0f, RandomAngle()));
        Transform inst = Instantiate(questionMarkPrefab, loc, Quaternion.identity); 

        inst.GetComponent<QuestionMarkScript>().mSpeed = Random.Range(minSpeed, maxSpeed);
        inst.GetComponent<QuestionMarkScript>().boxHeight = boxHeight + buffer;
        inst.GetComponent<QuestionMarkScript>().boxWidth = boxWidth + buffer;
        inst.GetComponent<QuestionMarkScript>().dir = dir;
        inst.GetComponent<QuestionMarkScript>().rSpeed = rSpeed;
        inst.GetComponent<RectTransform>().transform.position = loc; 

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
