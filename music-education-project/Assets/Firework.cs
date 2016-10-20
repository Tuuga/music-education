using UnityEngine;
using System.Collections;

public class Firework : MonoBehaviour {

	public Transform lehdetParent;
	public Rigidbody2D[] lehdet;

	public float force;
	public float maxTorque;

	public void SetExplosion (float time) {
		StartCoroutine(Explode(time));
	}

	public IEnumerator Explode (float time) {
		yield return new WaitForSeconds(time);

		lehdetParent.parent = transform.parent;

		Vector2 dir = new Vector2(0, 1);
		for (int i = 0; i < lehdet.Length; i++) {
			lehdet[i].isKinematic = false;
			lehdet[i].AddForce(dir * force, ForceMode2D.Impulse);
			lehdet[i].AddTorque(Random.Range(-maxTorque, maxTorque), ForceMode2D.Impulse);

			dir = Quaternion.Euler(0, 0, -360 / lehdet.Length) * dir;
		}
	}
}
