using UnityEngine;
using System.Collections;
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
		JSONObject data = e.data.GetField("d");
		for (int i = 0; i < data.Count; i++) {
			for (int n = 0; n < characterManager.characterData.Count; n++) {
				if (characterManager.characterData[n].CharacterID.ToString() == data[i].GetField("i").str) {
					// TODO: Assign locations to character movement agents for smoothness
					characterManager.characterData[n].gameObject.transform.position = new Vector3(data[i].GetField("l")[0].f, 
					                                                                              data[i].GetField("l")[1].f, 0f);
				}
			}
		}
		// Example: {"d":[{"i":"1","l":[0,525.3192]},{"i":"2","l":[0,525.3192]}]}
	}

	public void AddLocation(string characterID, Vector2 location) {
		JSONObject locationData = new JSONObject(JSONObject.Type.ARRAY);
		locationData.Add(location.x);
		locationData.Add(location.y);
		locationsToSend.AddField(characterID, locationData);
	}
}
