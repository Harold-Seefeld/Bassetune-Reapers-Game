using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;

public class CharacterManager : MonoBehaviour {

	private SocketIOComponent socket;
	public List<CharacterData> characterData;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		// Listen out for new character creations
		socket.On(SocketIOEvents.Output.BossIO.SPAWN_CREATURE, CreateCharacter);
		socket.On(SocketIOEvents.Input.HP, UpdateHP);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CreateCharacter(SocketIOEvent e) {
		// TODO: Create character with given data (assign meshes, etc use a prefab)
		GameObject newCharacter = new GameObject();
		CharacterData newCharacterData = newCharacter.AddComponent<CharacterData>();
		newCharacterData.CharacterEntitity = Convert.ToInt16(e.data.GetField("Entity"));
		newCharacterData.CharacterHP = Convert.ToInt16(e.data.GetField("HP").ToString());
		newCharacterData.CharacterID = Convert.ToInt16(e.data.GetField("ID").ToString());
		newCharacterData.CharacterOwner = e.data.GetField("Owner").ToString();
		newCharacterData.CharacterType = e.data.GetField("Type").ToString();
		// Add character data to the list
		characterData.Add(newCharacterData);
	}

	void UpdateHP(SocketIOEvent e) {
		for (int i = 0; i < characterData.Count; i++) {
			if (e.data.GetField("i") == characterData[i].CharacterID) {
				characterData[i].CharacterHP = e.data.GetField("h");
			}
		}
	}
}
