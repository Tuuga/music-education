using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class FileConverter : EditorWindow {

	DefaultAsset midiFile;
	string newFileName;
	byte[] _rawMidiData;

	[MenuItem("Window/File Converter")]
	static void Init() {
		FileConverter window = (FileConverter)EditorWindow.GetWindow(typeof(FileConverter));
		window.Show();
	}

	void OnGUI() {
		midiFile = (DefaultAsset)EditorGUILayout.ObjectField(midiFile, typeof(DefaultAsset), false);
		newFileName = EditorGUILayout.TextField("New file name", newFileName);

		if (GUILayout.Button("Convert File")) {
			string assetPath = AssetDatabase.GetAssetPath(midiFile);
			_rawMidiData = System.IO.File.ReadAllBytes(assetPath);
			string newPath = "Assets/Resources/Midis/" + newFileName + ".txt";
			System.IO.File.WriteAllBytes(newPath, _rawMidiData);
			AssetDatabase.Refresh();
		}
	}
}
