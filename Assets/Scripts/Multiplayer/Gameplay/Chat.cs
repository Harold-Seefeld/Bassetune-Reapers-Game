using UnityEngine;
using System.Collections;
using SocketIO;

public class Chat : MonoBehaviour {

	private SocketIOComponent socket;
	[SerializeField] private CharacterManager characterManager;
	
	// Use this for initialization
	void Start () {
		// Get socket object
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On(SocketIOEvents.talk, UpdateChatbox);		
	}

	// Use this for initialization
	void UpdateChatbox (SocketIOEvent e) {
		string username = e.data.GetField("username").str;
		string msg = e.data.GetField("msg").str;
		// TODO: Make this work with a chatbox
	}

	void SendMessage (string message) {
		JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
		data.AddField("message", message);
		socket.Emit(SocketIOEvents.talk, data);
	}
}
