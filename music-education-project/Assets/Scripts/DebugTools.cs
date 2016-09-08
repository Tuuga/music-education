using UnityEngine;
using System.Collections;

public class DebugTools : MonoBehaviour {

	public static void PrintArray(byte[] bytes) {
		foreach (byte b in bytes) {
			print(b);
		}
	}
}
