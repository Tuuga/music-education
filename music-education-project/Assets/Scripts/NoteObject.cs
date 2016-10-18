using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour {

	public Rigidbody2D naama;
	public Sprite[] altFaces;
	public Rigidbody2D[] lehdet;
	public Rigidbody2D[] varret;
	public float force;
	public float maxTorque;

	bool pressed;

	public bool IsPressed () {
		return pressed;
	}

	public void BreakFlower () {

		int index = Random.Range(0, altFaces.Length);
		naama.GetComponent<Image>().sprite = altFaces[index];

		naama.isKinematic = false;
		GetComponent<Animator>().enabled = false;
		pressed = true;

		Vector2 dir = new Vector2(0, 1);
		for (int i = 0; i < lehdet.Length; i++) {
			lehdet[i].isKinematic = false;
			lehdet[i].AddForce(dir * force, ForceMode2D.Impulse);
			lehdet[i].AddTorque(Random.Range(-maxTorque, maxTorque), ForceMode2D.Impulse);

			dir = Quaternion.Euler(0, 0, -360 / lehdet.Length) * dir;
		}

		dir = new Vector2(0, 1);
		for (int i = 0; i < varret.Length; i++) {
			varret[i].isKinematic = false;
			varret[i].AddForce(dir * force, ForceMode2D.Impulse);
			varret[i].AddTorque(Random.Range(-maxTorque, maxTorque), ForceMode2D.Impulse);

			dir = Quaternion.Euler(0, 0, -360 / lehdet.Length) * dir;
		}
	}
}
