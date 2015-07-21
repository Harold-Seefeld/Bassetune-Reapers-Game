using UnityEngine;
using System.Collections;
using SocketIO;

public class Knight : MonoBehaviour {

	private SocketIOComponent socket;
	[SerializeField] private CharacterManager characterManager;
	
	// Use this for initialization
	void Start () {
		// Get socket object
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
	}

	public void UseAbility (AbilityBase ability, Vector2 direction, int characterID, int weaponID) {
		JSONObject abilityUsage = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
		directionData.AddField("x", direction.x);
		directionData.AddField("y", direction.y);
		abilityUsage.AddField("target", directionData);
		abilityUsage.AddField("characterID", characterID);
		abilityUsage.AddField("abilityID", ability.abilityID);
		abilityUsage.AddField("weapon", weaponID);
		socket.Emit(SocketIOEvents.Output.Knight.ABILITY_START, abilityUsage);
	}

	public void UseItem (ItemBase item, int characterID) {

	}
	
}
