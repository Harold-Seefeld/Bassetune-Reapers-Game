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
		socket.On(SocketIOEvents.Input.Knight.ABILITY_START, StartKnightAbility);	
		socket.On(SocketIOEvents.Input.Knight.ABILITY_END, EndKnightAbility);	
		socket.On(SocketIOEvents.Input.Boss.ABILITY_START, StartBossAbility);	
		socket.On(SocketIOEvents.Input.Boss.ABILITY_END, EndBossAbility);
		socket.On(SocketIOEvents.Input.EFFECT, Effect);
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

	// Also recieve effects in this class
	public class EffectTypes {
		public string IncreasedRange = "I_Range";
		public string DecreasedRange = "D_Range";
		public string Regeneration = "Regen";
		public string Bleed = "Bleed";
		public string Stun = "Stun";
	}
	public EffectTypes effectTypes;
	
	void Effect(SocketIOEvent e) {
		string[] positiveEffects = new string[2] {effectTypes.IncreasedRange, effectTypes.Regeneration};
		string[] negativeEffects = new string[3] {effectTypes.DecreasedRange, effectTypes.Bleed, effectTypes.Stun};
		float characterID = e.data.GetField("i").f;
		float abilityID = e.data.GetField("a").f;
		string type = e.data.GetField("t").str;
 		// TODO: Implement effects so that it's visible to the player
	}
}
