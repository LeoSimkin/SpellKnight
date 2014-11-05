using UnityEngine;
using System.Collections;

public class BookManagerScript : MonoBehaviour {

	bool bookStateGrid = false; // False = Map, True = grid 
	char[,] gridArray = new char[6,5]; // [0,0] being bottom left corner, [5,4] being top right
	string wordBuffer = ""; // letters go here to build a word.
	float buttonSize = 0f;
	float bookTopBottomBuffer = 0f;
	int guiFontSize = 20;
	string allLetters = "AAAAAAAAABBCCDDDDEEEEEEEEEEEEFFGGGHHIIIIIIIIIJKLLLLMMNNNNNNOOOOOOOOPPQRRRRRRSSSSTTTTTTUUUUVVWWXYYZ";


	public Texture2D bookBG;
	public float letterDisplayWidth = 100f;
	public float letterDisplayHeight = 10f;
	public GUISkin bookSkin;
	public GUISkin skin1;
	//public float letterDisplayScreenHeight = 0f;


	// Use this for initialization
	void Start () {
	
		buttonSize = (Screen.width - 70) / 6; 
		bookTopBottomBuffer = ((Screen.height / 2) - ((5 * buttonSize) + (10 * 4))) / 2;
		letterDisplayHeight = buttonSize * 3/4;
		guiFontSize = Screen.width / 10;


		populateGrid ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {



		//letter display
		GUI.skin = skin1;
		skin1.box.fontSize = guiFontSize;
		GUI.BeginGroup(new Rect (Screen.width /5, Screen.height/2 - bookTopBottomBuffer - letterDisplayHeight, Screen.width*3/5, letterDisplayHeight));

		GUI.Box (new Rect (0, 0, Screen.width*3/5, letterDisplayHeight), wordBuffer);

		GUI.EndGroup ();

		//temporary control button
		GUI.BeginGroup (new Rect (100, 100, 100, 100));
		if (GUI.Button (new Rect (0, 0, 100, 100), "Clear")) {
			clearBuffer();		
		}
		GUI.EndGroup ();

		       


		//book grid
		GUI.skin = bookSkin;
		bookSkin.button.fontSize = guiFontSize;
		GUI.BeginGroup (new Rect (0, Screen.height / 2, Screen.width, Screen.height / 2));

		GUI.Box (new Rect (0, 0, Screen.width, Screen.height / 2), "");

		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 5; j++){
				if(GUI.Button (new Rect (10 + (i * (buttonSize+10)), bookTopBottomBuffer + (j * (buttonSize+10)) , buttonSize, buttonSize), gridArray[i,j].ToString() )){
					//append element to wordBuffer
					wordBuffer += gridArray[i,j];

					//move all elements down
					for(int k = j; k > 0; k--){
						gridArray[i,k] = gridArray[i,k-1];
					}
					//add new letter at top of column
					gridArray[i,0] = getLetter();
				}
			}
		}


		//GUI.Button (new Rect (10, 10, buttonSize, buttonSize), "S");

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

	public void clearBuffer(){
		wordBuffer = "";
	}
}
