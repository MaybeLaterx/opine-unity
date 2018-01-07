using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{

    [SerializeField] private string nextLevel;
    Ease scr;
    GameObject myScore, opponentScore, outcomeText;

    // Use this for initialization
    void Start()
    {
        scr = GetComponent<Ease>();
        scr.alignmentY = -6.5f;

        myScore = GameObject.FindGameObjectWithTag("MyScore");
        myScore.GetComponent<Ease>().alignmentY = 0f;

        opponentScore = GameObject.FindGameObjectWithTag("OpponentScore");
        opponentScore.GetComponent<Ease>().alignmentY = 0f;

        outcomeText = GameObject.FindGameObjectWithTag("Title");
        string newText;
        if (FetchFullGameTopics.myScore > FetchFullGameTopics.opponentScore) newText = "YOU WIN!";
        else if (FetchFullGameTopics.myScore < FetchFullGameTopics.opponentScore) newText = "YOU LOSE!";
        else newText = "DRAW!";

        outcomeText.GetComponent<TextMesh>().text = newText;
        outcomeText.GetComponent<Ease>().alignmentY = 6f;
    }

    private void OnMouseUp()
    {
        scr.alignmentY = -12.5f;
        myScore.GetComponent<Ease>().alignmentX = -12f;
        opponentScore.GetComponent<Ease>().alignmentX = 12f;
        outcomeText.GetComponent<Ease>().alignmentY = 16f;
        StartCoroutine(LevelLoad(0.5f));


        FetchFullGameTopics.myScore = 0;
        FetchFullGameTopics.opponentScore = 0;

        GameObject jsonObject = GameObject.FindGameObjectWithTag("JSON");
        Destroy(jsonObject);

    }

    IEnumerator LevelLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(nextLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
