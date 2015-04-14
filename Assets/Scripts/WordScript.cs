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
    private string curText;
    private bool changeable;

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

    void Start()
    {
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

        // Each word object gets the paper that it is on
        paper = transform.parent.parent.parent.gameObject;
        paperScript = paper.GetComponent<PaperScript>();
		

        // Each word object gets the line that it is on
        line = transform.parent.gameObject;
        lineScript = line.GetComponent<LineScript>();
        cameraController = GameObject.FindGameObjectWithTag("CameraController");
        playCutscene = cameraController.GetComponent<PlayCutscene>();

        // TODO: Read text into textOptions array here
        
        curText = GetComponent<Text>().text;
        defaultColor = Color.black;
        highlightColor = Color.red;

        gameObject.AddComponent<LayoutElement>();
        LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;

        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        // While a word is changeable and the current line is translated, highlight it
        if (changeable && lineScript.isTranslated)
        {
            // TODO: Add animations to highlight this changeable word; probably going to have a pulsing glow
        }
    }

    public void OnDown(BaseEventData e)
    {
		
        //Debug.Log(curText);
        if (paperScript.start)
        {
            // TODO: Detect current Act
            Debug.Log("Starting");
            StartCoroutine(playCutscene.Play(1));
        }
        else if (paperScript.exit)
        {
            Debug.Log("Exiting");
            Application.Quit();
        }
		//changeable && 
        else if (!paperScript.start && !paperScript.exit && paperScript.focused && lineScript.isTranslated)
        {
			foreach(WordStructure wordStruct in textBoxScript.structList){
				if(curText == wordStruct.current){
					if(wordStruct.alt != "N/A" && wordStruct.dependencies == -1){
						textBoxScript.clickedWordID = this.gameObject.name;
						wordStruct.isClicked = true;
						wordOptions[0] = wordStruct.current;
						wordOptions[1] = wordStruct.alt;
						createText();
					}
				}
			}
			
            StopAllCoroutines();
            //StartCoroutine(overlay());
        }
    }
	
	void createText(){		
		int i = 1;
		//wordOptionsUp = true;
		textPrefab = GameObject.Find("wordOptionMesh");
		//currX = -6;
		currX = 0;
		currY = 6;
		currZ = -5;
		int charCount = 1;
		
		foreach(string w in wordOptions){
			//Creates new object 
			GameObject textInstance;
			textInstance = Instantiate(textPrefab, new Vector3(currX,currY,currZ), Quaternion.identity) as GameObject;
			textInstance.name = "WordOption" + i;
			textInstance.transform.parent = GameObject.Find("GamePaper").transform;
			textInstance.GetComponent<TextMesh>().text = w;
			textInstance.AddComponent<BoxCollider2D>();
			
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
			currY = -2;
			currZ = -5;
			
			i++;
			charCount = 1;
		}
	}

}
