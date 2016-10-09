using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class KeyTrigger : EventTrigger {
	int note;

	public void SetNote(int n) {
		note = n;
	}

	public override void OnPointerDown(PointerEventData data) {
		Camera.main.GetComponent<SynthManager>().OnNote(note);
	}
}
