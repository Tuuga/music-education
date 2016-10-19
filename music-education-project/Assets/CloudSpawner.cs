using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour {

	public GameObject cloud;

	public Sprite[] cloudSprites;
	public Transform spawnPoint;
	public float yVariation;
	public float spawnInterval;
	public float spawnIntervalVariation;
	public float cloudSpeed;
	public float cloudSpeedVariation;

	void Start () {
		StartCoroutine(SpawnClouds());
	}

	IEnumerator SpawnClouds () {
		while (true) {
			var spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y + Random.Range(-yVariation, yVariation));
			GameObject cloudIns = (GameObject)Instantiate(cloud, spawnPos, Quaternion.identity);
			int spriteIndex = Random.Range(0, cloudSprites.Length);
			cloudIns.GetComponent<SpriteRenderer>().sprite = cloudSprites[spriteIndex];

			var speed = cloudSpeed + Random.Range(-cloudSpeedVariation, cloudSpeedVariation);
			cloudIns.GetComponent<CloudMover>().SetSpeed(speed);

			yield return new WaitForSeconds (spawnInterval + Random.Range(-spawnIntervalVariation, spawnIntervalVariation));
		}
	}
}
