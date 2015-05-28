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
    public float oldPosition;

    private GameObject paper;
    private PaperScript paperScript;

    private Canvas canvas;
    private CanvasScript canvasScript;

    public Text word;
    public string[] words;
    public List<string> wordList = new List<string>();
	
	public GameObject textBox;
	public TextBox textBoxScript;
	
	public GameObject gameController;
	public GameController gameControllerScript;

    /*** 3DText Attempt ***/

    public GameObject textPrefab;

    void Start()
    {
		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
		
        defaultColor = Color.black;
        highlightColor = Color.red;
		
		gameController = GameObject.FindGameObjectWithTag("GameController").gameObject;
		gameControllerScript = gameController.GetComponent<GameController>();

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

        gameObject.AddComponent<HorizontalLayoutGroup>();
        HorizontalLayoutGroup layoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.spacing = 3;
        layoutGroup.childForceExpandWidth = false;

        transform.localScale = new Vector3(0.05f, 0.05f, 1f);
		
        foreach (string s in words)
        {
            Text newWord;
            //Debug.Log(paperScript.start);
            if (paperScript.start || paperScript.exit)
            {
                newWord = Instantiate(word, paper.transform.position + (paper.transform.forward * -0.1f), transform.rotation) as Text;
                newWord.transform.SetParent(transform);
                newWord.transform.localScale = newWord.transform.localScale * 1;
            }
            else
            {
                newWord = Instantiate(word, paper.transform.position + (paper.transform.forward * -1.3f), transform.rotation) as Text;
                newWord.transform.SetParent(transform);
                newWord.transform.localScale = newWord.transform.localScale * 1;
				newWord.tag = "Word";
				newWord.name = "wordID" + textBoxScript.structList[textBoxScript.quickFixNum].wordID;
				textBoxScript.quickFixNum++;

                if (newWord.GetComponent<WordScript>().changeable == true)
                {
                    newWord.GetComponent<Text>().color = Color.red;
                }
            }

            // TODO: Set up mesh sizes to wrap to text
            // newWord gets string s as text
            newWord.text = s;
			
			if(newWord.text == ""){
				isTranslated = true;
			}
            
			/* foreach(WordStructure wordStruct in textBoxScript.swapWordList){
				if(newWord.text == wordStruct.current && wordStruct.lineID == "N/A" && newWord.name == "wordID" + wordStruct.wordID){
					wordStruct.lineID = gameObject.name;
				}
			} */
			
            lastWordEnd += newWord.GetComponent<Transform>().right.x;
            //Debug.Log(newWord.GetComponent<Transform>().right.x);
        }
        //Transform[] childArray = gameObject.GetComponentsInChildren<Transform>();
        childCount = transform.childCount;
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