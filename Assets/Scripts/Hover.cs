using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {
	
	public bool translateable;
	
	public Color OnMouseOverColor = Color.yellow;
	public Font OnMouseOverFont;
	public Font OnMouseExitFont;
	
	void Start(){
		if (translateable){
			GetComponent<TextMesh>().color = OnMouseOverColor;
		}
	}
	
	void OnMouseOver(){
		GetComponent<TextMesh>().font = OnMouseOverFont;
	}
	
	void OnMouseExit(){
		GetComponent<TextMesh>().font = OnMouseExitFont;
	}

}
