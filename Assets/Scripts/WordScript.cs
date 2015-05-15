using System.Collections;
using System.Reflection.Emit;
using Holoville.HOTween;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* 
 * This script is attached to each Word object.
 * Each Word object is instantiated by its Line parent.
 * Should the word be changeable, it will be highlighted with a pulsing glow, and detect clicks.
 * Post-click, the screen will dim and all change options for the clicked word will appear as children of the clicked word.
*/

public class WordScript : MonoBehaviour {
    public Color defaultColor;
    public Color highlightColor;

    //private string[] wordOptions;
    public string curText;
    public bool changeable;

    private GameObject gameController;
    private GameController gameControllerScript;

    private GameObject paper;
    private PaperScript paperScript;

    private GameObject line;
    private LineScript lineScript;

    private GameObject cameraController;
    private PlayCutscene playCutscene;

	//public bool wordOptionsUp = false;
	public string[] wordOptions;
	public GameObject textPrefab;
	
	public GameObject textBox;
	public TextBox textBoxScript;
	
	//Instantiated word prefabs use Camera.main
	// NOT ARBITRARY POINTS
	public float currX;
	public float currY;
	public float currZ;

    public AudioClip cutsceneMusic;

    public GameObject glowSystem;

    private Vector3 defaultPos;

    private Material fontMaterial;
    private float glowStrength;
    [SerializeField] private Material _glowMaterial;

    private bool _glowing;
    public bool Glowing
    {
        get { return _glowing; }
        set
        {
            _glowing = value;
            if (_glowing)
            {
                // make font glow
                HOTween.To(this, 0.3f, new TweenParms().Prop("glowStrength", 1.0f).OnUpdate(OnTweenUpdate));
            }
            else
            {
                HOTween.To(this, 0.3f, new TweenParms().Prop("glowStrength", 0.0f).OnUpdate(OnTweenUpdate));
            }
        }
    }

    private void OnTweenUpdate()
    {
        fontMaterial.SetFloat("_GlowStrength", glowStrength);
    }

    void Start()
    {
        fontMaterial = new Material(_glowMaterial);
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.cyan);
        texture.Apply();
        fontMaterial.SetTexture("_GlowTex", texture);
        gameObject.GetComponent<Text>().material = fontMaterial;
        Glowing = true;

		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
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

        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameControllerScript = gameController.GetComponent<GameController>();

        // Each word object gets the paper that it is on
        paper = transform.parent.parent.parent.gameObject;
        paperScript = paper.GetComponent<PaperScript>();

        // Each word object gets the line that it is on
        line = transform.parent.gameObject;
        lineScript = line.GetComponent<LineScript>();

        cameraController = GameObject.FindGameObjectWithTag("CameraController");
        playCutscene = cameraController.GetComponent<PlayCutscene>();

        defaultPos = transform.position;
        curText = GetComponent<Text>().text;
        defaultColor = Color.black;
        highlightColor = new Color(100, 149, 237);

        gameObject.AddComponent<LayoutElement>();
        LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;

        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        foreach (WordStructure wordStruct in textBoxScript.structList)
        {
            if (wordStruct.isChangeable && wordStruct.alt != "N/A" && this.gameObject.name == "wordID" + wordStruct.wordID && gameObject.GetComponent<Text>().text == wordStruct.current)
            {
                changeable = true;
                gameObject.layer = 8;
            }
        }

        // While a word is changeable, highlight it with pulse
        if (changeable)
        {
            transform.GetComponent<Text>().color = Color.red;
            GameObject glower = Instantiate(glowSystem, transform.position + (transform.forward * -3.0f), transform.rotation) as GameObject;
            glower.transform.SetParent(transform);
            ParticleSystem glow = glower.GetComponent<ParticleSystem>();
            
            glow.Play();
        }
    }
    /*
    public void OnEnter(BaseEventData e)
    {
        if (paperScript.focused && changeable)
        {
            StopAllCoroutines();
            StartCoroutine(Hover());
        }
    }

    public void OnExit(BaseEventData e)
    {
        if (changeable)
        {
            StopAllCoroutines();
            StartCoroutine(Unhover());
        }
    }

    IEnumerator Hover()
    {
        StopAllCoroutines();
        yield return StartCoroutine(HOTween.To(transform, 1.0f, "position", transform.position + (transform.forward * -2), false).WaitForCompletion());
    }

    IEnumerator Unhover()
    {
        StopAllCoroutines();
        yield return StartCoroutine(HOTween.To(transform, 0.7f, "position", defaultPos, false).WaitForCompletion());
    }
     */
    public void OnDown(BaseEventData e)
    {
		
        //Debug.Log(curText);
        if (paperScript.start && paperScript.focused)
        {
            //Debug.Log("Starting");
            StartCoroutine(playCutscene.Play(gameControllerScript.curAct));
        }
        else if (paperScript.exit && paperScript.focused)
        {
            //Debug.Log("Exiting");
            Application.Quit();
        }
		//changeable && 
        else if (!paperScript.start && !paperScript.exit && paperScript.focused && lineScript.isTranslated && paperScript.focused)
        {
			foreach(WordStructure wordStruct in textBoxScript.structList){
				if(curText == wordStruct.current){
					if(wordStruct.alt != "N/A" && wordStruct.dependencies == -1 && gameObject.name == "wordID" + wordStruct.wordID && gameObject.transform.parent.parent.parent.name == gameControllerScript.curAct + "." + wordStruct.noteID){
						string noteParent = transform.parent.parent.parent.name;
						textBoxScript.clickedWordID = this.gameObject.name;
						wordStruct.isClicked = true;
						wordOptions[0] = wordStruct.current;
						wordOptions[1] = wordStruct.alt;
						createText(noteParent);
					}
				}
			}
			
            StopAllCoroutines();
            //StartCoroutine(overlay());
        }
    }
	
	void createText(string noteParent){		
		int i = 1;
		//wordOptionsUp = true;
		//currX = -6;
		currX = 0;
		currY = 6;
		currZ = 55;
		int charCount = 1;
		//print("In createText");
		foreach(string w in wordOptions){
			//Creates new object 
			
			GameObject textInstance;
			textInstance = Instantiate(textPrefab, new Vector3(currX,currY,currZ), Quaternion.identity) as GameObject;
			textInstance.name = "WordOption" + i;
		    textInstance.tag = "WordOption";
			textInstance.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
			textInstance.GetComponent<TextMesh>().text = w;
			textInstance.AddComponent<BoxCollider2D>();
		    textInstance.GetComponent<TextMesh>().color = new Color(0, 0, 0, 0);
			//currX = textInstance.transform.position.x;
			//currY = textInstance.transform.position.y;
			//currZ = textInstance.transform.position.z;
			foreach(char c in w){
				//print (charCount);
				charCount++;
			}
			float textSize = textInstance.GetComponent<BoxCollider2D>().size.x;
			float newPosX = currX - (textSize / 7);
			textInstance.transform.localPosition = new Vector3(newPosX, currY, currZ);
			
			//currX = -3.14f;
			currX = 0;
			currY = 0.5f;
			currZ = 55;
			
			i++;
			charCount = 1;

            HOTween.To(textInstance.GetComponent<TextMesh>(), 1.0f, "color", new Color(0, 0, 0, 1), false);
		}
	}
}
