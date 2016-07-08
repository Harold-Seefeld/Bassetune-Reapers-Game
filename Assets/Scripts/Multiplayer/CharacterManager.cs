using UnityEngine;
using System;
using SocketIO;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

    public SocketIOComponent socket;
	public List<CharacterData> characterData;

    public static CharacterManager instance;

    private JSONObject locationsToSend = new JSONObject(JSONObject.Type.OBJECT);
    public GameObject characterPrefab;

    // Use this for initialization
    void Start () {
        instance = this;

		socket = FindObjectOfType<SocketIOComponent>();
		// Listen out for new character creations
		socket.On(SocketIOEvents.Input.CHAR_CREATED, CreateCharacter);
		socket.On(SocketIOEvents.Input.HP, UpdateHP);
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

	void CreateCharacter(SocketIOEvent e)
    {
        // Check if character already exists
        for (var i = 0; i < characterData.Count; i++)
        {
            if (characterData[i].CharacterID == (int)e.data.GetField("I").n)
            {
                // Don't create a new character
                return;
            }
        }
        // TODO: Create character with given data (assign meshes, etc use a prefab)
        Vector3 location = new Vector3(e.data.GetField("L").GetField("x").n, 5, e.data.GetField("L").GetField("y").n);
        GameObject newCharacter = (GameObject)Instantiate(characterPrefab, location, Quaternion.identity);
        CharacterData newCharacterData = newCharacter.AddComponent<CharacterData>();
		newCharacterData.CharacterEntity = (int)e.data.GetField("E").n;
		newCharacterData.CharacterHP = (int)e.data.GetField("H").n;
		newCharacterData.CharacterID = (int)e.data.GetField("I").n;
		newCharacterData.CharacterOwner = (int)e.data.GetField("O").n;
		// Add character data to the list
		characterData.Add(newCharacterData);
        // Allow character to be selected
        newCharacter.AddComponent<UnityEngine.UI.Extensions.CharacterSelectable>();
	}

	void UpdateHP(SocketIOEvent e)
    {
		for (int n = 0; n < e.data.Count; n++)
        {
			for (int i = 0; i < characterData.Count; i++)
            {
				if (e.data[n].GetField("i").str == characterData[i].CharacterID.ToString())
                {
					characterData[i].CharacterHP = Convert.ToInt16(e.data[n].GetField("h").n);
					i = characterData.Count;
				}
			}
		}
	}

    public void UpdateLocations(SocketIOEvent e)
    {
        // Update locations based on the recieved json
        List<JSONObject> data = e.data.GetField("d").list;
        for (int i = 0; i < data.Count; i++)
        {
            int recievedCharacterID = (int)data[i].GetField("i").n;
            for (int n = 0; n < characterData.Count; n++)
            {
                if (characterData[n].CharacterID == recievedCharacterID)// && characterData[n].CharacterOwner != Server.instance.currentPlayerID)
                {
                    // TODO: Assign locations to character movement agents for smoothness
                    //characterData[n].gameObject.transform.position = new Vector3(data[i].GetField("l")[0].n,
                    //                                                                              0f, data[i].GetField("l")[1].n);
                    //StartCoroutine(characterData[n].gridPlayer.FindPath(characterData[n].gameObject.transform.position,
                    //    new Vector3(data[i].GetField("l").list[0].n, 5, data[i].GetField("l").list[1].n)));
                    //characterData[n].gridPlayer.currentDestination = new Vector3(data[i].GetField("l").list[0].n, 5, data[i].GetField("l").list[1].n);
                    characterData[n].gameObject.GetComponent<Rigidbody>().MovePosition(new Vector3(data[i].GetField("l").list[0].n, 3, data[i].GetField("l").list[1].n));
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
