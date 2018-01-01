using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderBoxHeight : MonoBehaviour {

    GameObject[] boxes;
    float y;
    int numAbove;
    float[] ys;
    Vector3 target;

    [SerializeField] public float y1, y2, y3, y4 = 0;
    public float movespeed;
    public float lerpRatio;
    bool locked = false;
    public bool answerGiven;
    Vector3 endPos;
    public float endX; 
    public string topic;
    float initialZ;
    float forwardZ;
    public float alignmentX;
    public float alignmentY = Mathf.Infinity; 

    RaycastHit hit;

    // Use this for initialization
    void Start () {
        ys = new float[] {y1, y2, y3, y4};
        answerGiven = false;
        endPos = new Vector3(0,0,0);
        initialZ = transform.position.z;
        forwardZ = -6f;
        alignmentX = transform.position.x; 
	}

    private void OnMouseDown()
    {
        if (SceneManager.GetActiveScene().name == "S_PeckingOrder")
        {
            print("Menu option locked!");
            locked = true;
        }
        else print("Can't click boxes during presentation!"); 
    }

    // Update is called once per frame
    void Update() {
        if (answerGiven) // Remove box from game
        {
            if (endPos == new Vector3(0, 0, 0))
            {
                endPos = new Vector3(endX, transform.position.y, transform.position.z);
            }
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, endX, lerpRatio), transform.position.y, transform.position.z);
            float diff = Mathf.Abs(transform.position.x - endX);
            if (diff < 0.1f)
            {
                print("Box destroyed");
                Destroy(gameObject);
            }
        } else
        {
            boxes = GameObject.FindGameObjectsWithTag(tag);
            y = transform.position.y;

            numAbove = 0;
            foreach (GameObject box in boxes)
            {
                float by = box.transform.position.y;
                //print("Pos Y: " + by.ToString() + " , My Y: " + y.ToString());
                if (by < y)
                {
                    numAbove++;
                }
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            /*
            if (Physics.Raycast(ray, out hit))
            {
                //if (GameObject.ReferenceEquals(hit.collider, gameObject))
                print("Hit!");
                print(hit.collider.GetInstanceID().ToString() + " = " + this.GetInstanceID().ToString());
                if (hit.collider.GetInstanceID() == GetInstanceID()) // Must use -4 when referring to self?? ###
                {
                    print("Same ID!");
                    if (Input.GetButtonDown("Fire1"))
                    {
                        print("Match!");
                        locked = true;
                    }
                }
            }
            */

            if (locked)
            {

                transform.position = new Vector3(transform.position.x, hit.point.y, Mathf.Lerp(transform.position.z, forwardZ, lerpRatio));
                if (Input.GetButtonUp("Fire1")) {
                    locked = false;
                    print("Menu option unlocked!");
                }
            } else transform.position = new Vector3(Mathf.Lerp(transform.position.x, alignmentX, lerpRatio), Mathf.Lerp(transform.position.y, (alignmentY == Mathf.Infinity ? ys[numAbove] : alignmentY), lerpRatio), Mathf.Lerp(transform.position.z, initialZ, lerpRatio));

        }

        /*
        print("numAbove: " + numAbove.ToString());
        // Move towards appropriate y value
        target = new Vector3(transform.position.x, ys[numAbove], transform.position.z);
        //target = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
        print("LOL");
        //transform.position = Vector3.MoveTowards(transform.position, target, movespeed * Time.deltaTime); 
        float targetY = ys[numAbove];
        float moveDist = Mathf.Min(movespeed * Time.deltaTime, Mathf.Abs(y - targetY))  * Mathf.Sign(targetY - y);
        //float maxDist = Mathf.Abs(y - targetY); 
        //float newY = transform.position.y + moveDist;  // minus?????###
        transform.position = new Vector3(transform.position.x, y - moveDist, transform.position.z);
        */
	}
}
