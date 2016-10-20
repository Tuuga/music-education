using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SynthManager : MonoBehaviour {

	public string midiFilePath = "Midis/Groove.mid";
	//Try also: "FM Bank/fm" or "Analog Bank/analog" for some different sounds "GM Bank/gm"
	public string bankFilePath = "GM Bank/gm";
	public int bufferSize = 1024;
	float[] sampleBuffer;
	MidiSequencer midiSequencer;
	StreamSynthesizer midiStreamSynthesizer;


	// Tuugas variables
	bool[] playNote = new bool[127];
	bool[] stopNote = new bool[127];

	public bool spawnFlowers;
	public bool useRandomSong;
	public FlowerSpawner spawner;

	public float noteOffTime;
	public GameObject[] keys = new GameObject[127];
	public AudioSource[] midiSamples = new AudioSource[127];
	public string[] midiSongs;

	public Text currentSong;
	int songsIndex;

	FinalScenario fc;

	void Awake() {
		midiStreamSynthesizer = new StreamSynthesizer(44100, 2, bufferSize, 40);
		sampleBuffer = new float[midiStreamSynthesizer.BufferSize];

		midiStreamSynthesizer.LoadBank(bankFilePath);

		midiSequencer = new MidiSequencer(midiStreamSynthesizer);
		midiSequencer.NoteOnEvent += new MidiSequencer.NoteOnEventHandler(MidiNoteOnHandler);
		midiSequencer.NoteOffEvent += new MidiSequencer.NoteOffEventHandler(MidiNoteOffHandler);
		ChangeSong(0);

		fc = GameObject.Find("FinalScenario").GetComponent<FinalScenario>();

		if (useRandomSong) {
			PlayRandomSong();
		}
	}

	void Update() {

		// End of song
		if (useRandomSong && !midiSequencer.isPlaying && !fc.GetRunning() && (FlowerSpawner.GetFlowersLeft() <= 0 || !spawnFlowers)) {
			fc.EndScenarioStart();
		}

		if (GameObject.FindGameObjectsWithTag("Flower").Length == 0 && fc.GetEndDone()) {
			SceneManager.LoadScene(0);
		}

		for (int i = 0; i < midiSamples.Length; i++) {
			if (stopNote[i]) {
				stopNote[i] = false;
				if (keys[i] != null && !spawnFlowers) {
					keys[i].GetComponent<Image>().color = Color.white;
				}
			}

			if (playNote[i]) {
				playNote[i] = false;
				if (keys[i] != null && !spawnFlowers) {
					// turns the note yellow
					keys[i].GetComponent<Image>().color = Color.yellow;
				}

				if (spawnFlowers) {
					spawner.SpawnFlower(i);
				}
			}
		}
	}

	// See http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.OnAudioFilterRead.html for reference code

	private void OnAudioFilterRead(float[] data, int channels) {
		midiStreamSynthesizer.GetNext(sampleBuffer);
	}

	public void MidiNoteOnHandler(int channel, int note, int velocity) {
		playNote[note] = true;
	}
	public void MidiNoteOffHandler(int channel, int note) {
		stopNote[note] = true;
	}

	// For other scripts
	public void PlayNote (int note) {
		playNote[note] = true;
	}

	// Called from UI buttons
	public void OnNote(int note) {
		if (midiSamples[note] != null) {
			midiSamples[note].Play();
		}
	}
	
	public void OnNoteOff(int note) {
		StartCoroutine(StopNote(note));
	}

	IEnumerator StopNote(int note) {
		float time = noteOffTime;
		while (time > 0) {
			time -= Time.deltaTime;
			midiSamples[note].volume -= Time.deltaTime / noteOffTime;
			yield return new WaitForEndOfFrame();
		}

		midiSamples[note].Stop();
		midiSamples[note].volume = 1f;
	}

	public void PlaySong() {
		midiSequencer.LoadMidi(midiFilePath, false);
		midiSequencer.Play();
		print("Play");
	}

	public void StopSong() {
		midiSequencer.Stop(true);

		for (int i = 0; i < stopNote.Length; i++) {
			stopNote[i] = true;
			playNote[i] = false;
		}
	}

	public void RestartScene () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ChangeSong (int n) {
		songsIndex += n;
		songsIndex = Mathf.Clamp(songsIndex, 0, midiSongs.Length - 1);
		midiFilePath = "Midis/" + midiSongs[songsIndex];
		currentSong.text = midiSongs[songsIndex];

		StopSong();
	}

	public void PlayRandomSong () {
		StopSong();
		int index = Random.Range(0, midiSongs.Length);

		midiFilePath = "Midis/" + midiSongs[index];
		currentSong.text = midiSongs[index];

		midiSequencer.LoadMidi(midiFilePath, false);
		midiSequencer.Play();
	}
}
