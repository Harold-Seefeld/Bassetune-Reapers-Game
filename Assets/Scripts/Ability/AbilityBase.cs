using UnityEngine;
using System.Collections;

/*
 * This is base Ability class, to create custom ability simply override this class.
 * This class will be utilized by Inventory System or Ability system or Actor class which hold all it's ability scripts and will invoke the ability.
 * Logic of Ability can be custimized by overriding this three function :
 * - OnCastBegin() : This function will be called once when ability began to cast
 * - OnCast()      : This function is called everytime as long as attack time
 * - OnCastEnd()   : This function is called once when the abiility cast ended
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
	Prepare,
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
	public bool onMainMenu = false;

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

	public string buyPrice = "1000";
	public string sellPrice = "1000";
	
	protected PlayerBase actor = null;
	protected Transform target = null;
	protected Vector3 targetPos;
	int abilityIndex;
	AbilityState state = AbilityState.Idle;
	// Timer for use internally
	float timer = 0f;
	
	void Update(){
		if (!onMainMenu)
			BaseUpdate ();
	}
	
	protected void BaseUpdate(){
		timer -= Time.deltaTime;

		switch (state){
		case AbilityState.Idle:
			actor.inGameCanvas.abilities [abilityIndex].icon.color = new Color (1f, 1f, 1f);
			actor.inGameCanvas.abilities [abilityIndex].timer.text = "";
			break;
		case AbilityState.Prepare:

			if (!actor.agent.hasPath){
				state = AbilityState.Cast;
				actor.inGameCanvas.abilities [abilityIndex].outline.enabled = true;
				actor.inGameCanvas.abilities [abilityIndex].icon.color = new Color (1f, 1f, 1f);
				timer = castTime;
				
				// Begin Casting
				OnCastBegin ();
			}
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
	
	public void Cast(Transform _target, Vector3 _targetPos){
		// Check if ability is usable. TODO: replace 10 with weapon flags
//		if (!IsUsable(10)) return;

		if (state != AbilityState.Idle)
			return;

		target = _target;
		targetPos = _targetPos;
		actor.agent.SetDestination (target.position);
		state = AbilityState.Prepare;
	}

	public void CancelCast(){
		Debug.Log ("Abort Cast");
		if (state == AbilityState.Idle){
			return;
		} else if (state == AbilityState.Prepare){
			state = AbilityState.Idle;
		} else if (state == AbilityState.Cast){
			state = AbilityState.Cooldown;
			timer = cooldownTime;
			actor.inGameCanvas.abilities [abilityIndex].outline.enabled = false;
			actor.inGameCanvas.abilities [abilityIndex].icon.color = new Color (0.4f, 0.4f, 0.4f);
		}
		OnCastCancel ();
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
	protected virtual void OnCastBegin(){}
	protected virtual void OnCast(){}
	protected virtual void OnCastEnd(){}
	protected virtual void OnCastCancel(){}
}
