using UnityEngine;
using System.Collections;

//dummy dictionary to test interactions
public class Dictionary : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool contains(string word){
		if (word.Length > 4) {
			return true;
		} else {
			return false;
		}


	}
}
		