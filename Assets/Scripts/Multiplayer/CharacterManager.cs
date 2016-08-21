using UnityEngine;
using System;
using SocketIO;
using System.Collections.Generic;
using System.Collections;

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
        StartCoroutine(CreateCharacter(Server.instance.currentPlayerID != 1, e));
	}

    IEnumerator CreateCharacter(bool playerIDSet, SocketIOEvent e)
    {
        // Delay
        if (!playerIDSet)
        {
            yield return new WaitForSeconds(1);
        }

        // Check if character already exists
        for (var i = 0; i < characterData.Count; i++)
        {
            if (characterData[i].CharacterID == (int)e.data.GetField("I").n)
            {
                // Don't create a new character
                yield break;
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
        newCharacterData.CharacterMaxHP = (int)e.data.GetField("M").n;
        // Add character data to the list
        characterData.Add(newCharacterData);
        // Allow character to be selected
        newCharacter.AddComponent<UnityEngine.UI.Extensions.CharacterSelectable>();

        // Set Default Character
        if (Server.instance.currentPlayerID == newCharacterData.CharacterOwner)
        {
            // For knights
            if (newCharacterData.CharacterEntity == 0 || newCharacterData.CharacterEntity == 1)
            {
                Server.instance.currentDefaultCharacter = newCharacterData;
                UseCaller.isKnight = true;
            }
            // For bosses
            if (newCharacterData.CharacterEntity >= 3000 || newCharacterData.CharacterEntity < 3200)
            {
                Server.instance.currentDefaultCharacter = newCharacterData;
                UseCaller.isKnight = false;
            }
        }
    }

	void UpdateHP(SocketIOEvent e)
    {
        var data = e.data.GetField("d").list;
		for (int n = 0; n < e.data.Count; n++)
        {
			for (int i = 0; i < characterData.Count; i++)
            {
				if ((int)data[n].GetField("i").n == characterData[i].CharacterID)
                {
					characterData[i].CharacterHP = Convert.ToInt16(data[n].GetField("h").n);
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
