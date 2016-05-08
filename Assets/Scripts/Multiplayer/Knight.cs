using UnityEngine;
using System.Collections;
using SocketIO;

public class Knight : MonoBehaviour {

	private SocketIOComponent socket;
	private CharacterManager characterManager;
	
	// Use this for initialization
	void Start () {
		// Get socket object
		socket = FindObjectOfType<SocketIOComponent>();
        characterManager = FindObjectOfType<CharacterManager>();
	}

	public void UseAbility (int abilityID, Vector2 target, int characterID, int weaponID) {
		JSONObject abilityUsage = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
		directionData.AddField("x", target.x);
		directionData.AddField("y", target.y);
		abilityUsage.AddField("target", directionData);
		abilityUsage.AddField("characterID", characterID);
		abilityUsage.AddField("abilityID", abilityID);
		abilityUsage.AddField("weapon", weaponID);
		socket.Emit(SocketIOEvents.Output.Knight.ABILITY_START, abilityUsage);
	}

	public void UseItem (int itemID, int characterID, Vector2 target) {
		JSONObject itemUsage = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
		directionData.AddField("x", target.x);
		directionData.AddField("y", target.y);
		itemUsage.AddField("characterID", characterID);
		itemUsage.AddField("itemID", itemID);
		socket.Emit(SocketIOEvents.Output.Knight.USE_ITEM, itemUsage);
	}

    public void UseItem(int itemID)
    {
        JSONObject itemUsage = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
        directionData.AddField("x", 0);
        directionData.AddField("y", 0);
        itemUsage.AddField("characterID", 0);
        itemUsage.AddField("itemID", itemID);
        socket.Emit(SocketIOEvents.Output.Knight.USE_ITEM, itemUsage);
    }

}
