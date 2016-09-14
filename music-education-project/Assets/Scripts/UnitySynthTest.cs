using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]
public class UnitySynthTest : MonoBehaviour
{
	//Public
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
	bool[] playNote = new bool[13];
	bool[] stopNote = new bool[13];

	int playNoteIndex;
	int stopNoteIndex;

	int[] notesInMidi = {62, 63, 65, 67, 69, 70, 72, 74, 75, 77, 79, 81, 82};
	string[] midiInString = { "D4", "D#4", "F4", "G4", "A4", "A#4", "C5", "D5", "D#5", "F5", "G5", "A5", "A#5"};
	//string[] midiInString = {"C4", "C#4", "D4", "D#4", "F4", "F#4", "G4", "G#4", "A4", "A#4", "B4", "C5", "C5#", "D5", "D#5", "E5", "F5", "F5#", "G5", "G#5", "A5", "A#5", "B5"};
	GameObject[] keys;
	public AudioSource[] midiSamples;
	
	// Awake is called when the script instance
	// is being loaded.
	void Awake ()
	{
		midiStreamSynthesizer = new StreamSynthesizer (44100, 2, bufferSize, 40);
		sampleBuffer = new float[midiStreamSynthesizer.BufferSize];		
		
		midiStreamSynthesizer.LoadBank (bankFilePath);
		
		midiSequencer = new MidiSequencer (midiStreamSynthesizer);
		midiSequencer.LoadMidi (midiFilePath, false);
		//These will be fired by the midiSequencer when a song plays. Check the console for messages
		midiSequencer.NoteOnEvent += new MidiSequencer.NoteOnEventHandler (MidiNoteOnHandler);
		midiSequencer.NoteOffEvent += new MidiSequencer.NoteOffEventHandler (MidiNoteOffHandler);

		keys = GameObject.FindGameObjectsWithTag("Key");
	}
	
	// Start is called just before any of the
	// Update methods is called the first time.
	void Start ()
	{
		
	}
	
