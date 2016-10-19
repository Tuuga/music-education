using UnityEngine;
using System.Collections;

public class PlayRandomNotes : MonoBehaviour {

	SynthManager synth;

	void Start () {
		synth = Camera.main.GetComponent<SynthManager>();
		StartCoroutine(RandomNotes());
	}

	IEnumerator RandomNotes () {
		while (FlowerSpawner.GetFlowersLeft() < 100) {
			synth.PlayNote(Random.Range(57, 78));
			yield return new WaitForSeconds(Random.Range(0, 1f));
		}
		yield return new WaitForSeconds(1f);
	}
}
