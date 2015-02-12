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
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextRead : MonoBehaviour {
	char[] delimiterChars = {' ', '\n'};
	Text text;
	public TextAsset asset;
	string assetText;
	string[] words;
	Regex re = new Regex(@"([A-Za-z]+'[a-z])([^\w\s'])|([A-Za-z]+)([^\w\s'])|([A-Za-z]+'[a-z])");
	
	List<string> list = new List<string>();
	
    void Start() {
		text = GetComponent<Text>();
		assetText = asset.text;
		words = assetText.Split(delimiterChars);
		
		foreach(string s in words){
			Match result = re.Match(s);
			
			if(result.Success){
				if(result.Groups[1].Value != "" && result.Groups[2].Value != ""){
					list.Add(result.Groups[1].Value);
					list.Add(result.Groups[2].Value);
				}
				else if(result.Groups[3].Value != "" && result.Groups[4].Value != ""){
					list.Add(result.Groups[3].Value);
					list.Add(result.Groups[4].Value);
				}
				else if(result.Groups[5].Value != ""){
					list.Add(result.Groups[5].Value);
				}
			}
			else{
				list.Add(s);
			}
		}
		
		for(int i = 0; i < list.Count; i++){
			print(list[i]);
		}
    }
	
	void Update(){
		text.text = assetText;
	}
}