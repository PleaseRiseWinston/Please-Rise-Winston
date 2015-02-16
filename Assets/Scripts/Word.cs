/* 
 * This script is attached to each Word object.
 * Each Word object is instantiated by its Line parent.
 * Should the word be changeable, it will be highlighted with a pulsing glow, and detect clicks.
 * Post-click, the screen will dim and all change options for the clicked word will appear as children of the clicked word.
*/

using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;

public class Word : MonoBehaviour {

    public Color defaultColor;
    public Color highlightColor;

    public GameObject wordPrefab;

    private string[] wordOptions;
    private string curText;
    private bool changeable;

    private GameObject paper;
    private NoteFocus noteFocus;

    private GameObject line;
    private SwapFont swapFont;

    void Start()
    {
        // TODO: Read text into textOptions array here

        curText = GetComponent<Text>().text;
        defaultColor = Color.black;
        highlightColor = Color.red;

        /*
         * 'paper' references the Paper object found as the parent to Canvas
         * 'line' references the Line object found as the parent to Word
         * 
         * -Paper
         *   - Canvas
         *     - Line
         *       - Word
         *       - Word
         *       - Word
         *     - Line
         *       - etc...
        */

        // Each word object gets the paper that it is on
        paper = transform.parent.parent.parent.gameObject;
        noteFocus = paper.GetComponent<NoteFocus>();

        // Each word object gets the line that it is on
        line = transform.parent.gameObject;
        swapFont = line.GetComponent<SwapFont>();

        wordOptions[0] = "rawr";
    }

    public void OnDown(BaseEventData e)
    {
        if (changeable && noteFocus.focused && swapFont.isTranslated)
        {
            StopAllCoroutines();
            StartCoroutine(overlay());
        }
    }

    IEnumerator overlay()
    {
        // TODO: Make screen darken and have textOptions[] entries show up
        foreach (string word in wordOptions)
        {
            // TODO: Instantiate word on screen as clickable entity
            Vector3 wordLocation = new Vector3 ((Screen.width/2), (Screen.height/2), 0);
            Instantiate(wordPrefab, wordLocation, Quaternion.identity);
            Debug.Log(wordPrefab);
        }
        yield return StartCoroutine(HOTween.To(wordPrefab, 0.1f, "color", Color.clear).WaitForCompletion());
    }

    void Update()
    {
        // While a word is changeable and the current line is translated, highlight it
        if (changeable && swapFont.isTranslated)
        {
            // TODO: Add animations to highlight this changeable word; probably going to have a pulsing glow
        }
    }
}
