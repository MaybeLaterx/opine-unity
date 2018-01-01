using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmYayOrNay : MonoBehaviour
{

    static public Transform controller;
    static public float secsToWait;
    static public bool transitioning;
    public string boxType;

    // Use this for initialization
    void Start()
    {
        transitioning = true;
        StartCoroutine(WaitTime());
        controller = GameObject.FindGameObjectWithTag("GameController").transform;
        secsToWait = 0.75f; 
    }

    static public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(secsToWait);
        transitioning = false;
    }

    void OnMouseUp() // on release
    {
        if (!transitioning) Trigger(boxType); 
    }

    public void Trigger(string response)
    {
        // Update lights 
        foreach (GameObject indicator in GameObject.FindGameObjectsWithTag("Indicator"))
        {
            indicator.GetComponent<IndicatorColour>().shouldUpdate = true;
        }

        // tell controller to update lights and get next
        bool isGameOver = controller.GetComponent<GameHandlerYayOrNay>().UpdateIndicators(response); //"yay", "nay" or "skip"
        if (!isGameOver) controller.GetComponent<GameHandlerYayOrNay>().MakeNextSwiper();


        // tell swiper to disappear in appropriate direction
        GameObject[] swipers = GameObject.FindGameObjectsWithTag("YayOrNayCard"); // MUST PASS REFERENCE INSTEAD
        GameObject swiper = swipers[swipers.Length - 1];
        print("Swipers: " + swipers.Length);
        switch (response)
        {
            case "yay": swiper.GetComponent<SwipeScript>().pivot = swiper.GetComponent<SwipeScript>().pivotLeft; break;
            case "nay": swiper.GetComponent<SwipeScript>().pivot = swiper.GetComponent<SwipeScript>().pivotRight; break;
            case "skip": swiper.GetComponent<SwipeScript>().pivot = swiper.GetComponent<SwipeScript>().pivotBottom; break;
            default: print("Bad response: " + response); break;
        }
                

        // update self (internal cooldown)
        transitioning = true;
        StartCoroutine(WaitTime());
        
    }
}
