using UnityEngine;
using System.Collections;

public class BlockMover : MonoBehaviour {

	public float speed;

	void Update () {
		transform.position += -transform.forward * speed * Time.deltaTime;
	}
}
