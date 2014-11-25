using UnityEngine;
using System.Collections;

public class MonsterManagerScript : MonoBehaviour {

	float monsterCenterH;
	float monsterCenterV;

	public Texture monsterTexture;

	int monsterHPCurrent;
	int monsterHPMAX = 100;


	// Use this for initialization
	void Start () {
	
		monsterCenterH = Screen.width / 2;
		monsterCenterV = Screen.height / 4;

		monsterHPCurrent = monsterHPMAX;		

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void takeDamage(int damage){
		monsterHPCurrent -= damage;
	}
}
