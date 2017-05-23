using UnityEngine;
using System.Collections;
using SocketIO;

public class AbilityReciever : MonoBehaviour {

	private SocketIOComponent socket;

    public PrefabStore knightAbilities;
    public PrefabStore lordSideCharacters;

    // Special Effect Prefabs
    public GameObject HealEffect;
	
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
		int characterID = (int)e.data.GetField("i").f;
		int abilityID = (int)e.data.GetField("a").f;
        int entityID = 0;

        foreach (CharacterData character in CharacterManager.instance.characterData)
        {
            if (character.CharacterID == characterID)
            {
                entityID = character.CharacterEntity;
            }
        }

        // TODO: Spawn ability effects on the character

        // Play Animation
        foreach (CharacterData character in CharacterManager.instance.characterData)
        {
            if (character.CharacterID == characterID)
            {
                // Get animation name
                string animationName = "";
                if (character.CharacterEntity == 0 || character.CharacterEntity == 1)
                {
                    // Animation names for knights originate from the ability
                    foreach (GameObject abilityObject in knightAbilities.prefabs)
                    {
                        if (abilityObject.GetComponent<Ability>().itemID == abilityID)
                        {
                            animationName = abilityObject.GetComponent<Ability>().anim;
                        }
                    }
                }
                else
                {
                    // Animation names on lord-side characters indexed starting at 0 for each increment of abilityID
                    foreach (GameObject lordCharacter in lordSideCharacters.prefabs)
                    {
                        if (lordSideCharacters.GetComponent<CharacterData>().CharacterEntity == entityID)
                        {
                            animationName = lordCharacter.GetComponent<CharacterData>().AnimationNames[abilityID];
                        }
                    }
                }
                character.GetComponent<Animator>().CrossFade(animationName, 0.2f);
            }
        }
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
        public string Heal = "H";
        public string Regeneration = "R";
		public string Bleed = "BL";
		public string Stun = "ST";
        public string Flying = "FLY";
        public string Invisibility = "I";
        public string Poison = "P";
        public string Purge = "PU";
        public string Stagger = "ST";
        public string HalfStagger = "H";
        public string Burn = "BU";
        public string Acid = "A";
        public string Freeze = "F";
        public string WrathOfFireMiasma = "W";
	}
    public EffectTypes effectTypes = new EffectTypes();
	
	void Effect(SocketIOEvent e) {
		string[] positiveEffects = new string[5] {effectTypes.Purge, effectTypes.Flying, effectTypes.Invisibility, effectTypes.Heal, effectTypes.Regeneration};
		string[] negativeEffects = new string[9] {effectTypes.Bleed, effectTypes.Stun, effectTypes.Poison, effectTypes.Stagger, effectTypes.HalfStagger, effectTypes.Burn, effectTypes.Acid,
                                                  effectTypes.Freeze, effectTypes.WrathOfFireMiasma};
		int characterID = (int)e.data.GetField("c").f;
		string effectType = e.data.GetField("e").str;
        // TODO: Implement effects so that it's visible to the player

        // Find character affected
        GameObject characterObject = null;
        foreach (CharacterData characterData in CharacterManager.instance.characterData)
        {
            if (characterData.CharacterID == characterID)
            {
                characterObject = characterData.gameObject;
            }
        }
        Vector3 halfHeight = characterObject.GetComponentInChildren<CapsuleCollider>().bounds.center;

        if (effectType == effectTypes.Heal)
        {
            // Create effect on the character
            GameObject instantiatedObject = Instantiate(HealEffect, halfHeight, Quaternion.identity);
            instantiatedObject.transform.SetParent(characterObject.transform);
        }
        else if (effectType == effectTypes.Regeneration)
        {

        }
        else if (effectType == effectTypes.Bleed)
        {

        }
        else if (effectType == effectTypes.Stun)
        {

        }
        else if (effectType == effectTypes.Flying)
        {

        }
        else if (effectType == effectTypes.Invisibility)
        {

        }
        else if (effectType == effectTypes.Poison)
        {

        }
        else if (effectType == effectTypes.Purge)
        {

        }
        else if (effectType == effectTypes.Stagger)
        {

        }
        else if (effectType == effectTypes.HalfStagger)
        {

        }
        else if (effectType == effectTypes.Burn)
        {

        }
        else if (effectType == effectTypes.Acid)
        {

        }
        else if (effectType == effectTypes.Freeze)
        {

        }
        else if (effectType == effectTypes.WrathOfFireMiasma)
        {

        }
    }
}
