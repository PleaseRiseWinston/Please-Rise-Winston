using UnityEngine;using UnityEngine.UI;
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
    public float oldPosition;

    private GameObject paper;
    private PaperScript paperScript;

    private Canvas canvas;
    private CanvasScript canvasScript;

    public Text word;
    public string[] words;
    public List<string> wordList = new List<string>();
	public int quickFixNum = 0;

    /*** 3DText Attempt ***/

    public GameObject textPrefab;

    void Start()
    {
        //gameObject.AddComponent<BoxCollider2D>();
        gameObject.AddComponent<ContentSizeFitter>();
        gameObject.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        gameObject.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

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
        oldPosition = 0;
        foreach (string s in words)
        {
            Text newWord;
            //Debug.Log(paperScript.start);
            if (paperScript.start || paperScript.exit)
            {
				quickFixNum = PaperScript.wordIDNum;
                newWord = Instantiate(word, paper.transform.position + (paper.transform.forward * -0.5f), transform.rotation) as Text;
				newWord.name = "wordID" + quickFixNum;
				PaperScript.wordIDNum++;
            }
            else
            {
				quickFixNum = PaperScript.wordIDNum;
                newWord = Instantiate(word, transform.position + new Vector3(lastWordEnd, 0, 0) + (transform.forward * -0.2f), transform.rotation) as Text;
				newWord.name = "wordID" + quickFixNum;
				PaperScript.wordIDNum++;
            }
            newWord.transform.SetParent(transform);
            newWord.transform.localScale = newWord.transform.localScale * 3;

            //newWord.gameObject.AddComponent<ContentSizeFitter>();
            //newWord.gameObject.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            //newWord.gameObject.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // TODO: Set up mesh sizes to wrap to text
            // newWord gets string s as text
            newWord.text = s;
            
            lastWordEnd += newWord.GetComponent<Transform>().right.x;
            //Debug.Log(newWord.GetComponent<Transform>().right.x);
        }
        Transform[] childArray = gameObject.GetComponentsInChildren<Transform>();
        childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform childText = transform.GetChild(i);
            childText.GetComponent<Text>().font = defaultFont;
            Vector3[] corners = new Vector3[4];
            childText.GetComponent<RectTransform>().GetWorldCorners(corners);
            //Debug.Log(corners[0], childText);
            float width = Mathf.Abs(corners[0].x - corners[3].x);
        }

        /*
        foreach (string s in words)
        {
            GameObject newWord = Instantiate(textPrefab, transform.position + new Vector3(lastWordEnd, 0, 0) + (transform.forward * -0.2f), transform.rotation) as GameObject;
            TextMesh textMesh = newWord.GetComponent<TextMesh>();
            textMesh.text = s;
            textMesh.font = defaultFont;
            lastWordEnd += newWord.gameObject.GetComponent<BoxCollider2D>().bounds.max.x;
            Debug.Log(newWord.collider2D.bounds.size.x);
        }*/
    }

    public void OnEnter(BaseEventData e)
    {
        Debug.Log ("Over");
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
        if (isVisible && paperScript.focused)
        {
            isVisible = false;
            for (int i = 0; i < childCount; i++)
            {
                if (i < childCount - 1)
                {
                    HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", Color.clear);
                }
                else
                {
                    yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", Color.clear).WaitForCompletion());
                }
            }
        }

        if (!isTranslated && paperScript.focused)
        {
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).GetComponent<Text>().font = translatedFont;
                // Add corrections after this.
                //transform.GetChild(i).GetComponent<Text>().fontSize = afterSize;
            }
            isTranslated = true;
        }

        if (!isVisible && paperScript.focused)
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
                    {
                        yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", highlightColor).WaitForCompletion());
                    }
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
                    {
                        yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", defaultColor).WaitForCompletion());
                    }
                }
            }
        }
    }

    public IEnumerator untranslate()
    {
        if (isVisible && paperScript.focused)
        {
            isVisible = false;
            for (int i = 0; i < childCount; i++)
            {
                if (i < childCount - 1)
                {
                    HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", Color.clear);
                }
                else
                {
                    yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", Color.clear).WaitForCompletion());
                }
            }
        }

        if (isTranslated && paperScript.focused)
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
                {
                    yield return StartCoroutine(HOTween.To(transform.GetChild(i).GetComponent<Text>(), 0.2f, "color", defaultColor).WaitForCompletion());
                }
            }
        }
    }
}

// Get over it, bitches.