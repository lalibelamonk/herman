using UnityEngine;
using System.Collections;

/**
 * A simple camera follow class to lock a camera's X position
 * to the target's
 **/
public class CameraFollowX : MonoBehaviour {

	public GameObject target;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float targetX, cameraX, interpVelocity;
		Vector3 targetDirection, targetPos;

		targetX = target.transform.position.x;
		cameraX = transform.position.x;
		targetDirection = new Vector3 (targetX - cameraX, 0f, 0f);

		interpVelocity = targetDirection.magnitude * 6f;
		targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

		transform.position = Vector3.Lerp (transform.position, targetPos, 0.25f);
	}
}
