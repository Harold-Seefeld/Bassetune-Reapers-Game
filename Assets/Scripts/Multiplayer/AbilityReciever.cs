using UnityEngine;
using System.Collections;
using SocketIO;

public class AbilityReciever : MonoBehaviour {

	private SocketIOComponent socket;
	[SerializeField] private CharacterManager characterManager;
	
	// Use this for initialization
	void Start () {
		// Get socket object
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On(SocketIOEvents.Input.KnightIO.ABILITY_START, StartKnightAbility);	
		socket.On(SocketIOEvents.Input.KnightIO.ABILITY_END, EndKnightAbility);	
		socket.On(SocketIOEvents.Input.BossIO.ABILITY_START, StartBossAbility);	
		socket.On(SocketIOEvents.Input.BossIO.ABILITY_END, EndBossAbility);	
	}

	void StartKnightAbility (SocketIOEvent e) {
		float characterID = e.data.GetField("i").f;
		float abilityID = e.data.GetField("a").f;
		// TODO: Spawn ability effects on the character
	}

	void EndKnightAbility (SocketIOEvent e) {
		float characterID = e.data.GetField("i").f;
		float abilityID = e.data.GetField("a").f;
		// TODO: Spawn ability effects on the character
	}

	void StartBossAbility (SocketIOEvent e) {
		float characterID = e.data.GetField("i").f;
		float abilityID = e.data.GetField("a").f;
		// TODO: Spawn ability effects on the character
	}
	
	void EndBossAbility (SocketIOEvent e) {
		float characterID = e.data.GetField("i").f;
		float abilityID = e.data.GetField("a").f;
		// TODO: Spawn ability effects on the character
	}
}
