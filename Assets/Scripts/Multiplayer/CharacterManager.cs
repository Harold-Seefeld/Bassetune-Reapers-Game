using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;

public class CharacterManager : MonoBehaviour {

    public SocketIOComponent socket;
	public List<CharacterData> characterData;

    private JSONObject locationsToSend = new JSONObject(JSONObject.Type.OBJECT);

    // Use this for initialization
    void Start () {
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		// Listen out for new character creations
		socket.On(SocketIOEvents.Input.CHAR_CREATED, CreateCharacter);
		socket.On(SocketIOEvents.Input.HP, UpdateHP);
        socket = go.GetComponent<SocketIOComponent>();
        socket.On(SocketIOEvents.move, UpdateLocations);
    }

    void Update ()
    {
        // Return if no locations to send
        if (locationsToSend.Count == 0)
        {
            return;
        }
        socket.Emit(SocketIOEvents.move, locationsToSend);
        // Empty locations to send array
        locationsToSend = new JSONObject(JSONObject.Type.OBJECT);
    }

	void CreateCharacter(SocketIOEvent e) {
		// TODO: Create character with given data (assign meshes, etc use a prefab)
		GameObject newCharacter = new GameObject();
		CharacterData newCharacterData = newCharacter.AddComponent<CharacterData>();
		newCharacterData.CharacterEntity = Convert.ToInt16(e.data.GetField("Entity").n);
		newCharacterData.CharacterHP = Convert.ToInt16(e.data.GetField("HP").n);
		newCharacterData.CharacterID = Convert.ToInt16(e.data.GetField("ID").n);
		newCharacterData.CharacterOwner = e.data.GetField("Owner").str;
		// Add character data to the list
		characterData.Add(newCharacterData);
	}

	void UpdateHP(SocketIOEvent e) {
		for (int n = 0; n < e.data.Count; n++) {
			for (int i = 0; i < characterData.Count; i++) {
				if (e.data[n].GetField("i").str == characterData[i].CharacterID.ToString()) {
					characterData[i].CharacterHP = Convert.ToInt16(e.data[n].GetField("h").n);
					i = characterData.Count;
				}
			}
		}
	}

    public void UpdateLocations(SocketIOEvent e)
    {
        // Update locations based on the recieved json
        JSONObject data = e.data.GetField("d");
        for (int i = 0; i < data.Count; i++)
        {
            for (int n = 0; n < characterData.Count; n++)
            {
                if (characterData[n].CharacterID.ToString() == data[i].GetField("i").str)
                {
                    // TODO: Assign locations to character movement agents for smoothness
                    characterData[n].gameObject.transform.position = new Vector3(data[i].GetField("l")[0].n,
                                                                                                  0f, data[i].GetField("l")[1].n);
                }
            }
        }
        // Example: {"d":[{"i":"1","l":[0,525.3192]},{"i":"2","l":[0,525.3192]}]}
    }

    public void AddLocation(string characterID, Vector2 location)
    {
        JSONObject locationData = new JSONObject(JSONObject.Type.ARRAY);
        locationData.Add(location.x);
        locationData.Add(location.y);
        locationsToSend.AddField(characterID, locationData);
    }

}
