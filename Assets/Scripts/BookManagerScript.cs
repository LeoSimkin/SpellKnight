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
	GUIStyle buttonStyle;
	bool firstLetterSelected = false;
	int wordValue = 0;

	int[,] selectionStack = new int[30, 2]; // stack to keep track of order of letters
	int selectionStackIndex = 0;



	public Texture2D bookBG;
	public float letterDisplayWidth = 100f;
	public float letterDisplayHeight = 10f;
	public GUISkin bookSkin;
	public GUISkin skin1;
	public GUIStyle highlightedLetter;
	public GUIStyle selectedLetter;
	public GameObject dictionary;



	// Use this for initialization
	void Start () {
	
		buttonSize = (Screen.width - 70) / 6; 
		bookTopBottomBuffer = ((Screen.height / 2) - ((5 * buttonSize) + (10 * 4))) / 2;
		letterDisplayHeight = buttonSize * 3/4;
		guiFontSize = Screen.width / 10;
		buttonStyle = bookSkin.button;

		populateGrid ();
		clearBoolGrid ();
		initSelectionStack ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		//buttonStyle = bookSkin.button;
		//letterScoreTable = generateScoreTable ();

		//letter display
		GUI.skin = skin1;
		skin1.button.fontSize = guiFontSize;
		GUI.BeginGroup(new Rect (Screen.width /5 - letterDisplayHeight, Screen.height/2 - bookTopBottomBuffer - letterDisplayHeight, Screen.width*3/5 + (2*letterDisplayHeight), letterDisplayHeight));

		if (GUI.Button (new Rect (0, 0, letterDisplayHeight, letterDisplayHeight), "<")) {
			clearBuffer();	
			clearBoolGrid();
			initSelectionStack();		
		}
		if (GUI.Button (new Rect (letterDisplayHeight, 0, Screen.width * 3 / 5, letterDisplayHeight), wordBuffer) || GUI.Button (new Rect (letterDisplayHeight + Screen.width * 3 / 5, 0, letterDisplayHeight, letterDisplayHeight), ">")) {
			Debug.Log ("Word in dictionary?: " + dictionary.GetComponent<DictionaryManagerScript>().Contains(wordBuffer));
			if (dictionary.GetComponent<DictionaryManagerScript>().Contains(wordBuffer)){//dictionary.contains(wordBuffer)){
				// can consolidate following code into a clearWord function
				wordValue = getWordScore(wordBuffer);
				Debug.Log ("Word Score = " + wordValue);
				clearBuffer();
				for (int i = 0; i < 6; i++) {
					for (int j = 0; j < 5; j++){
						if (gridArrayIsSelected[i,j] == true) {
							//move all elements down
							for(int k = j; k > 0; k--){
								gridArray[i,k] = gridArray[i,k-1];
							}
							//add new letter at top of column
							gridArray[i,0] = getLetter();
						}
					}
				}
				clearBoolGrid ();
			}else{
				//placeholder for failed word effect
				wordBuffer = "NO";
				clearBoolGrid();
			}
			initSelectionStack();		
		}


		GUI.EndGroup ();
		/*
		//temporary control buttons
		GUI.BeginGroup (new Rect (Screen.width/4, 100, Screen.width/2, 100));
		if (GUI.Button (new Rect (0, 0, 100, 100), "Clear")) {
			clearBuffer();	
			clearBoolGrid();
			initSelectionStack();
		}
		if (GUI.Button (new Rect (Screen.width/2 - 100, 0, 100, 100), "Submit")) {
			if (dictionary.GetComponent<Dictionary>().contains(wordBuffer)){//dictionary.contains(wordBuffer)){
				// can consolidate following code into a clearWord function
				clearBuffer();
				for (int i = 0; i < 6; i++) {
					for (int j = 0; j < 5; j++){
						if (gridArrayIsSelected[i,j] == true) {
							//move all elements down
							for(int k = j; k > 0; k--){
								gridArray[i,k] = gridArray[i,k-1];
							}
							//add new letter at top of column
							gridArray[i,0] = getLetter();
						}
					}
				}
				clearBoolGrid ();
			}else{
				//placeholder for failed word effect
				wordBuffer = "NO";
				clearBoolGrid();
			}
			initSelectionStack();
		}
		GUI.EndGroup ();
		*/
		       


		//book grid
		GUI.skin = bookSkin;
		bookSkin.button.fontSize = guiFontSize;

		GUI.BeginGroup (new Rect (0, Screen.height / 2, Screen.width, Screen.height / 2));

		GUI.Box (new Rect (0, 0, Screen.width, Screen.height / 2), "");

		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 5; j++){
				//change button style based on gridArrayIsSelected
				if (gridArrayIsSelected[i,j] == true){

					if (i==getLastI() && j == getLastJ()){
						buttonStyle = highlightedLetter;
						buttonStyle.fontSize = guiFontSize;
					}
					else{
						buttonStyle = selectedLetter;
						buttonStyle.fontSize = guiFontSize;
					}
				}
				else{
					buttonStyle = bookSkin.button;
				}
				//specific letter button
				if(GUI.Button (new Rect (10 + (i * (buttonSize+10)), bookTopBottomBuffer + (j * (buttonSize+10)) , buttonSize, buttonSize), gridArray[i,j].ToString(), buttonStyle )){

					//remove or send to wordbuffer
					if(gridArrayIsSelected[i,j] == true){ //remove last letter from word buffer

						if(i == getLastI() && j == getLastJ()){
							wordBuffer = wordBuffer.Remove(wordBuffer.Length - 1, 1);
							popButtonFromStack();
							gridArrayIsSelected[i,j] = !gridArrayIsSelected[i,j];
						}
					}
					else if (isValid(i,j) && gridArrayIsSelected[i,j] == false) { //add letter to wordbuffer
						wordBuffer += gridArray[i,j];
						pushButtonToStack(i,j);
						gridArrayIsSelected[i,j] = !gridArrayIsSelected[i,j];
					}
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



	//check if the last pushed button is within one space
	bool isValid(int row, int col){
		//first letter is always valid
		if (firstLetterSelected == false){
			return true;
		}

		//set up the boundaries for check
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

		//check if the previously selected letter is in the range
		for (int i = (row - leftBound); i <= (row + rightBound); i++) {
			for (int j = col - topBound; j <= col + bottomBound; j++){
				if (i == getLastI() && j == getLastJ()){
					return true;
				}
			}
		}
		return false;
	}

	void pushButtonToStack(int i, int j){
		selectionStack [selectionStackIndex, 0] = i;
		selectionStack [selectionStackIndex, 1] = j;
		selectionStackIndex ++;

		if (selectionStackIndex > 0) {
			firstLetterSelected = true;		
		}
	}

	void popButtonFromStack(){
		selectionStackIndex --;
		selectionStack [selectionStackIndex, 0] = -1;
		selectionStack [selectionStackIndex, 1] = -1;

		if (selectionStackIndex <= 0) {
			firstLetterSelected = false;		
		}
	}

	int getLastI(){
		if (selectionStackIndex == 0){
			return -1;
		}
		else{
			return selectionStack [selectionStackIndex - 1, 0];
		}
	}

	int getLastJ(){
		if (selectionStackIndex == 0) {
			return -1;
		}
		else {
			return selectionStack [selectionStackIndex - 1, 1];
		}
	}

	//clears the selectionStack and resets the index
	void initSelectionStack(){
		for (int i = 0; i<30; i++) {
			selectionStack [i, 0] = -1;
			selectionStack [i, 1] = -1;	
		}
		selectionStackIndex = 0;
		firstLetterSelected = false;
	}

	int getWordScore(string word){
		int wordScore = 0;
		for (int i = 0; i < word.Length; i ++){
			wordScore += getLetterScore(word[i]);
		}
		return wordScore;
	}

	int getLetterScore(char letter){
		//Debug.Log ("getLetterScore");
		int letterScore = 0;
		if( letter == 'E' || letter =='A' || letter == 'I' || letter == 'L' || letter == 'N' || letter == 'O' || letter == 'R' || letter == 'S' || letter == 'T' || letter == 'U'){
			letterScore = 1;
		}
		else if (letter == 'D' || letter == 'G'){
			letterScore = 2;
		}
		else if (letter == 'B' || letter == 'C' || letter == 'M' || letter == 'P'){
			letterScore = 3;
		}
		else if (letter == 'F' || letter == 'H' || letter == 'V' || letter == 'W' || letter == 'Y'){
			letterScore = 4;
		}
		else if (letter == 'K'){
			letterScore = 5;
		}
		else if (letter == 'J' || letter == 'X'){
			letterScore = 8;
		}
		else if (letter == 'Z' || letter == 'Q'){
			letterScore = 10;
		}
		//Debug.Log ("letter score = " + letterScore);
		return letterScore;
	}

	public void clearBuffer(){
		wordBuffer = "";
	}



}
