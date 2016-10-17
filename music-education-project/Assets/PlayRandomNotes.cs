using UnityEngine;
using System.Collections;

public class PlayRandomNotes : MonoBehaviour {

	SynthManager synth;
	FlowerSpawner spawner;

	void Start () {
		synth = Camera.main.GetComponent<SynthManager>();
		spawner = GameObject.Find("FlowerSpawner").GetComponent<FlowerSpawner>();
		StartCoroutine(RandomNotes());
	}

	IEnumerator RandomNotes () {
		while (spawner.GetFlowersCount() < 100) {
			synth.PlayNote(Random.Range(57, 78));
			yield return new WaitForSeconds(Random.Range(0, 1f));
		}
		yield return new WaitForSeconds(1f);
	}
}
