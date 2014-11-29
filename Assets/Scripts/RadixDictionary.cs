using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class RadixDictionary : MonoBehaviour {
	private Dictionary<char, Node> roots = new Dictionary<char, Node>(26);


	void Awake() {
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		TextAsset wordfile = Resources.Load ("wordlist") as TextAsset;
		if (wordfile != null)
						UnityEngine.Debug.Log ("loaded");
		StringReader reader = new StringReader(wordfile.text);

		string line;
		while ( (line = reader.ReadLine()) != null ) {
			this.Add(line);
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


	/**
	 * Searches the dictionary for the word input.
	 * Returns true if word is in dictionary, false if not.
	 */
	public bool Contains(string word) {
		Node node;
		bool childFound;

		if (roots.TryGetValue(word[0], out node)) {
			word = word.Substring(1);
		}		else {
			return false;
		}

		/**
		  * loop is exited by one of two methods:
		  * no match -> early termination (return false) 
		  * or word.Length == 0 -> check whether node.EndOfWord == true  (return node.EndOfWord)
		  */

		while (word.Length > 0) {
			childFound = false;
			for (int i = 1; i < word.Length + 1; i++) {
				if (node.Children.ContainsKey(word.Substring(0, i))) {
					childFound = true;
					node = node.Children[word.Substring(0, i)];
					word = word.Substring(i);
					break;
				}
			}

			if (!childFound) return false;
		}

		// word.length == 0; now at the end of word
		return node.EndOfWord;
	}

	/**
	 * Insert a word into the dictionary.
	 */
	public void Add(string word) {
		Node node;

		if (!roots.ContainsKey(word[0])) {
			roots[word[0]] = new Node();
		}

		node = roots[word[0]];
		word = word.Substring(1);

		/**
		 * While loop exited at the end of the word (substring length = 0)
		 * Or when no child nodes match the remaining word
		 */
		bool matchFound = true;
		while (word.Length > 0 && matchFound) {
			// if current node has child with key of just first letter 
			if (node.Children.ContainsKey(word[0].ToString())) {
				node = node.Children[word[0].ToString()];
				word = word.Substring(1);
				continue;
			} else {
				// find whether child exists with key starting with same letter
				matchFound = false;
				foreach (string key in node.Children.Keys) {
					if (key[0] == word[0]) {
						matchFound = true;
						// find how many characters match
						int matchingChars = 0;
						while (matchingChars < word.Length && matchingChars < key.Length) {
							if (word[matchingChars] == key[matchingChars]) {
								matchingChars++;
							} else {
								break;
							}
						}

						// if matchingChars == word.length (word remainder == key)
						// break out of While loop, EndOfWord = true
						if (matchingChars == word.Length) {
							node = node.Children[word];
							word = "";
							break;				
						// if matchingChars == key.Length, multi-letter matching node. OK to select this as node and continue						
						} else if (matchingChars == key.Length) {
							node = node.Children[key];
							word = word.Substring(matchingChars);
							break;
						} else {
						/** split node
						 * stores node with partial match in temp variable
						 * creates new node with key = matching substring
						 * adds stored node as child to new node with key = unmatched substring of key
						 * allows old node to retain its own child relationships
						 */
							Node nodeTemp = node.Children[key];
							node.Children.Remove(key);
							node.Children[word.Substring(0,matchingChars)] = new Node();
							// traverse tree
							node = node.Children[word.Substring(0,matchingChars)];
							node.Children[key.Substring(matchingChars)] = nodeTemp;
							word = word.Substring(matchingChars);
							
							matchFound = false;
							break;
						}
					}
				}
				// got through list of Children keys with no match?
				if (!matchFound) break;
			}
		}
			
		if (word.Length != 0) {
			node.Children[word] = new Node();
			node.Children[word].EndOfWord = true;
			return;
		} else {
			node.EndOfWord = true;
		}
	}


	
	private class Node {
		public bool EndOfWord = false; 
		public Dictionary<string,Node> Children = new Dictionary<string,Node>();
	}	
}
