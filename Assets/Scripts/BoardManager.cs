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
	
	//private Transform boardHolder;		// Not exactly sure why this is needed, I think just as a common location origin...
	public Queue <Wall> upperWalls = new Queue<Wall>();		// All upperwalls stored in this queue
	public Queue <Wall> lowerWalls = new Queue<Wall>();		// Same for lower walls
	private float lastX;										// keep track of world position to spawn next wall
	private float lowerLimit, upperLimit;						// min and max height for spawning walls
	private float screenWidth;
	private Vector3 spriteSize;

	/**
	 * Initialization for wall objects. Buff/debuffs should 
	 * probably not be spawned in the initial spawned space of any level.
	 **/
	private void boardSetup () {
		Vector3 topLeft, bottomLeft, topRight;
		spriteSize = getSpriteSize();

		//get camera bounds, so we can start spawning walls at the right place.
		topLeft = Camera.main.ViewportToWorldPoint(new Vector3 (0, 1, 0));
		bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3 (0, 0, 0));
		topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
		screenWidth = topRight.x - topLeft.x;
		lowerLimit = bottomLeft.y + (spriteSize.y / 2);
		upperLimit = topLeft.y - (spriteSize.y / 2);

		// Fill the floor and ceiling with walls across the length of the screen
		for(lastX = topLeft.x; lastX < screenWidth; lastX += spriteSize.x) {
			Wall upperClone;
			upperClone = Instantiate(wallPrefab, new Vector2 (lastX, upperLimit), Quaternion.identity) as Wall;
			upperClone.transform.Rotate (180, 0, 0);   //Rotate the wall, eventually a circular sprite could be used...idk...
			upperWalls.Enqueue(upperClone);

			Wall lowerClone;
			lowerClone = Instantiate (wallPrefab, new Vector2(lastX, lowerLimit), Quaternion.identity) as Wall;
			lowerWalls.Enqueue(lowerClone);
		}

		// need to revert lastX or there is a gap in the walls being spawned later.
		lastX -= spriteSize.x;
	}

	/**
	 * Get an instantiated version of wall to measure the sprite. Don't know of a better way to do this yet
	 */
	private Vector3 getSpriteSize() {
		Wall wall = Instantiate(wallPrefab, new Vector2 (-10, -10), Quaternion.identity) as Wall;
		Vector3 spriteWidth = wall.GetComponent<SpriteRenderer>().bounds.size;
		Destroy(wall);
		return spriteWidth;
	}

	/**
	 * In case we need to do setup other than boardSetup, there is another function
	 **/
	public void setupScene (int level) {
		boardSetup();
	}
	
	private float newYPosition(Wall lastWall){
		float nextPos = Random.Range (0, .5f) + lastWall.transform.position.y;
		float sign = Random.Range (0, 1);
		if (sign < .5) {
			nextPos = nextPos * -1f;
		}
		if (nextPos > upperLimit){
			return upperLimit;
		} else if (nextPos < lowerLimit) {
			return lowerLimit;
		} else {
			return nextPos;
		}
		
	}

	/**
	 * Each frame, check if a wall has disappeared. If it has, spawn a new one.
	 *
	 * Also, I have no idea why I need to rotate all the walls every time, I just do /cry
	 **/
	void Update () {
		Wall lastUpperWall = upperWalls.Peek ();
		Wall lastLowerWall = lowerWalls.Peek ();
		if (!lastUpperWall.GetComponent<SpriteRenderer>().isVisible) {
			lastUpperWall = upperWalls.Dequeue ();
			//lastUpperWall.selectNewSprite(); TODO
			lastUpperWall.transform.position = new Vector2 (lastX + spriteSize.x, newYPosition(lastUpperWall));
			lastUpperWall.transform.Rotate (180, 0, 0);
			upperWalls.Enqueue (lastUpperWall);

			lastLowerWall = lowerWalls.Dequeue ();
			//lastUpperWall.selectNewSprite(); TODO
			lastLowerWall.transform.position = new Vector2 (lastX + spriteSize.x, newYPosition(lastLowerWall));
			lastLowerWall.transform.Rotate (180, 0, 0);
			lowerWalls.Enqueue(lastLowerWall);
			lastX += spriteSize.x;
		}
	}
}
