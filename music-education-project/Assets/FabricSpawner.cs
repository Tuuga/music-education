using UnityEngine;
using System.Collections;

public class FabricSpawner : MonoBehaviour {

	public GameObject fab;

	void Start () {
		if (GameObject.Find("FabricManager") == null) {
			Instantiate(fab);
		}
	}
}
