using UnityEngine;
using System;
using System.Text;
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
	//#define DEBUG_WRAPPER
	#if DEBUG_WRAPPER
		public string WRAPPER_NAME = "rtmidi_wrapper_debug"
	#else
		public const string WRAPPER_NAME  = "rtmidi_wrapper";
	#endif
	
	
	
	//Environment
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void setupEnv();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	//Input Setup
	public static extern  int createInput();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void cleanupInputEnv();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  int destroyInput();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  int openInputPort(int port=0);
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void closeInputPort();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  int isInputPortOpen();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  uint getInPortCount();
	//[DllImport(MidiWrapper.WRAPPER_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	//public static extern  void getInputPortName(IntPtr name, uint port = 0);
	//public static extern  void getInputPortName(StringBuilder name, uint port = 0);

	//[DllImport(MidiWrapper.WRAPPER_NAME, CallingConvention = CallingConvention.Cdecl)]
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void getInputPortName(StringBuilder name, uint port = 0);


	//wrapper for getInputPortName
	public string getInputName(uint port = 0)
	{
		//IntPtr strPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 512);
		//getInputPortName(strPtr);
		//return Marshal.PtrToStringAnsi(strPtr);
		StringBuilder sb = new StringBuilder(512);
		//[In, Out] getInputPortName(sb);
		getInputPortName(sb, port);
		return sb.ToString();
	}
	//Output Setup
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  int createOutput();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  int destroyOutput();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  int openOutputPort(int port = 0);
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void closeOutputPort();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  int isOutputPortOpen();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  uint getOutPortCount();
	//[DllImport(MidiWrapper.WRAPPER_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	//public static extern  void getOutputPortName(IntPtr name, uint port = 0);
	//[DllImport(MidiWrapper.WRAPPER_NAME, CallingConvention = CallingConvention.Cdecl)]
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void getOutputPortName(StringBuilder name, uint port = 0);
	//wrapper for getOutputPortName
	public string getOutputName(uint port = 0)
	{
		//IntPtr strPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 512);
		//getOutputPortName(strPtr);
		//return Marshal.PtrToStringAnsi(strPtr);
		StringBuilder sb = new StringBuilder(512);
		//[In, Out] getOutputPortName(sb);
		getOutputPortName(sb, port);
		return sb.ToString();
	}
	//MIDI Input
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern long getNextMessageAsLong();
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern uint getNextMessageAsUInt();
	//
	
	[DllImport(MidiWrapper.WRAPPER_NAME, CallingConvention = CallingConvention.Cdecl)]
	public static extern MidiNoteMessage getNextMessageStruct();
	
	//Does NOT work correctly ... don't know why ...
	//[DllImport(MidiWrapper.WRAPPER_NAME, CallingConvention = CallingConvention.Cdecl)]
	//public static extern void fillWithNextNoteMessage(MidiNoteMessage message);
	
	//MIDI Output
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void noteOn(byte id, byte velocity, int channel);
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void noteOff(byte  id, int channel);
	//MIDI general output .. try with noteON and noteOff
	[DllImport(MidiWrapper.WRAPPER_NAME)]
	public static extern  void sendLimitedMessage(uint data, int nBytes, int channel = 0);

	
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
			
			for (uint i =0 ; i < nins; i++){
				Debug.Log("in name["+i+"] = " + getInputName(i));
			}
			for (uint i =0 ; i < nouts; i++){
				Debug.Log("out name["+i+"] = " + getOutputName(i));
			}
		}catch{
			Debug.LogError("Error getting port names");
		}
	}
	
	public void openPorts(int port=0){
		try{
			openInputPort(0);
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
	
	//public void getMessage(){
	//	MidiNoteMessage mm = new MidiNoteMessage();
	//	//[In, Out] fillWithNextNoteMessage(mm);
	//	fillWithNextNoteMessage(mm);
	//	Debug.Log("Message code: " + mm.code + " id: "+mm.id +" vel: " + mm.velocity + " timestamp: " + mm.timestamp);
	//}

	public void getMessageStruct(){
		//[In, Out] MidiNoteMessage mm = getNextMessageStruct();
		MidiNoteMessage mm = getNextMessageStruct();
		Debug.Log("STRUCT Message code: " + mm.code + " id: "+mm.id +" vel: " + mm.velocity + " timestamp: " + mm.timestamp);
	}

	public void getMessageAsLong(){
		long msg = getNextMessageAsLong();
		Debug.Log("message as long = "+String.Format("#{0:X}", msg));
	}
	public void getMessageAsUInt(){
		uint msg = getNextMessageAsUInt();
		Debug.Log("message as uint = "+String.Format("#{0:X}", msg));
	}
	
	public void activateNote(){//(byte note){
		noteOn(60,100,0);
		//noteOn(note,100);
	}
	
	//public void deactivateNote(byte note){
	public void deactivateNote(){
		noteOff(60,0);
	}
	
	
	public void activateNoteGeneric(){//(byte note){
		uint data = 0x00000000;
		data = 100 << 8 *2 | 60 << 8 *1 | 144 ;
		sendLimitedMessage(data, 3);
		//noteOn(note,100);
	}
	
	//public void deactivateNote(byte note){
	public void deactivateNoteGeneric(){
		uint data = 0x00000000;
		data = 0 << 8 *2 | 60 << 8 *1 | 128 ;
		sendLimitedMessage(data, 3);
	}
	
	void OnDestroy(){
		/*var din = destroyInput();
		Debug.Log("closed midi input = " + din);
		var dout = destroyOutput();
		Debug.Log("closed midi output = " + dout);
		*/
	}
}
