using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BackgroundColourScript : MonoBehaviour {

    Camera mainCamera = null;
    Color newCol = Color.white;
    Color oldCol = Color.black;
    Color lerpedCol = Color.white;

    Color votingColour, loginColour, resultsColour, playingColour; 

    float lerpVal;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    Color Color255(float r, float g, float b, float a)
    {
        Color col = new Color(r / 255f, g / 255f, b / 255f, a);
        return col; 
    }

    // Use this for initialization
    void Start () {
        lerpVal = 0;

        resultsColour = Color255(174, 79, 147, 1);
        loginColour = Color255(117, 70, 114, 1);
        votingColour = Color255(0, 174, 172, 1);
        playingColour = Color255(0, 130, 180, 1);
	}
	
	// Update is called once per frame
	void Update () {
        //print("Still here!");

        if (mainCamera == null) mainCamera = Camera.main;

        string levelName = SceneManager.GetActiveScene().name;
        if (levelName == "S_VotingTime" || levelName == "S_Menu") newCol = votingColour;
        else if (levelName == "S_Login") newCol = loginColour;
        else if (levelName.Contains("Results")) newCol = resultsColour;
        else newCol = playingColour;

        
        if (lerpedCol == newCol)
        {
            oldCol = newCol;  // adopt new colour when transition is complete
            lerpVal = 0; 
        } else
        {
        
            lerpedCol = Color.Lerp(oldCol, newCol, lerpVal);
            lerpVal += 0.02f; 
            mainCamera.backgroundColor = lerpedCol; 
        }

        

	}
}
