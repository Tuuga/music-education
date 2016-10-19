using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class FlowerSpawner : MonoBehaviour {

	public Transform startPoint;
	public Transform endPoint;
	public int startNote;
	public int endNote;

	public GameObject flower;
	public Transform canvas;

	public List<GameObject> flowers;
	public List<float> flowerSpawnTime;
	List<int> notesOrder;

	SynthManager synthMain;

	int noteRange;
	float xRange;

	static int flowersLeft;

	void Start () {
		synthMain = Camera.main.GetComponent<SynthManager>();
		if (!synthMain.spawnFlowers) {
			return;
		}

		flowers = new List<GameObject>();
		flowerSpawnTime = new List<float>();
		notesOrder = new List<int>();


		xRange = endPoint.position.x - startPoint.position.x;
		noteRange = endNote - startNote;

	}

	public static int GetFlowersLeft () {
		return flowersLeft;
	}

	public static void OneLessFlower () {
		flowersLeft--;
		print(flowersLeft);
	}

	public void SpawnFlower (int note) {
		flowersLeft++;
		GameObject flowerIns = (GameObject)Instantiate(flower, canvas);
		flowerIns.GetComponent<NoteTrigger>().SetNote(note);

		var xPos = startPoint.position.x + ((xRange / noteRange) * (note - startNote));
		xPos = Mathf.Clamp(xPos, 0, Screen.width);

		flowerIns.transform.position = new Vector3(xPos, 0);
		

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
		Fabric.EventManager.Instance.PostEvent("Sfx/Flower/Spawn");

		if (synthMain.keys[notesOrder[0]] != null) {
			synthMain.keys[notesOrder[0]].GetComponent<Image>().color = Color.white;
		}
		notesOrder.RemoveAt(0);

		if (synthMain.keys[notesOrder[0]] != null) {
			synthMain.keys[notesOrder[0]].GetComponent<Image>().color = Color.yellow;
		}
	}
}
