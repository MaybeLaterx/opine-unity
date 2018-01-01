using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmTopDog : MonoBehaviour
{

    static public Transform controller;
    static public float secsToWait;
    static public bool transitioning;
    public string boxType;

    static public Transform[] indicators, cards; // received from GameHandler

    static float delay = 0.75f;

    // Use this for initialization
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").transform;
        transitioning = true;
        StartCoroutine(TransitionTimer());
    }

    static public IEnumerator TransitionTimer()
    {
        yield return new WaitForSeconds(delay);
        transitioning = false;
    }

    private void OnMouseUp()
    {
        if (!transitioning) controller.GetComponent<GameHandlerTopDog>().InputAnswer(boxType);
    }

    /* MOVED TO HANDLER
    public void Trigger(string response)
    {
        // Update lights 
        foreach (Transform indicator in indicators)
        {
            indicator.GetComponent<IndicatorColour>().shouldUpdate = true;
        }

        // tell controller to update lights and get next
        bool isGameOver = controller.GetComponent<GameHandlerTopDog>().UpdateIndicators(response); //"1", "2" or "skip"
        if (!isGameOver) controller.GetComponent<GameHandlerTopDog>().MakeNextSwiper();

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
        StartCoroutine(TransitionTimer());

    }
    */
}