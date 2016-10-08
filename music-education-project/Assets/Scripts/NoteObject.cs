using UnityEngine;
using System.Collections;

public class NoteObject : MonoBehaviour {

	public Rigidbody2D[] lehdet;
	public float force;

	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}

	public void BreakFlower () {
		rb.isKinematic = false;

		Vector2 dir = new Vector2(0, 1);
		for (int i = 0; i < lehdet.Length; i++) {
			lehdet[i].isKinematic = false;
			lehdet[i].AddForce(dir * force, ForceMode2D.Impulse);
			dir = Quaternion.Euler(0, 0, -360 / lehdet.Length) * dir;
		}

	}
}
