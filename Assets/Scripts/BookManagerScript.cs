using UnityEngine;
using System.Collections;

public class BookManagerScript : MonoBehaviour {

	bool bookStateGrid = false; // False = Map, True = grid 
	char[,] gridArray = new char[6,5]; // [0,0] being top left corner, [5,4] being bottom right
	bool[,] gridArrayIsSelected = new bool[6, 5]; //if true, the letter has been selected and should be glowing

	string wordBuffer = ""; // letters go here to build a word.
	float buttonSize = 0f;
	float bookTopBottomBuffer = 0f;
	int guiFontSize = 20;
	string allLetters = "AAAAAAAAABBCCDDDDEEEEEEEEEEEEFFGGGHHIIIIIIIIIJKLLLLMMNNNNNNOOOOOOOOPPQRRRRRRSSSSTTTTTTUUUUVVWWXYYZ";
	Dictionary dictionary;
	GUIStyle buttonStyle;


	public Texture2D bookBG;
	public float letterDisplayWidth = 100f;
	public float letterDisplayHeight = 10f;
	public GUISkin bookSkin;
	public GUISkin skin1;
	public GUIStyle highlightedLetter;
	//public float letterDisplayScreenHeight = 0f;


	// Use this for initialization
	void Start () {
	
		buttonSize = (Screen.width - 70) / 6; 
		bookTopBottomBuffer = ((Screen.height / 2) - ((5 * buttonSize) + (10 * 4))) / 2;
		letterDisplayHeight = buttonSize * 3/4;
		guiFontSize = Screen.width / 10;
		buttonStyle = bookSkin.button;

		dictionary = new Dictionary();

		populateGrid ();
		clearBoolGrid ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		//buttonStyle = bookSkin.button;


		//letter display
		GUI.skin = skin1;
		skin1.box.fontSize = guiFontSize;
		GUI.BeginGroup(new Rect (Screen.width /5, Screen.height/2 - bookTopBottomBuffer - letterDisplayHeight, Screen.width*3/5, letterDisplayHeight));

		GUI.Box (new Rect (0, 0, Screen.width*3/5, letterDisplayHeight), wordBuffer);

		GUI.EndGroup ();

		//temporary control button
		GUI.BeginGroup (new Rect (Screen.width/4, 100, Screen.width/2, 100));
		if (GUI.Button (new Rect (0, 0, 100, 100), "Clear")) {
			clearBuffer();		
		}
		if (GUI.Button (new Rect (Screen.width/2 - 100, 0, 100, 100), "Submit")) {
			if (dictionary.contains(wordBuffer)){
				wordBuffer = "YES";
			}else{
				wordBuffer = "NO";
			}
		}
		GUI.EndGroup ();

		       


		//book grid
		GUI.skin = bookSkin;
		bookSkin.button.fontSize = guiFontSize;
		GUI.BeginGroup (new Rect (0, Screen.height / 2, Screen.width, Screen.height / 2));

		GUI.Box (new Rect (0, 0, Screen.width, Screen.height / 2), "");

		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 5; j++){
				//change button style based on gridArrayIsSelected
				if (gridArrayIsSelected[i,j] == true){
					buttonStyle = highlightedLetter;
				}
				else{
					buttonStyle = bookSkin.button;
				}

				if(GUI.Button (new Rect (10 + (i * (buttonSize+10)), bookTopBottomBuffer + (j * (buttonSize+10)) , buttonSize, buttonSize), gridArray[i,j].ToString(), buttonStyle )){
					//append element to wordBuffer
					if(gridArrayIsSelected[i,j] == true){
						wordBuffer = wordBuffer.Remove(wordBuffer.Length -1);
					}
					else {
						wordBuffer += gridArray[i,j];
					}

					gridArrayIsSelected[i,j] = !gridArrayIsSelected[i,j];
			


					/*
					//move all elements down
					for(int k = j; k > 0; k--){
						gridArray[i,k] = gridArray[i,k-1];
					}
					//add new letter at top of column
					gridArray[i,0] = getLetter();
					*/
				}
			}
		}

		GUI.EndGroup ();
	}


	char getLetter(){
		char letter = allLetters[Random.Range (0,allLetters.Length)]; 

		return letter;
	}

	void populateGrid(){
		for (int i = 0; i<6; i++) {
			for (int j = 0; j<5; j++) {
					gridArray [i, j] = getLetter ();
			}
		}
	}

	void clearBoolGrid(){
		for (int i = 0; i<6; i++) {
			for (int j = 0; j<5; j++) {
				gridArrayIsSelected[i,j] = false;
			}
		}
	}

	bool generateValidZone(int row, int col){
		int leftBound = 1;
		int rightBound = 1;
		int topBound = 1; 
		int bottomBound = 1;

		if (row == 0){
			leftBound = 0;
		}

		if(row == 5) {
			rightBound = 0;
		}

		if (col == 0) {
			topBound = 0;		
		}

		if (col == 4) {
			bottomBound = 0;		
		}

		for (int i = (row - leftBound); i <= (row + rightBound); i++) {
			for (int j = col - topBound; j <= col + bottomBound; j++){
				if (gridArrayIsSelected[i,j] == true){
					return true;
				}
			}
		}

		return false;
	}

	public void clearBuffer(){
		wordBuffer = "";
	}



}
