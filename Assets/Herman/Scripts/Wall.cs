using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

/**
 * Basic top/bottom wall of the levels.
 **/
public class Wall : MonoBehaviour {

	public Sprite [] sprites;
	private SpriteRenderer renderer;

	// Use this for initialization
	void Awake () {
		renderer = GetComponent<SpriteRenderer> ();
		setSprite ();
	}

	private void setSprite () {
		int rand = (int)Random.Range (0, sprites.Length);
		renderer.sprite = sprites [rand];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
