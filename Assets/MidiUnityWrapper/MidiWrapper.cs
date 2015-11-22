using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;



public class MidiWrapper : MonoBehaviour {
	
	[StructLayout(LayoutKind.Sequential)]
	public struct MidiNoteMessage{
		public int code;
		public int id;
		public int velocity;
		public double timestamp;
	}
	////////////////TESTS
	[DllImport("rtmidi_wrapper")]
	public static extern void note60Off(); 
	[DllImport("rtmidi_wrapper")]
	public static extern void note60On();
	[DllImport("rtmidi_wrapper")]
	public static extern long getNextMessageAsLong();
	///////////
	
	//Environment
	[DllImport("rtmidi_wrapper")]
	public static extern  void setupEnv();
	[DllImport("rtmidi_wrapper")]
	//Input Setup
	public static extern  int createInput();
	[DllImport("rtmidi_wrapper")]
	public static extern  void cleanupInputEnv();
	[DllImport("rtmidi_wrapper")]
	public static extern  int destroyInput();
	[DllImport("rtmidi_wrapper")]
	public static extern  int openInputPort(int port=0);
	[DllImport("rtmidi_wrapper")]
	public static extern  void closeInputPort();
	[DllImport("rtmidi_wrapper")]
	public static extern  int isInputPortOpen();
	[DllImport("rtmidi_wrapper")]
	public static extern  uint getInPortCount();
	[DllImport("rtmidi_wrapper", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	public static extern  void getInputPortName(IntPtr name, uint port = 0);

	//wrapper for getInputPortName
	public string getInputName(int port = 0)
	{
		IntPtr strPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 512);
		//strPtr = (IntPtr)"                                                                                   ";
		getInputPortName(strPtr);
		return Marshal.PtrToStringAnsi(strPtr);
	}
	//Output Setup
	[DllImport("rtmidi_wrapper")]
	public static extern  int createOutput();
	[DllImport("rtmidi_wrapper")]
	public static extern  int destroyOutput();
	[DllImport("rtmidi_wrapper")]
	public static extern  int openOutputPort(int port = 0);
	[DllImport("rtmidi_wrapper")]
	public static extern  void closeOutputPort();
	[DllImport("rtmidi_wrapper")]
	public static extern  int isOutputPortOpen();
	[DllImport("rtmidi_wrapper")]
	public static extern  uint getOutPortCount();
	[DllImport("rtmidi_wrapper", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	public static extern  void getOutputPortName(IntPtr name, uint port = 0);
	//wrapper for getOutputPortName
	public string getOutputName(int port = 0)
	{
		IntPtr strPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 512);
		//strPtr = "                                                                                   ";
		getOutputPortName(strPtr);
		return Marshal.PtrToStringAnsi(strPtr);
	}
	//MIDI Input
	[DllImport("rtmidi_wrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern void fillWithNextNoteMessage(MidiNoteMessage message);
	//MIDI Output
	[DllImport("rtmidi_wrapper")]
	public static extern  void noteOn(byte id, byte velocity, int channel);
	[DllImport("rtmidi_wrapper")]
	public static extern  void noteOff(byte  id, int channel);

	
	//void Start () {
	public void createIO(){
		Debug.Log(System.Environment.Version);
		try{
			Debug.Log("Setting up environment ");
			setupEnv();
			var inConnection = createInput();
			Debug.Log("input created = " + inConnection);
			var outConnection = createOutput();
			Debug.Log("output created = " + outConnection);
			
		}catch{
		
		}
	
	}
	public void deleteIO(){
		Debug.Log("deleting IO");
		var din = destroyInput();
		Debug.Log("del input = " +din);
		var dout = destroyOutput();
		Debug.Log("del out = " +dout);
	}
	// Update is called once per frame
	void Update () {
		
	}
	
	public void getPortCount(){
		try{	
			var nins = getInPortCount();
			Debug.Log("input ports = " + nins);
			var nouts = getOutPortCount();
			Debug.Log(" output ports = "+nouts);
		}catch{
			Debug.LogError("Error counting ports");
		}
		
	}
	
	public void getPortsNames(){
		try{
			var nins = getInPortCount();
			Debug.Log("input ports = " + nins);
			var nouts = getOutPortCount();
			Debug.Log(" output ports = "+nouts);
			
			for (int i =0 ; i < nins; i++){
				Debug.Log("in name["+i+"] = " + getInputName(i));
			}
			for (int i =0 ; i < nouts; i++){
				Debug.Log("out name["+i+"] = " + getOutputName(i));
			}
		}catch{
			Debug.LogError("Error getting port names");
		}
	}
	
	public void openPorts(int port=0){
		try{
			openInputPort(port);
			openOutputPort(1);
		}catch{
			Debug.LogError("Error opening ports");
		}
	}
	
	public void checkPortsOpen(){
		try{
			var io = isInputPortOpen();
			Debug.Log(" input port open = "+io);
			var oo = isOutputPortOpen();
			Debug.Log(" output port open = "+oo);
		}catch{
			Debug.LogError("Error opening ports");
		}
	}
	
	public void getMessage(){
		MidiNoteMessage mm = new MidiNoteMessage();
		fillWithNextNoteMessage(mm);
		Debug.Log("Message code: " + mm.code + " id: "+mm.id +" vel: " + mm.velocity + " timestamp: " + mm.timestamp);
	}
	
	public void getMessageAsLong(){
		long msg = getNextMessageAsLong();
		Debug.Log("message as long = "+String.Format("#{0:X}", msg));
	}
	
	public void activateNote(){//(byte note){
		noteOn(60,100,0);
		//noteOn(note,100);
	}
	
	//public void deactivateNote(byte note){
	public void deactivateNote(){
		noteOff(60,0);
	}
	
	void OnDestroy(){
		/*var din = destroyInput();
		Debug.Log("closed midi input = " + din);
		var dout = destroyOutput();
		Debug.Log("closed midi output = " + dout);
		*/
	}
}
