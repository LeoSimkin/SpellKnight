using UnityEngine;
using System.Collections;

public class BookManagerScript : MonoBehaviour {

	bool bookStateGrid = false; // False = Map, True = grid 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		GUI.BeginGroup (new Rect (0, Screen.height / 2, Screen.width, Screen.height / 2));

		GUI.Box (new Rect (0, 0, Screen.width, Screen.height / 2), "The Book is Here");

		GUI.EndGroup ();
	}

}
