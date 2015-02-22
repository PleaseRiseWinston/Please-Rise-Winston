using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/* 
 * This script is attached to each Line object.
 * The Line object will handle fading and translation of each row of words via this script.
 * This script is instanced with the proper number of words stored in a variable.
 * The script then iterates through and instantiates Word objects for each word.
*/

public class LineScript : MonoBehaviour {

    public bool translateable = false;

    public int beforeSize = 28;
    public int afterSize = 20;

    public Color defaultColor;
    public Color highlightColor;
    public Font defaultFont;
    public Font translatedFont;

    public bool isTranslated = false;
    public bool isVisible = true;

    public float wordSpacing = 10;
    public float lastWordEnd;

    private GameObject paper;
    private PaperScript paperScript;

    private Canvas canvas;
    private CanvasScript canvasScript;

    public Text word;
    public string[] words;
    public List<string> wordList = new List<string>();

    void Start()
    {
        defaultColor = Color.black;
        highlightColor = Color.red;

        /*
         * 'paper' references the Paper object found as the parent to Canvas
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

        // Each line object gets the paper that it is on
        paper = transform.parent.parent.gameObject;
        paperScript = paper.GetComponent<PaperScript>();

        // Each line object gets the canvas that it is on
        canvas = transform.parent.GetComponent<Canvas>();
        canvasScript = canvas.GetComponent<CanvasScript>();

        // Iterate through the wordList and instantiate words with space buffers in between
        lastWordEnd = 0;
        foreach (string s in words)
        {
            Text newWord = Instantiate(word, transform.position + new Vector3(lastWordEnd + wordSpacing, 0, 0), transform.rotation) as Text;
            newWord.transform.SetParent(transform);

            // newWord gets string s as text
            newWord.text = s;
            lastWordEnd = newWord.transform.right.x;
        }

        /*
        for (int i = 0; i < wordList.Count; i++){
            if (i == 0)
            {
                lastWordEnd = 0;
            }
            
            Text newWord = Instantiate(word, transform.position + new Vector3(lastWordEnd + wordSpacing, 0, 0), transform.rotation) as Text;
            newWord.transform.SetParent(transform);
            
            // Give each newly intantiated word the text from the wordList;
            newWord.text = wordList[i];

            lastWordEnd = newWord.transform.right.x;
            //print(lastWordEnd);
        }
        */
    }

    public void OnEnter(BaseEventData e)
    {
        //Debug.Log ("Over");
        if (paperScript.focused)
        {
            StopAllCoroutines();
            StartCoroutine(translate());
        }
    }

    public void OnExit(BaseEventData e)
    {
        if (paperScript.focused)
        {
            StopAllCoroutines();
            StartCoroutine(untranslate());
        }
    }

    /***Translate Functions***/

    IEnumerator translate()
    {
        if (isVisible)
        {
            isVisible = false;
            yield return StartCoroutine(HOTween.To(word, 0.2f, "color", Color.clear).WaitForCompletion());
        }

        if (!isTranslated)
        {
            word.font = translatedFont;
            // Add corrections after this.
            word.fontSize = afterSize;
            isTranslated = true;
        }

        if (!isVisible)
        {
            isVisible = true;
            if (translateable)
            {
                yield return StartCoroutine(HOTween.To(word, 0.2f, "color", highlightColor).WaitForCompletion());
            }
            else
            {
                yield return StartCoroutine(HOTween.To(word, 0.2f, "color", defaultColor).WaitForCompletion());
            }
        }
    }

    IEnumerator untranslate()
    {
        if (isVisible)
        {
            isVisible = false;
            yield return StartCoroutine(HOTween.To(word, 0.2f, "color", Color.clear).WaitForCompletion());
        }

        if (isTranslated)
        {
            word.font = defaultFont;
            // Add corrections after this.
            word.fontSize = beforeSize;
            isTranslated = false;
        }

        if (!isVisible)
        {
            isVisible = true;
            yield return StartCoroutine(HOTween.To(word, 0.2f, "color", defaultColor).WaitForCompletion());
        }
    }
}
