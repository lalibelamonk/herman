using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

	public float restartLevelDelay = 1f;
	public float accelerateForce = 40;
	public float startSpeed;

	public bool NSFW;

	private Animator animator;
	private int armor;
	private bool isDead;
	private float maxSpeed = 6f;

	protected override void Awake () {
		if (NSFW) {
			animator = GetComponent<Animator>();
		}
		base.Awake ();
	}

	private void Start () {
		rb2D.isKinematic = false;
		rb2D.velocity = new Vector2 (startSpeed, 0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey ("up")) {
			accelerate(Input.GetAxis ("Vertical"), accelerateForce);
		} else {
			deccelerate();
		}
	}

	private void accelerate (float mag, float force) {
		if (NSFW) {
			animator.SetBool ("isAccelerating", true);
		}
		moveByForce (0f, 1f, mag * force);
	}

	private void deccelerate () {
		if (NSFW) {
			animator.SetBool ("isAccelerating", false);
		}
	}
	
	private void moveByForce (float xShare, float yShare, float magnitude) {
		rb2D.AddForce (new Vector2 (xShare * magnitude, yShare * magnitude));
		
		if(rb2D.velocity.y > maxSpeed) {
			rb2D.velocity = new Vector2(rb2D.velocity.x, maxSpeed);
		}
		if(rb2D.velocity.y < (maxSpeed * -1f)) {
			rb2D.velocity = new Vector2(rb2D.velocity.x, (maxSpeed * -1f));
		}
	}
	
	// called on collisions with walls
	public void checkIfGameOver () {
		if (armor <= 0) {
			restart();
		}
	}
	
	private void restart() {
		if (NSFW) {
			animator.enabled = false;
		}
		isDead = true;
		rb2D.isKinematic = true;
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void loseArmor (int loss) {
		armor -= loss;
		checkIfGameOver();	
	}
	
	private void OnTriggerEnter2D (Collider2D trigger) {
		if (trigger.tag == "Wall") {
			loseArmor (1);
		} else if (trigger.tag == "buff_1") {
		} else if (trigger.tag == "debuff_1") {
		}
	}
}
