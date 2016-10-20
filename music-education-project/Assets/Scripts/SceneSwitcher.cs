using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher : MonoBehaviour {

	int currentSceneIndex;

	void Start () {
		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		if (currentSceneIndex == 0) {
			Fabric.EventManager.Instance.PostEvent("Sfx/Ambience/Game Stop");
			Fabric.EventManager.Instance.PostEvent("Sfx/Ambience/Menu");
		} else {
			Fabric.EventManager.Instance.PostEvent("Sfx/Ambience/Menu Stop");
			Fabric.EventManager.Instance.PostEvent("Sfx/Ambience/Game");
		}
	}

	public void SwitchScene (int index) {
		Fabric.EventManager.Instance.PostEvent("Sfx/Button/Main");
		SceneManager.LoadScene(index);
	}
}
