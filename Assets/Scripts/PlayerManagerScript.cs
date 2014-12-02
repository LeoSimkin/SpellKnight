using UnityEngine;
using System.Collections;

public class PlayerManagerScript : MonoBehaviour {

	float healthGUIWidth;
	float healthGUIHeight;
	float healthBarWidth;
	float healthBarHeight;
	float healthBarBorder;

	// Use this for initialization
	void Start () {
		healthGUIWidth = Screen.width;
		healthGUIHeight = Screen.height / 20;
		healthBarBorder = healthGUIHeight / 5;
		healthBarWidth = healthGUIWidth - 2 * healthBarBorder;
		healthBarHeight = healthGUIHeight - 2 * healthBarBorder;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.BeginGroup (new Rect(0, 0, Screen.width, Screen.height / 20));
		GUI.Box (new Rect (0, 0, Screen.width, Screen.height / 20), "Health Bar");
		GUI.Box (new Rect (healthBarBorder, healthBarBorder, healthBarWidth, healthBarHeight), "Health");
		GUI.EndGroup ();
	}
}
