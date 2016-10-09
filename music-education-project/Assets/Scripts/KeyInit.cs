using UnityEngine;
using System.Collections;

public class KeyInit : MonoBehaviour {

	public int note;

	void Start () {
		GetComponent<KeyTrigger>().SetNote(note);
	}
}