	// Update is called every frame, if the
	// MonoBehaviour is enabled.
	void Update ()
	{
		//Demo of direct note output
        if (Input.GetKeyDown(KeyCode.A))
            midiStreamSynthesizer.NoteOn (1, midiNote, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.A))
            midiStreamSynthesizer.NoteOff (1, midiNote);
        if (Input.GetKeyDown(KeyCode.W))
            midiStreamSynthesizer.NoteOn (1, midiNote + 1, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.W))
            midiStreamSynthesizer.NoteOff (1, midiNote + 1);
        if (Input.GetKeyDown(KeyCode.S))
            midiStreamSynthesizer.NoteOn (1, midiNote + 2, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.S))
            midiStreamSynthesizer.NoteOff (1, midiNote + 2);		
        if (Input.GetKeyDown(KeyCode.E))
            midiStreamSynthesizer.NoteOn (1, midiNote + 3, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.E))
            midiStreamSynthesizer.NoteOff (1, midiNote + 3);
        if (Input.GetKeyDown(KeyCode.D))
            midiStreamSynthesizer.NoteOn (1, midiNote + 4, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.D))
            midiStreamSynthesizer.NoteOff (1, midiNote + 4);
        if (Input.GetKeyDown(KeyCode.F))
            midiStreamSynthesizer.NoteOn (1, midiNote + 5, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.F))
            midiStreamSynthesizer.NoteOff (1, midiNote + 5);
        if (Input.GetKeyDown(KeyCode.T))
            midiStreamSynthesizer.NoteOn (1, midiNote + 6, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.T))
            midiStreamSynthesizer.NoteOff (1, midiNote + 6);
        if (Input.GetKeyDown(KeyCode.G))
            midiStreamSynthesizer.NoteOn (1, midiNote + 7, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.G))
            midiStreamSynthesizer.NoteOff (1, midiNote + 7);		
        if (Input.GetKeyDown(KeyCode.Y))
            midiStreamSynthesizer.NoteOn (1, midiNote + 8, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.Y))
            midiStreamSynthesizer.NoteOff (1, midiNote + 8);
        if (Input.GetKeyDown(KeyCode.H))
            midiStreamSynthesizer.NoteOn (1, midiNote + 9, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.H))
            midiStreamSynthesizer.NoteOff (1, midiNote + 9);
        if (Input.GetKeyDown(KeyCode.U))
            midiStreamSynthesizer.NoteOn (1, midiNote + 10, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.U))
            midiStreamSynthesizer.NoteOff (1, midiNote + 10);
        if (Input.GetKeyDown(KeyCode.J))
            midiStreamSynthesizer.NoteOn (1, midiNote + 11, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.J))
            midiStreamSynthesizer.NoteOff (1, midiNote + 11);		
        if (Input.GetKeyDown(KeyCode.K))
            midiStreamSynthesizer.NoteOn (1, midiNote + 12, midiNoteVolume, midiInstrument);
        if (Input.GetKeyUp(KeyCode.K))
            midiStreamSynthesizer.NoteOff (1, midiNote + 12);

		// Plays note samples
		for (int i = 0; i < midiSamples.Length; i++) {
		//for (int i = 0; i < midiInString.Length; i++) {
			
			for (int j = 0; j < keys.Length; j++) {
				if (stopNote[i] && keys[j].name == midiInString[i]) {
					// white or black
					if (keys[j].name.Contains("#")) {
						keys[j].GetComponent<Image>().color = Color.black;
					} else {
						keys[j].GetComponent<Image>().color = Color.white;
					}
				}

				if (playNote[i] && keys[j].name == midiInString[i]) {
					// yellow
					keys[j].GetComponent<Image>().color = Color.yellow;
				}
			}

			
			if (stopNote[i]) {
				//midiSamples[i].Stop();
				stopNote[i] = false;
			}

			if (playNote[i]) {
				//midiSamples[i].Play();
				playNote[i] = false;
			}
			
		}
	}

	// OnGUI is called for rendering and handling
	// GUI events.
	void OnGUI ()
	{
		// Make a background box
		GUILayout.BeginArea (new Rect (Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 300));
		
		
		if (GUILayout.Button ("Play Song")) {
			midiSequencer.Play ();
		}
		if (GUILayout.Button ("Stop Song")) {
			midiSequencer.Stop (true);
		}		
		GUILayout.Label("Play keys AWSEDFTGYHJK");
		
		GUILayout.Box("Instrument: " + Mathf.Round(midiInstrument));
		midiInstrument = (int)GUILayout.HorizontalSlider (midiInstrument, 0.0f, maxSliderValue);
		GUILayout.Box("Volume: " + Mathf.Round(midiNoteVolume));
		midiNoteVolume = (int)GUILayout.HorizontalSlider (midiNoteVolume, 0.0f, maxSliderValue);
		// End the Groups and Area	
		GUILayout.EndArea ();
		
        Event e = Event.current;
        if (e.isKey)
            Debug.Log("Detected key code: " + e.keyCode);		
	}
	
	// This function is called when the object
	// becomes enabled and active.
	void OnEnable ()
	{
		
	}
	
	// This function is called when the behaviour
	// becomes disabled () or inactive.
	void OnDisable ()
	{
		
	}
	
	// Reset to default values.
	void Reset ()
	{
		
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
	private void OnAudioFilterRead (float[] data, int channels)
	{

		//This uses the Unity specific float method we added to get the buffer
		midiStreamSynthesizer.GetNext (sampleBuffer);
		
		for (int i = 0; i < data.Length; i++) {
			//data [i] = sampleBuffer [i] * gain;
		}
	}

	public void MidiNoteOnHandler (int channel, int note, int velocity)
	{
		//print(note);
		//Debug.Log ("NoteOn: " + note.ToString () + " Velocity: " + velocity.ToString ());
		for (int i = 0; i < notesInMidi.Length; i++) {
			//if (notesInMidi[i] == note + 12) {
			if (notesInMidi[i] == note) {
				//print(midiInString[i]);
				playNoteIndex = i;
				playNote[i] = true;
			}
		}
	}
	
	public void MidiNoteOffHandler (int channel, int note)
	{
		//Debug.Log ("NoteOff: " + note.ToString ());
		for (int i = 0; i < notesInMidi.Length; i++) {
			//if (notesInMidi[i] == note + 12) {
			if (notesInMidi[i] == note) {
				//print(midiInString[i]);
				stopNoteIndex = i;
				stopNote[i] = true;
			}
		}
	}

	public void OnNote (int note) {
		midiStreamSynthesizer.NoteOn(1, note, midiNoteVolume, midiInstrument);
		MidiNoteOnHandler(1, note, midiNoteVolume);
		//midiSamples[playNoteIndex].Play();
		
		for (int i = 0; i < notesInMidi.Length; i++) {
			if (notesInMidi[i] == note) {
				midiSamples[i].Play();
			}
		}
		
		StartCoroutine(StopNote(note));
	}

	IEnumerator StopNote (int note) {
		yield return new WaitForSeconds(0.5f);
		midiStreamSynthesizer.NoteOff(1, note);
		MidiNoteOffHandler(1, note);
		//midiSamples[stopNoteIndex].Stop();
		
		for (int i = 0; i < notesInMidi.Length; i++) {
			if (notesInMidi[i] == note) {
				midiSamples[i].Stop();
			}
		}
		
	}
}
