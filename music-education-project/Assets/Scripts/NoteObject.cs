using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class NoteObject : MonoBehaviour {

	public Rigidbody2D naama;
	public Rigidbody2D[] lehdet;
	public float force;
	public float maxTorque;

	bool pressed;

	public bool IsPressed () {
		return pressed;
	}

	public void BreakFlower () {
		naama.isKinematic = false;

		Vector2 dir = new Vector2(0, 1);
		for (int i = 0; i < lehdet.Length; i++) {
			lehdet[i].isKinematic = false;
			lehdet[i].AddForce(dir * force, ForceMode2D.Impulse);
			lehdet[i].AddTorque(Random.Range(-maxTorque, maxTorque), ForceMode2D.Impulse);

			dir = Quaternion.Euler(0, 0, -360 / lehdet.Length) * dir;
			pressed = true;
		}
	}
}
