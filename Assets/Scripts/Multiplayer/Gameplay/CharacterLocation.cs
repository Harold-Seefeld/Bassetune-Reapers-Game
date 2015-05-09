using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;

public class CharacterLocation : MonoBehaviour {

	private SocketIOComponent socket;
	private JSONObject locationsToSend = new JSONObject(JSONObject.Type.OBJECT);
	[SerializeField] CharacterManager characterManager;

	// Use this for initialization
	void Start () {
		// Get socket object
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		// Movement listener
		socket.On(SocketIOEvents.move, UpdateLocations);
	}
	
	// Update is called once per frame
	void Update () {
		// Return if no locations to send
		if (locationsToSend.Count == 0) {
			return;
		}
		socket.Emit(SocketIOEvents.move, locationsToSend);
		// Empty locations to send array
		locationsToSend = new JSONObject(JSONObject.Type.OBJECT);
	}

	public void UpdateLocations (SocketIOEvent e) {
		// Update locations based on the recieved json
		for (int a = 0; a < e.data.Count; a++) {
			for (int n = 0; n < characterManager.characterData.Count; n++) {
				if (characterManager.characterData[n].CharacterID.ToString() == e.data[a].GetField("id").ToString()) {
					// TODO: Assign locations to character movement agents for smoothness
					characterManager.characterData[n].gameObject.transform.position = new Vector3(Convert.ToSingle(e.data[a].GetField("location").GetField("x")), 
					                                                                              Convert.ToSingle(e.data[a].GetField("location").GetField("y")), 0f);
				}
			}
		}
		// Example: ["m",[{"location":[0,6.025636]},{"location":[0,6.025636]}]]
	}

	public void AddLocation(string characterID, Vector2 location) {
		JSONObject locationData = new JSONObject(JSONObject.Type.ARRAY);
		locationData.Add(location.x);
		locationData.Add(location.y);
		locationsToSend.AddField(characterID, locationData);
	}
}
