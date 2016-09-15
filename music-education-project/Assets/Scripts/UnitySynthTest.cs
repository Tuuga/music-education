using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UnitySynthTest : MonoBehaviour {
	//Public
	public static bool useSamples;
	//Check the Midi's file folder for different songs
	public string midiFilePath = "Midis/Groove.mid";
	//Try also: "FM Bank/fm" or "Analog Bank/analog" for some different sounds
	public string bankFilePath = "GM Bank/gm";
	public int bufferSize = 1024;
	public int midiNote = 60;
	public int midiNoteVolume = 100;
	public int midiInstrument = 1;
	//Private 
	private float[] sampleBuffer;
	private float gain = 1f;
	private MidiSequencer midiSequencer;
	private StreamSynthesizer midiStreamSynthesizer;

	private float sliderValue = 1.0f;
	private float maxSliderValue = 127.0f;


	// Tuugas variables
	bool[] playNote = new bool[24];
	bool[] stopNote = new bool[24];

	int playNoteIndex;
	int stopNoteIndex;

	int[] notesInMidi = { 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83 };
	string[] midiInString = { "C4", "C#4", "D4", "D#4", "E4", "F4", "F#4", "G4", "G#4", "A4", "A#4", "B4", "C5", "C5#", "D5", "D#5", "E5", "F5", "F#5", "G5", "G#5", "A5", "A#5", "B5" };
	public GameObject[] keys;
	public AudioSource[] midiSamples;

	void Awake() {
		midiStreamSynthesizer = new StreamSynthesizer(44100, 2, bufferSize, 40);
		sampleBuffer = new float[midiStreamSynthesizer.BufferSize];

		midiStreamSynthesizer.LoadBank(bankFilePath);

		midiSequencer = new MidiSequencer(midiStreamSynthesizer);
		midiSequencer.LoadMidi(midiFilePath, false);
		//These will be fired by the midiSequencer when a song plays. Check the console for messages
		midiSequencer.NoteOnEvent += new MidiSequencer.NoteOnEventHandler(MidiNoteOnHandler);
		midiSequencer.NoteOffEvent += new MidiSequencer.NoteOffEventHandler(MidiNoteOffHandler);
	}

	void Update() {

		// Plays note samples
		for (int i = 0; i < midiSamples.Length; i++) {
			if (stopNote[i]) {
				if (keys[i].name.Contains("#")) {
					// turns the note black
					keys[i].GetComponent<Image>().color = Color.black;
				} else {
					// turns the note white
					keys[i].GetComponent<Image>().color = Color.white;
				}
			}

			if (playNote[i]) {
				// turns the note yellow
				keys[i].GetComponent<Image>().color = Color.yellow;
			}

			// Stops the note sample
			if (stopNote[i]) {
				if (useSamples) {
					midiSamples[i].Stop();
				}
				stopNote[i] = false;
			}

			// Plays the note sample
			if (playNote[i]) {
				if (useSamples) {
					midiSamples[i].Play();
				}
				playNote[i] = false;
			}
		}
	}

	// See http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.OnAudioFilterRead.html for reference code
	//	If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.
	//
	//	The filter is inserted in the same order as the MonoBehaviour script is shown in the inspector. 	
	//	OnAudioFilterRead is called everytime a chunk of audio is routed thru the filter (this happens frequently, every ~20ms depending on the samplerate and platform). 
	//	The audio data is an array of floats ranging from [-1.0f;1.0f] and contains audio from the previous filter in the chain or the AudioClip on the AudioSource. 
	//	If this is the first filter in the chain and a clip isn't attached to the audio source this filter will be 'played'. 
	//	That way you can use the filter as the audio clip, procedurally generating audio.
	//
	//	If OnAudioFilterRead is implemented a VU meter will show up in the inspector showing the outgoing samples level. 
	//	The process time of the filter is also measured and the spent milliseconds will show up next to the VU Meter 
	//	(it turns red if the filter is taking up too much time, so the mixer will starv audio data). 
	//	Also note, that OnAudioFilterRead is called on a different thread from the main thread (namely the audio thread) 
	//	so calling into many Unity functions from this function is not allowed ( a warning will show up ). 	
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
		for (int i = 0; i < notesInMidi.Length; i++) {
			if (notesInMidi[i] == note) {
				playNoteIndex = i;
				playNote[i] = true;
			}
		}
	}

	public void MidiNoteOffHandler(int channel, int note) {
		for (int i = 0; i < notesInMidi.Length; i++) {
			if (notesInMidi[i] == note) {
				stopNoteIndex = i;
				stopNote[i] = true;
			}
		}
	}

	// Called from UI buttons
	public void OnNote(int note) {
		midiStreamSynthesizer.NoteOn(1, note, midiNoteVolume, midiInstrument);
		MidiNoteOnHandler(1, note, midiNoteVolume);

		for (int i = 0; i < notesInMidi.Length; i++) {
			if (notesInMidi[i] == note) {
				midiSamples[i].Play();
			}
		}

		StartCoroutine(StopNote(note));
	}

	// Called from OnNote
	// Stops the note after yield time
	IEnumerator StopNote(int note) {
		yield return new WaitForSeconds(0.5f);
		midiStreamSynthesizer.NoteOff(1, note);
		MidiNoteOffHandler(1, note);

		for (int i = 0; i < notesInMidi.Length; i++) {
			if (notesInMidi[i] == note) {
				midiSamples[i].Stop();
			}
		}
	}

	public void PlaySong() {
		midiSequencer.Play();
	}

	public void StopSong() {
		midiSequencer.Stop(true);
	}

	public void ToggleUseSamples () {
		useSamples = !useSamples;
	}
}
