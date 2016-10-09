using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher : MonoBehaviour {

	public void SwitchScene (int index) {
		SceneManager.LoadScene(index);
	}
}
