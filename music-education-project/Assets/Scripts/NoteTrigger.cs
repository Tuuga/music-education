using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class NoteTrigger : EventTrigger {
	int note;

	public void SetNote(int n) {
		note = n;
	}

	public override void OnPointerDown(PointerEventData data) {

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
