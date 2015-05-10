using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class wordOptionsScript : MonoBehaviour {
	public static string textMeshWord = "";
	
	GameObject textBox;
	TextBox textBoxScript;
	
	void Start(){
		textBox = GameObject.Find("TextBox");
		textBoxScript = textBox.GetComponent<TextBox>();
	}
	
	void OnMouseDown(){
		//Gets the the word in GameObject text box and sets it to a string
		textMeshWord = GetComponent<TextMesh>().text;
		
		if(gameObject.transform.name == "WordOption2"){
			textBoxScript.Swap();
			
			//Reloads notes pre-parsed			
			// for(int k = textBoxScript.allNoteLines[currAct].Length - 1; k >= 0; k--){
				// string noteName = gameControllerScript.noteArray[currAct][k].name;
				
				// GameObject.Find(noteName).transform.GetChild(0).GetComponent<CanvasScript>().Parser();
			// }
			
		}
	    StartCoroutine(FadeOut());
	}

    IEnumerator FadeOut()
    {
        //Destroy the words from game world
        HOTween.To(GameObject.Find("WordOption1").GetComponent<TextMesh>(), 1.0f, "color", new Color(0, 0, 0, 0), false);
        yield return StartCoroutine(HOTween.To(GameObject.Find("WordOption2").GetComponent<TextMesh>(), 1.0f, "color", new Color(0, 0, 0, 0), false).WaitForCompletion());

        Destroy(GameObject.Find("WordOption1"));
        Destroy(GameObject.Find("WordOption2"));
    }
}
