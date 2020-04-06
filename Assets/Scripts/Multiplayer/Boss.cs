using UnityEngine;
using System.Collections;
using SocketIO;

public class Boss : MonoBehaviour {

	private SocketIOComponent socket;
	[SerializeField] private CharacterManager characterManager;
	
	// Use this for initialization
	void Start () {
		// Get socket object
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
	}
	
	public void UseAbility (Ability ability, Vector2 target, int characterID, int weaponID) {
		JSONObject abilityUsage = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
		directionData.AddField("x", target.x);
		directionData.AddField("y", target.y);
		abilityUsage.AddField("target", directionData);
		abilityUsage.AddField("characterID", characterID);
		abilityUsage.AddField("abilityID", ability.abilityID);
		socket.Emit(SocketIOEvents.Output.Boss.ABILITY_START, abilityUsage);
	}

	public void UseAbility (Ability ability, bool toggle, int characterID, int weaponID) {
		JSONObject abilityUsage = new JSONObject(JSONObject.Type.OBJECT);
		abilityUsage.AddField("toggle", toggle);
		abilityUsage.AddField("characterID", characterID);
		abilityUsage.AddField("abilityID", ability.abilityID);
		socket.Emit(SocketIOEvents.Output.Boss.ABILITY_START, abilityUsage);
	}
}
