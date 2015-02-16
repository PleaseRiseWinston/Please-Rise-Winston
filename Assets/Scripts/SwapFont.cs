/* 
 * This script is attached to each Line object.
 * The Line object will handle fading and translation of each row of words via this script.
 * This script uses the NoteFocus script from the Canvas parent.
*/

using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using UnityEngine.EventSystems;
using System.Collections;

public class SwapFont : MonoBehaviour
{
    public bool translateable = false;

    public int beforeSize = 28;
    public int afterSize = 20;

    public Color defaultColor;
    public Color highlightColor;
    public Font defaultFont;
    public Font translatedFont;

    private Text text;
    public bool isTranslated = false;
    public bool isVisible = true;

    private GameObject paper;
    private NoteFocus noteFocus;

    void Start()
    {
        text = GetComponent<Text>();
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

        // Each word object gets the paper that it is on
        paper = transform.parent.parent.gameObject;
        noteFocus = paper.GetComponent<NoteFocus>();
    }

    public void OnEnter(BaseEventData e)
    {
        //Debug.Log ("Over");
        if (noteFocus.focused)
        {
            StopAllCoroutines();
            StartCoroutine(translate());
        }
    }

    public void OnExit(BaseEventData e)
    {
        if (noteFocus.focused)
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
            yield return StartCoroutine(HOTween.To(text, 0.2f, "color", Color.clear).WaitForCompletion());
        }

        if (!isTranslated)
        {
            text.font = translatedFont;
            // Add corrections after this.
            text.fontSize = afterSize;
            isTranslated = true;
        }

        if (!isVisible)
        {
            isVisible = true;
            if (translateable)
            {
                yield return StartCoroutine(HOTween.To(text, 0.2f, "color", highlightColor).WaitForCompletion());
            }
            else
            {
                yield return StartCoroutine(HOTween.To(text, 0.2f, "color", defaultColor).WaitForCompletion());
            }
        }
    }

    IEnumerator untranslate()
    {
        if (isVisible)
        {
            isVisible = false;
            yield return StartCoroutine(HOTween.To(text, 0.2f, "color", Color.clear).WaitForCompletion());
        }

        if (isTranslated)
        {
            text.font = defaultFont;
            // Add corrections after this.
            text.fontSize = beforeSize;
            isTranslated = false;
        }

        if (!isVisible)
        {
            isVisible = true;
            yield return StartCoroutine(HOTween.To(text, 0.2f, "color", defaultColor).WaitForCompletion());
        }
    }
}
