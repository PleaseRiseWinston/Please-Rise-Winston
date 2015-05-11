using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Tray : MonoBehaviour
{
    public GameObject gameController;
    public GameController gameControllerScript;

    public float trayCooldown;

    public Color defaultColor;
    public Color transparent;

	void Start ()
	{
	    gameController = GameObject.FindGameObjectWithTag("GameController");
	    gameControllerScript = gameController.GetComponent<GameController>();

        trayCooldown = Time.time + 3f;

        // Records/declares colors, then sets glow to transparent
        defaultColor = transform.GetChild(0).transform.GetComponent<SpriteRenderer>().color;
        transparent = new Color(1, 1, 1, 0);
        transform.GetChild(0).transform.GetComponent<SpriteRenderer>().color = transparent;
	}

    public void OnMouseEnter()
    {
        StopAllCoroutines();
        StartCoroutine(Glow());
    }

    public void OnMouseExit()
    {
        StopAllCoroutines();
        StartCoroutine(Unglow());
    }

    IEnumerator Glow()
    {
        yield return StartCoroutine(HOTween.To(transform.GetChild(0).transform.GetComponent<SpriteRenderer>(), 0.8f, "color", defaultColor).WaitForCompletion());
    }

    IEnumerator Unglow()
    {
        yield return StartCoroutine(HOTween.To(transform.GetChild(0).transform.GetComponent<SpriteRenderer>(), 0.8f, "color", transparent).WaitForCompletion());
    }

    public void OnMouseDown()
    {
        
        // Check all lines in curnote are translated
        bool allTranslated = true;
        for (int i = 0; i < gameControllerScript.curNote.transform.GetChild(0).transform.childCount; i++)
        {
            if (!gameControllerScript.curNote.transform.GetChild(0).transform.GetChild(i).GetComponent<LineScript>().isTranslated)
            {
                allTranslated = false;
                Debug.Log("Line " + i + "not translated");
            }
        }
        

        // Only trays/moves curNote iff paper is unfocused and the last paper has arrived at its destination
        if (!gameControllerScript.curNote.GetComponent<PaperScript>().focused && !gameControllerScript.curNoteInMotion && trayCooldown <= Time.time)
        {
            //print("curNote name: " + gameControllerScript.curNote.name);
            gameControllerScript.curNote.GetComponent<PaperScript>().inTray = true;
            gameControllerScript.ToTray();
        }

        // TODO: Add Weights
    }
}
