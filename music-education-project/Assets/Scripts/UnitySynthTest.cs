using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class UnitySynthTest : MonoBehaviour {
	//Public
	public static bool useSamples;
	//Check the Midi's file folder for different songs
	public string midiFilePath = "Midis/Groove.mid";
	//Try also: "FM Bank/fm" or "Analog Bank/analog" for some different sounds "GM Bank/gm"
	public string bankFilePath = "FM Bank/fm";
	public int bufferSize = 1024;
	public int midiNote = 60;
	public int midiNoteVolume = 100;
	public int midiInstrument = 1;
	//Private 
	float[] sampleBuffer;
	float gain = 1f;
	MidiSequencer midiSequencer;
	StreamSynthesizer midiStreamSynthesizer;

	private float sliderValue = 1.0f;
	private float maxSliderValue = 127.0f;


	// Tuugas variables
	bool[] playNote = new bool[127];
	bool[] stopNote = new bool[127];

	public float noteOffTime;
	public GameObject[] keys = new GameObject[127];
	public AudioSource[] midiSamples = new AudioSource[127];
	public string[] midiSongs;

	public Text currentSong;
	int songsIndex;

	void Awake() {
		midiStreamSynthesizer = new StreamSynthesizer(44100, 2, bufferSize, 40);
		sampleBuffer = new float[midiStreamSynthesizer.BufferSize];

		midiStreamSynthesizer.LoadBank(bankFilePath);

		midiSequencer = new MidiSequencer(midiStreamSynthesizer);
		midiSequencer.NoteOnEvent += new MidiSequencer.NoteOnEventHandler(MidiNoteOnHandler);
		midiSequencer.NoteOffEvent += new MidiSequencer.NoteOffEventHandler(MidiNoteOffHandler);
		ChangeSong(0);
	}

	void Start () {
		
	}

	void Update() {

		for (int i = 0; i < midiSamples.Length; i++) {

			if (stopNote[i] && keys[i] != null) {
				stopNote[i] = false;

				if (keys[i].name.Contains("#")) {
					// turns the note black
					keys[i].GetComponent<Image>().color = Color.black;
				} else {
					// turns the note white
					keys[i].GetComponent<Image>().color = Color.white;
				}
			}

			if (playNote[i] && keys[i] != null) {
				playNote[i] = false;
				// turns the note yellow
				keys[i].GetComponent<Image>().color = Color.yellow;
			}
		}
	}

	// See http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.OnAudioFilterRead.html for reference code

	private void OnAudioFilterRead(float[] data, int channels) {

		//This uses the Unity specific float method we added to get the buffer
		midiStreamSynthesizer.GetNext(sampleBuffer);

		// Plays midi data
		if (!useSamples) {
			for (int i = 0; i < data.Length; i++) {
				data[i] = sampleBuffer[i] * gain;
			}
		}
	}

	public void MidiNoteOnHandler(int channel, int note, int velocity) {
		playNote[note] = true;	
	}

	public void MidiNoteOffHandler(int channel, int note) {
		stopNote[note] = true;
	}

	// Called from UI buttons
	public void OnNote(int note) {
		midiStreamSynthesizer.NoteOn(1, note, midiNoteVolume, midiInstrument);
		MidiNoteOnHandler(1, note, midiNoteVolume);

		midiSamples[note].Play();
	}

	
	public void OnNoteOff(int note) {
		midiStreamSynthesizer.NoteOff(1, note);
		MidiNoteOffHandler(1, note);
		
		StartCoroutine(StopNote(note));
	}

	// Called from OnNote
	// Stops the note after yield time
	IEnumerator StopNote(int note) {
		float time = noteOffTime;
		while (time > 0) {
			time -= Time.deltaTime;
			midiSamples[note].volume -= Time.deltaTime / noteOffTime;
			//yield return new WaitForSeconds(noteOffTime);
			yield return new WaitForEndOfFrame();
		}

		//midiStreamSynthesizer.NoteOff(1, note);
		//MidiNoteOffHandler(1, note);
		midiSamples[note].Stop();
		midiSamples[note].volume = 1f;
	}

	public void PlaySong() {
		midiSequencer.LoadMidi(midiFilePath, false);
		midiSequencer.Play();
	}

	public void StopSong() {
		midiSequencer.Stop(true);

		for (int i = 0; i < stopNote.Length; i++) {
			stopNote[i] = true;
			playNote[i] = false;
		}
	}

	public void ToggleUseSamples () {
		useSamples = !useSamples;
	}

	public void ChangeSong (int n) {
		songsIndex += n;
		songsIndex = Mathf.Clamp(songsIndex, 0, midiSongs.Length - 1);
		midiFilePath = "Midis/" + midiSongs[songsIndex];
		currentSong.text = midiSongs[songsIndex];

		StopSong();
	}
}
