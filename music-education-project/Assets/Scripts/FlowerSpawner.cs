using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class FlowerSpawner : MonoBehaviour {

	public GameObject flower;
	public Transform canvas;
	public float xScale;

	public List<GameObject> flowers;
	public List<float> flowerSpawnTime;
	List<int> notesOrder;

	SynthManager synthMain;
	
	void Start () {
		flowers = new List<GameObject>();
		flowerSpawnTime = new List<float>();
		notesOrder = new List<int>();

		synthMain = Camera.main.GetComponent<SynthManager>();
	}

	public void SpawnFlower (int note) {
		GameObject flowerIns = (GameObject)Instantiate(flower, canvas);
		flowerIns.GetComponent<NoteTrigger>().SetNote(note);
		flowerIns.transform.position = new Vector3(note * xScale, 200f);

		flowers.Add(flowerIns);
		flowerSpawnTime.Add(Time.time);
		notesOrder.Add(note);

		if (flowers.Count > 1) {
			flowerIns.SetActive(false);
		}
	}

	public IEnumerator SetNextFlowerActive () {
		float timer;
		if (flowerSpawnTime.Count > 1) {
			timer = flowerSpawnTime[1] - flowerSpawnTime[0];
		} else {
			timer = 0f;
		}

		yield return new WaitForSeconds(timer);
		flowerSpawnTime.RemoveAt(0);
		flowers[0].SetActive(true);

		if (synthMain.keys[notesOrder[0]] != null) {
			print(synthMain.keys[notesOrder[0]].name + " White");
			synthMain.keys[notesOrder[0]].GetComponent<Image>().color = Color.white;
		}
		notesOrder.RemoveAt(0);
		if (synthMain.keys[notesOrder[0]] != null) {
			print(synthMain.keys[notesOrder[0]].name + " Yellow");
			synthMain.keys[notesOrder[0]].GetComponent<Image>().color = Color.yellow;
		}
	}
}
