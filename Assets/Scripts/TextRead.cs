/*
* Remember to set the .txt file to the script as an asset.
* Takes a whole file, reads the content and puts it into a string.
* Each word is put into an array as a string. ["Like", "this."]
* Each string in the array is then checked with a very complicated
* regular expression pattern (regex). The pattern basically helps in
* separating words and punctuation. The pattern ignores apostrophes 
* so we don't get stuff like ["that", "'", "s"] and looks
* for contractions too ["That's", "right" "."]. It checks what group (regex related) is not empty
* and inserts that group into a list.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextRead : MonoBehaviour {
	char[] delimiterChars = {' ', '\n'};
	//Text text;			//For UI Text in Unity
	public Text textPrefab;
	//public TextAsset asset;
	string assetText;
	string[] words;
	Regex re = new Regex(@"([A-Za-z]+'[a-z])([^\w\s'])|([A-Za-z]+)([^\w\s'])|([A-Za-z]+'[a-z])");
	
	List<string> list = new List<string>();
    string[] testArray = { "Hello", "world" };
	
	public Transform putWordsHere;
	
    void Start() {
		//text = GetComponent<Text>();	//text.text = string;
		//assetText = asset.text;
		words = assetText.Split(delimiterChars);
		
		//text.text = assetText;
		
		foreach(string s in words){
			Match result = re.Match(s);
			
			if(result.Success){
                // Parse conjunction + punctuation
				if(result.Groups[1].Value != "" && result.Groups[2].Value != ""){
					list.Add(result.Groups[1].Value);
					list.Add(result.Groups[2].Value);
				}
                // Parse normal word + punctuation
				else if(result.Groups[3].Value != "" && result.Groups[4].Value != ""){
					list.Add(result.Groups[3].Value);
					list.Add(result.Groups[4].Value);
				}
                // Parse conjunction
				else if(result.Groups[5].Value != ""){
					list.Add(result.Groups[5].Value);
				}
			}
			else{
				list.Add(s);
			}
		}
		
		/*for(int i = 0; i < list.Count; i++){
			print(list[i]);
		}*/
		
		for(int i = 0; i < testArray.Length; i++){
			Text textInstance;
            // TODO: Increment position in here
			textInstance = Instantiate(textPrefab, putWordsHere.position, putWordsHere.rotation) as Text;
			textInstance.transform.parent = GameObject.Find("Canvas").transform;
			textInstance.text = testArray[i];
		}
    }
	
	void Update(){
	}
}