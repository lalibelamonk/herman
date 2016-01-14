using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/**
 * The board manager is the script in charge of 
 * spawning the board (the walls, buffs, debuffs, etc).
 * Basically everything other than the player and
 * the HUD.
 **/
public class BoardManager : MonoBehaviour {

	public Wall wallPrefab;  	// We pas in a prefab so we can create objects on the fly
								// This will eventually need to be done for any other object

	// So right now all the numbers for wall spawning is hard-coded in these
	// values. Eventually we will need to dynamically know sizes of the sprites. 
	// TODO make dynamic
	public static int numberWalls = 24;
	public float upperY = 5.5f;
	public float lowerY = -5.5f;
	public int spriteOffset = 2;
	
	private Transform boardHolder;		// Not exactly sure why this is needed, I think just as a common location origin...
	private Queue <Wall> upperWalls = new Queue<Wall>(numberWalls);		// All upperwalls stored in this queue
	private Queue <Wall> lowerWalls = new Queue<Wall>(numberWalls);		// Same for lower walls
	private float lastUpperX, lastLowerX;		// We need to know the position of the last wall so we can spawn a new one next to it

	/**
	 * Initialization for wall objects. Buff/debuffs should 
	 * probably not be spawned in the initial spawned space of any level.
	 * 
	 * NOTE: right now this sets up a scene smaller than the camera viewport, 
	 * its intentional so that I can see objects as they spawn.
	 **/
	void boardSetup () {
		boardHolder = new GameObject("Board").transform;

		// Fill the floor and ceiling with walls. This is ugly and hardcoded for now. 
		for(int x = 0; x < numberWalls; x = x + spriteOffset) {
			// eventually this instantiation logic should reside in the wall object
			Wall upperClone;
			upperClone = Instantiate(wallPrefab, new Vector2 (x - (numberWalls * .5f), upperY), Quaternion.identity) as Wall;
			upperClone.transform.SetParent(boardHolder);
			upperWalls.Enqueue(upperClone);

			Wall lowerClone;
			lowerClone = Instantiate (wallPrefab, new Vector2(x - (numberWalls * .5f), lowerY), Quaternion.identity) as Wall;
			lowerClone.transform.SetParent(boardHolder);
			lowerClone.transform.Rotate (180, 0, 0);   //Rotate the wall, eventually a circular sprite could be used...idk...
			lowerWalls.Enqueue(lowerClone);
		}

		// store final positions of the walls
		lastUpperX = numberWalls * .5f;
		lastLowerX = numberWalls * .5f;
	}

	/**
	 * In case we need to do setup other than boardSetup, there is another function
	 **/
	public void setupScene (int level) {
		boardSetup();
	}
	
	float newYPosition(Wall lastWall){
		float nextPos = Random.Range (0, .5f) + lastWall.transform.position.y;
		float sign = Random.Range (0, 1);
		if (sign < .5) {
			nextPos = nextPos * -1f;
		}
		if (nextPos > upperY){
			return upperY;
		} else if (nextPos < lowerY) {
			return lowerY;
		} else {
			return nextPos;
		}
		
	}

	/**
	 * Each frame, check if a wall has disappeared. If it has, spawn a new one.
	 **/
	void Update () {
		// Check both lower and upper because eventually they may disappear at different times
		Wall lastUpperWall = upperWalls.Peek ();
		Wall lastLowerWall = lowerWalls.Peek ();
		if (!lastUpperWall.GetComponent<SpriteRenderer>().isVisible) {
			lastUpperWall = upperWalls.Dequeue ();
			//lastUpperWall.selectNewSprite(); TODO
			lastUpperWall.transform.position = new Vector2 (lastUpperX + spriteOffset, newYPosition(lastUpperWall));
			upperWalls.Enqueue (lastUpperWall);
			lastUpperX += spriteOffset;
		}
		if (!lastLowerWall.GetComponent<SpriteRenderer>().isVisible) {
			lastLowerWall = lowerWalls.Dequeue ();
			//lastUpperWall.selectNewSprite(); TODO
			lastLowerWall.transform.position = new Vector2 (lastLowerX + spriteOffset, newYPosition(lastLowerWall));
			lowerWalls.Enqueue(lastLowerWall);
			lastLowerX += spriteOffset;
		}
	}
}
