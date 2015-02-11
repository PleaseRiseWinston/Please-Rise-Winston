//Takes in a file, and reads that text.
//Remember to set the .txt file to the script as an asset.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextRead : MonoBehaviour {
	Text text;
	
	public TextAsset asset;
	string assetText;
	
    void Start() {
		text = GetComponent<Text>();
		assetText = asset.text;
		
        print(assetText);
    }
	
	void Update(){
		text.text = assetText;
	}
}