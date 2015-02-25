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

public class LineScript : MonoBehaviour
{

    public bool translateable = false;

    public int beforeSize = 28;
    public int afterSize = 20;
    public int childCount;

    public Color defaultColor;
    public Color highlightColor;
    public Font defaultFont;
    public Font translatedFont;

    public bool isTranslated = false;
    public bool isVisible = true;

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
        gameObject.AddComponent<BoxCollider2D>();

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
            Text newWord = Instantiate(word, transform.position + new Vector3(lastWordEnd, 0, 0) + (transform.forward * -0.2f), transform.rotation) as Text;
            //newWord.gameObject.AddComponent<BoxCollider2D>();
            newWord.transform.SetParent(transform);
            newWord.transform.localScale = newWord.transform.localScale * 3;

            // TODO: Set up mesh sizes to wrap to text
            // newWord gets string s as text
            newWord.text = s;
            lastWordEnd += newWord.transform.right.x;
            //Debug.Log(newWord.transform.right.x);
        }
        Transform[] childArray = gameObject.GetComponentsInChildren<Transform>();
        childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).GetComponent<Text>().font = defaultFont;
        }
    }

    void Update()
    {
        if (paperScript.focused)
        {
            gameObject.collider2D.enabled = true;
        }
        else
        {
            gameObject.collider2D.enabled = false;
        }
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
        StopAllCoroutines();
        StartCoroutine(untranslate());
    }


    /***Translate Functions***/

    public IEnumerator translate()
    {
        if (isVisible)
        {
            isVisible = false;
            for (int i = 0; i < childCount; i++)
            {
                if (i < childCount - 1)
                {
                    HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", Color.clear);
                }
                else
                    yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", Color.clear).WaitForCompletion());
            }
        }

        if (!isTranslated)
        {
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).GetComponent<Text>().font = translatedFont;
                // Add corrections after this.
                //transform.GetChild(i).GetComponent<Text>().fontSize = afterSize;
            }
            isTranslated = true;
        }

        if (!isVisible)
        {
            isVisible = true;
            if (translateable)
            {
                for (int i = 0; i < childCount; i++)
                {
                    if (i < childCount - 1)
                    {
                        HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", highlightColor);
                    }
                    else
                        yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", highlightColor).WaitForCompletion());
                }
            }
            else
            {
                for (int i = 0; i < childCount; i++)
                {
                    if (i < childCount - 1)
                    {
                        HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", defaultColor);
                    }
                    else
                        yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", defaultColor).WaitForCompletion());
                }
            }
        }
    }

    public IEnumerator untranslate()
    {
        if (isVisible)
        {
            isVisible = false;
            for (int i = 0; i < childCount; i++)
            {
                if (i < childCount - 1)
                {
                    HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", Color.clear);
                }
                else
                    yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", Color.clear).WaitForCompletion());
            }
        }

        if (isTranslated)
        {
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).GetComponent<Text>().font = defaultFont;
                // Add corrections after this.
                //transform.GetChild(i).GetComponent<Text>().fontSize = beforeSize;
            }
            isTranslated = false;
        }

        if (!isVisible)
        {
            isVisible = true;
            for (int i = 0; i < childCount; i++)
            {
                if (i < childCount - 1)
                {
                    HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", defaultColor);
                }
                else
                    yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", defaultColor).WaitForCompletion());
            }
        }
    }
}

// Get over it, bitches.