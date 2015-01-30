using UnityEngine;
using System.Collections;

/*
 * This is base Ability class, to create custom ability simply override this class.
 * This class will be utilized by Inventory System or Ability system or Actor class which hold all it's ability scripts and will invoke the ability.
 * 
 * If you overriding Unity function from this class, don't forget to call Base[FunctionToOverride] before writing your scripts.
 * Remember base.[FunctionToOverride] won't work due to unity reflection architectures.
 */
public enum AbilityType{
	Offensive,
	Defensive,
	Special
}

public enum AbilityState{
	Idle,
	Cast,
	Cooldown
}

[AddComponentMenu("Ability/AbilityBase")]
public class AbilityBase : MonoBehaviour {
	// About the ability, used in UI
	public Sprite icon;
	public string abilityName;
	public AbilityType abilityType;
	public string description;

	// Required weapons, use bitwise format
	public int requiredWeapons;
	// % of damage ratio
	public float damageRatio;
	// Time needed during cast
	public float castTime;
	// Time needed for cooling down
	public float cooldownTime;
	// Animation for this ability, TODO: Adjust with actor animator
	public string anim;
	// Prefabs for effects
	public GameObject effect;
	
	PlayerBase actor = null;
	int abilityIndex;
	AbilityState state = AbilityState.Idle;
	// Timer for use internally
	float timer = 0f;
	
	void Update(){
		BaseUpdate ();
	}
	
	protected void BaseUpdate(){
		timer -= Time.deltaTime;

		switch (state){
		case AbilityState.Idle:
			actor.inGameCanvas.abilities [abilityIndex].icon.color = new Color (1f, 1f, 1f);
			actor.inGameCanvas.abilities [abilityIndex].timer.text = "";
			break;
		case AbilityState.Cast:
			OnCast();
			if (timer < 0){
				state = AbilityState.Cooldown;
				timer = cooldownTime;
				actor.inGameCanvas.abilities [abilityIndex].outline.enabled = false;
				actor.inGameCanvas.abilities [abilityIndex].icon.color = new Color (0.4f, 0.4f, 0.4f);
				OnCastEnd();
			}
			break;
		case AbilityState.Cooldown:
			if (timer < 0){
				state = AbilityState.Idle;
			}
			break;
		}

		// Update UI Timer
		if (timer >= 0){
			actor.inGameCanvas.abilities [abilityIndex].timer.text = timer > 60 ? Mathf.CeilToInt(timer/60) + "m" : Mathf.CeilToInt(timer) + "s";
		}
	}
	
	public void Cast(){
		// Check if ability is usable. TODO: replace 10 with weapon flags
//		if (!IsUsable(10)) return;

		if (state != AbilityState.Idle)
			return;

		state = AbilityState.Cast;
		actor.inGameCanvas.abilities [abilityIndex].outline.enabled = true;
		timer = castTime;

		// Begin Casting
		OnCastBegin ();
	}

	public void OnEquipBegin(PlayerBase a, int i){
		actor = a;
		abilityIndex = i;
		actor.inGameCanvas.abilities [abilityIndex].ability = this;
	}

	public void OnEquipEnd(){
		actor.inGameCanvas.abilities [abilityIndex].ability = null;
		actor = null;
		abilityIndex = 0;
	}

	public bool IsUsable(int weaponBits){
		return ((weaponBits & requiredWeapons) == requiredWeapons);
	}

	// Logic for casting : Override below function
	public virtual void OnCastBegin(){}
	public virtual void OnCast(){}
	public virtual void OnCastEnd(){}
}
