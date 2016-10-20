using UnityEngine;
using System.Collections;

public class FinalScenario : MonoBehaviour {

	public GameObject keys;
	public Transform[] spawnPoints;
	public GameObject firework;
	public Transform fireworkPoint;
	public int barrageCount;
	public float barrageInterval;
	public float spawnInterval;
	public float explosionTime;
	public float force;
	public float upForce;

	bool endDone;
	bool running;

	// DEBUG
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(StartFinalScenario());
		}
	}

	IEnumerator StartFinalScenario() {
		running = true;
		keys.SetActive(false);

		for (int b = 0; b <= barrageCount; b++) {
			for (int i = 0; i < spawnPoints.Length; i++) {
				var spawnPos = spawnPoints[i].position;
				var dir = (fireworkPoint.position - spawnPos).normalized;
				GameObject fireworkIns = (GameObject)Instantiate(firework, spawnPos, Quaternion.identity);
				var rb = fireworkIns.GetComponent<Rigidbody2D>();
				rb.AddForce((dir * force) + (Vector3.up * upForce), ForceMode2D.Impulse);

				fireworkIns.GetComponent<Firework>().SetExplosion(explosionTime);

				yield return new WaitForSeconds(spawnInterval);
			}
			yield return new WaitForSeconds(barrageInterval);
		}

		yield return new WaitForSeconds(3f);

		endDone = true;
		running = false;
	}

	public void EndScenarioStart () {
		StartCoroutine(StartFinalScenario());
	}

	public bool GetEndDone () {
		return endDone;
	}

	public bool GetRunning() {
		return running;
	}
}
