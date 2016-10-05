using UnityEngine;
using System.Collections;

public class BlockMover : MonoBehaviour {

	public float speed;

	void Update () {
		transform.position += Vector3.down * speed * Time.deltaTime;
	}
}
