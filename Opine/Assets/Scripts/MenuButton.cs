using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    [SerializeField] private string nextLevel;
    Ease scr;

    // Use this for initialization
    void Start () {
        scr = GetComponent<Ease>();
        scr.alignmentX = 0f;

	}

    private void OnMouseUp()
    {
        scr.alignmentX = 12f;
        StartCoroutine(LevelLoad(0.5f));
    }

    IEnumerator LevelLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(nextLevel);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
