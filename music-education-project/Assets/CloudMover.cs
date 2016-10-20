using UnityEngine;
using System.Collections;

public class CloudMover : MonoBehaviour {

	float speed;

	void Update () {
		transform.position += Vector3.right * speed * Time.deltaTime;

		// destroys cloud when it's off screen
		if (transform.position.x > 7f) {
			Destroy(gameObject);
		}
	}

	public void SetSpeed (float s) {
		speed = s;
	}
}