using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class NoteTrigger : EventTrigger {
	int note;
	bool animDone;

	public void SetNote(int n) {
		note = n;
	}

	void Start () {
		StartCoroutine(SetAnimDone());
	}

	IEnumerator SetAnimDone () {
		yield return new WaitForSeconds(1.8f);
		animDone = true;
	}

	public override void OnPointerDown(PointerEventData data) {
		if (!animDone) { return; }

		var spawner = GameObject.Find("FlowerSpawner").GetComponent<FlowerSpawner>();

		if (!GetComponent<NoteObject>().IsPressed()) {
			spawner.flowers.Remove(gameObject);
			if (spawner.flowers.Count > 0) {	
				StartCoroutine(spawner.SetNextFlowerActive());
			}
			Camera.main.GetComponent<SynthManager>().OnNote(note);
			GetComponent<NoteObject>().BreakFlower();
			Destroy(gameObject, 5f);
		}
	}
}
