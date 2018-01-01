using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeVote : MonoBehaviour {

    public Vector3 pivot, pivotCentre, pivotBottom, pivotRight, pivotLeft;
    float pivotOffset = 14f, lerpRatio = 0.3f, rotationIntensity = 2.5f, minDragRequirement = 4f; 

    Transform controller;
    RaycastHit hit;
    public string topic; 

    bool lockedToMouse; 

	// Use this for initialization
	void Start () {
        pivotCentre = new Vector3(0, 0, 0);
        pivot = pivotCentre;
        pivotLeft = new Vector3(pivot.x - pivotOffset, pivot.y, pivot.z);
        pivotRight = new Vector3(pivot.x + pivotOffset, pivot.y, pivot.z);
        pivotBottom = new Vector3(pivot.x, -15f, pivot.z);

        controller = GameObject.FindGameObjectWithTag("GameController").transform;
        lockedToMouse = false; 
    }

    public void Yay()
    {
        pivot = pivotLeft;
        print("Yay! (Swiped left)");

        controller.GetComponent<GameHandlerVoting>().Trigger("yay");
    }

    public void Nay()
    {
        pivot = pivotRight;

        controller.GetComponent<GameHandlerVoting>().Trigger("nay");
    }

    public void Skip()
    {
        pivot = pivotBottom;
        print("Skipping!");

        controller.GetComponent<GameHandlerVoting>().Trigger("skip");
    }

    // Click
    private void OnMouseDown()
    {
        if (pivot == pivotCentre)
        {
            print("Topic card locked to mouse!");
            lockedToMouse = true;
        }
        else
        {
            print("Too late! Can't change mind!");
        }

    }

    // Release
    private void OnMouseUp()
    {
        if (lockedToMouse)
        {
            lockedToMouse = false;
            print("Topic card unlocked from mouse!");

            // send left, right, or to centre
            print("X: " + transform.position.x);
            print("Min X: " + (pivot.x + minDragRequirement));
            if (transform.position.x > pivot.x + minDragRequirement) Nay();
            else if (transform.position.x < pivot.x - minDragRequirement) Yay();
            else
            {
                print("Returning to central position.");
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Angle 
        //transform.RotateAround(anchor, transform.position, 1 * Time.deltaTime);
        //float xRatio = 
        //zRotation = Mathf.Lerp(-45f, 45f, xRatio);
        transform.eulerAngles = new Vector3(0, 0, transform.position.x * -1f * rotationIntensity);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (lockedToMouse)
        {
            transform.position = new Vector3(hit.point.x, Mathf.Lerp(transform.position.y, pivot.y, lerpRatio), transform.position.z);
        }
        else transform.position = new Vector3(Mathf.Lerp(transform.position.x, pivot.x, lerpRatio), Mathf.Lerp(transform.position.y, pivot.y, lerpRatio), transform.position.z);

        float pivotDist = Vector3.Distance(pivot, transform.position);
        if (pivot != pivotCentre && pivotDist < 1f)
        {
            Destroy(this);
            print("Card " + topic + " destroyed");
        }
    }
}
