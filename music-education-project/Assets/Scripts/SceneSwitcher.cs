using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher : MonoBehaviour {

	public void SwitchScene (int index) {
		Fabric.EventManager.Instance.PostEvent("Sfx/Button/Main");
		SceneManager.LoadScene(index);
	}
}
