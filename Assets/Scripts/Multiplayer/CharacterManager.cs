using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;

public class CharacterManager : MonoBehaviour {

	public SocketIOComponent socket;
	public List<CharacterData> characterData;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		// Listen out for new character creations
		socket.On(SocketIOEvents.Input.CHAR_CREATED, CreateCharacter);
		socket.On(SocketIOEvents.Input.HP, UpdateHP);
	}

	void CreateCharacter(SocketIOEvent e) {
		// TODO: Create character with given data (assign meshes, etc use a prefab)
		GameObject newCharacter = new GameObject();
		CharacterData newCharacterData = newCharacter.AddComponent<CharacterData>();
		newCharacterData.CharacterEntity = Convert.ToInt16(e.data.GetField("Entity").n);
		newCharacterData.CharacterHP = Convert.ToInt16(e.data.GetField("HP").n);
		newCharacterData.CharacterID = Convert.ToInt16(e.data.GetField("ID").n);
		newCharacterData.CharacterOwner = e.data.GetField("Owner").str;
		newCharacterData.CharacterOwnerNick = e.data.GetField("Nick").str;
		newCharacterData.CharacterType = e.data.GetField("Type").str;
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

}
