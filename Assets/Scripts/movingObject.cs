using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
       
	public LayerMask blockingLayer;
	private BoxCollider2D boxCollider;
	protected Rigidbody2D rb2D;
	
	
	//Protected, virtual functions can be overridden by inheriting classes.
	protected virtual void Awake () {
		boxCollider = GetComponent <BoxCollider2D> ();
		rb2D = GetComponent <Rigidbody2D> ();
	}
	
	protected void moveByForce (float xShare, float yShare, float magnitude) {
		rb2D.AddForce (new Vector2 (xShare * magnitude, yShare * magnitude));
	}
}