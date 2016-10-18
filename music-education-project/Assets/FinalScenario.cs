using UnityEngine;
using System.Collections;

public class FinalScenario : MonoBehaviour {

	public Transform[] spawnPoints;
	public Transform flyTowards;
	public GameObject firework;
	public float force;
	public float interval;
	public float time;

	bool finalScenarioDone;


	// WIP
	public IEnumerator StartFinalScenario () {

		for (int i = 0; i < spawnPoints.Length; i++) {
			var spawnPos = spawnPoints[i].position;
			var dir = (flyTowards.position - spawnPos).normalized;
			GameObject fireworkIns = (GameObject)Instantiate(firework, spawnPos, Quaternion.identity);
			var rb = fireworkIns.GetComponent<Rigidbody2D>();
			rb.AddForce(dir * force);
		}

		yield return new WaitForSeconds(interval);
	}
}
