using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class DictionaryManagerScript : MonoBehaviour {
	private HashSet<String> dictionary = new HashSet<String>();


	void Awake() {
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		TextAsset wordfile = Resources.Load ("wordlist") as TextAsset;
		if (wordfile != null)
						UnityEngine.Debug.Log ("loaded");
		StringReader reader = new StringReader(wordfile.text);

		string line;
		while ( (line = reader.ReadLine()) != null ) {
			dictionary.Add(line);
			//Debug.Log (line + " Added");	
			//Debug.Log (line + " " + this.Contains(line));
			}
		UnityEngine.Debug.Log("Build time: " + stopwatch.Elapsed);
		UnityEngine.Debug.Log ("Dictionary check:" + this.Contains("ABATES"));
		reader = null;
		//Destroy (wordfile);
	}

	void Update() {
		}


	public bool Contains(string word) {
		return dictionary.Contains (word);
	}

	public void Add(string word) {
		dictionary.Add (word);
	}
	
}
