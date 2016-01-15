using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	public float levelStartDelay = 2f;
	public static GameManager instance = null;
	private BoardManager boardScript;
	
	private Text levelText;
	private GameObject levelImage;
	private bool doingSetup;
	private int level = 1;
	
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		//DontDestroyOnLoad(gameObject);
		initGame();
	}
	
	private void OnLevelWasLoaded (int index) {
		level++;
		
		initGame();
	}
	
	void initGame() {
		boardScript = GetComponent<BoardManager>();
		doingSetup = true;
		//levelImage = GameObject.Find ("levelImage");
		//levelText = GameObject.Find ("levelText").GetComponent<Text>();
		//levelText.text = "Day " + level;
		//levelImage.SetActive (true);
		//Invoke("hideLevelImage", levelStartDelay);

		boardScript.setupScene(level);
	}
	
	public void gameOver() {
		//levelText.text = "After " + level + " days, you starved.";
		//levelImage.SetActive(true);
		//enabled = false;
	}
}