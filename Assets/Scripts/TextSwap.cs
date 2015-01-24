using UnityEngine;
using System.Collections;

public class Hover1 : MonoBehaviour {

	public string OnMouseOverText = "All that glisters is not gold -";
	public string OnMouseExitText = "Nōn omne quod nitet aurum est -";

	void OnMouseOver(){
		GetComponent<TextMesh>().text = OnMouseOverText;
	}
	
	void OnMouseExit(){
		GetComponent<TextMesh>().text = OnMouseExitText;
	}

}
