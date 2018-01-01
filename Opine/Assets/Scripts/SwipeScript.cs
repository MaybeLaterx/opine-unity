using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeScript : MonoBehaviour {

    bool lockedToMouse = false;
    public float minDragRequirement = 4f;
    public Vector3 pivot; // Where it should try to pull back to when released
    public Vector3 pivotCentre, pivotLeft, pivotRight, pivotBottom; 
    public float pivotOffset = 12f; 
    RaycastHit hit;
    public float lerpRatio = 0.3f;
    public float rotationIntensity = 3f;
    AudioSource[] sounds;
    AudioSource chime;
    AudioSource beep;

    public string topic;
    public bool answerGiven;
    bool draggableScene;

    Transform triggerInst; 

    // Use this for initialization
    void Start () {
        pivotCentre = new Vector3(0, 0, 0);
        pivot = pivotCentre;
        pivotLeft = new Vector3(pivot.x - pivotOffset, pivot.y, pivot.z);
        pivotRight = new Vector3(pivot.x + pivotOffset, pivot.y, pivot.z);
        pivotBottom = new Vector3(pivot.x, -15f, pivot.z); 
        
        sounds = GetComponents<AudioSource>();
        chime = sounds[0];
        beep = sounds[1];

        draggableScene = (SceneManager.GetActiveScene().name == "S_YayOrNay");
        if (draggableScene) triggerInst = GameObject.FindGameObjectWithTag("BottomButton").transform;
    }

    // Click
    private void OnMouseDown()
    {
        if (draggableScene)
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
                //beep.Play();
            }
        }
    }

    public void Yay()
    {
        pivot = pivotLeft;
        print("Yay! (Swiped left)");
        //chime.Play();

        // update button inputs internal cooldown
        ConfirmYayOrNay.transitioning = true;
        //StartCoroutine(ConfirmYayOrNay.WaitTime());

        triggerInst.GetComponent<ConfirmYayOrNay>().Trigger("yay");
    }

    public void Nay()
    {
        pivot = pivotRight;
        print("Nay! (Swiped right)");
        //chime.Play();

        // update button inputs internal cooldown
        ConfirmYayOrNay.transitioning = true;
        //StartCoroutine(ConfirmYayOrNay.WaitTime());

        triggerInst.GetComponent<ConfirmYayOrNay>().Trigger("nay");
    }

    public void Skip()
    {
        pivot = pivotBottom;
        print("Skipping!");
        //chime.Play();

        // update button inputs internal cooldown
        ConfirmYayOrNay.transitioning = true;
        //StartCoroutine(ConfirmYayOrNay.WaitTime());

        triggerInst.GetComponent<ConfirmYayOrNay>().Trigger("skip");
    }

    // Update is called once per frame
    void Update () {
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
