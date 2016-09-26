using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FileConverter : EditorWindow {

	List<DefaultAsset> files;
	byte[] _rawMidiData;

	[MenuItem("Window/File Converter")]
	static void Init() {
		FileConverter window = (FileConverter)EditorWindow.GetWindow(typeof(FileConverter));
		window.Show();
	}

	void OnGUI() {

		if (GUILayout.Button("Convert File")) {

			List<string> allFilePaths = new List<string>(System.IO.Directory.GetFiles("Assets/Resources/Midis/"));
			List<string> midiPaths = new List<string>();

			for (int i = 0; i < allFilePaths.Count; i++) {
				if (allFilePaths[i].EndsWith(".mid")) {
					midiPaths.Add(allFilePaths[i]);
				}
			}

			foreach (string s in midiPaths) {
				_rawMidiData = System.IO.File.ReadAllBytes(s);
				System.IO.File.WriteAllBytes(s + ".txt", _rawMidiData);
			}
			AssetDatabase.Refresh();
		}
	}
}
