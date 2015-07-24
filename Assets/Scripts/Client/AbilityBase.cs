using UnityEngine;
using System.Collections;

[AddComponentMenu("Ability/AbilityBase")]
public class AbilityBase : MonoBehaviour {

	public enum AbilityType {
		Offensive,
		Defensive
	}
	
	public enum AbilityState {
		Idle,
		Prepare,
		Cast,
		Cooldown
	}

	// UI Variables
	public Sprite icon;
	public string abilityName;
	public AbilityType abilityType;
	public string description;
	// Prices for UI
	public string buyPrice;
	public string sellPrice;
	// Required weapons
	public WeaponBase.WeaponType[] requiredWeapons;
	// % of damage ratio
	public float damageRatio;
	// Time needed during cast
	public float castTime;
	// Time needed for cooling down
	public float cooldownTime;
	// Animation for this ability
	public string anim;
	// Prefabs for effects
	public GameObject startEffect;
	public GameObject endEffect;
	// Game state
	public AbilityState abilityState;
	// Index
	public int abilityID;
	// Is the ability togglable?
	public bool isTogglable;
}
