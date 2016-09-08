using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MidiReader : MonoBehaviour {
	
	public string path;

	byte[] _rawMidiData;
	public List<byte> _midiBytes;

	void Awake () {
		_rawMidiData = File.ReadAllBytes(path);
		_midiBytes = new List<byte>(_rawMidiData);

		File.WriteAllBytes("Assets/Resources/Midis/Song1text.txt", _rawMidiData);

		print (CheckMidiTag(_rawMidiData));
	}
	
	bool CheckMidiTag (byte[] bytes) {
		string checker = "MThd";
		string bytesString = "";
		for (int i = 0; i < 4; i++) {
			bytesString += (char)bytes[i];
		}
		if (bytesString == checker) {
			return true;
		} else {
			return false;
		}
	}
}
