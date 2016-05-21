using UnityEngine;
using System.Collections;

public class Ability : ItemBase {

	public enum AbilityType {
		Offensive,
		Defensive
	}
    public AbilityType abilityType;
	
	public enum AbilityState {
		Idle,
		Prepare,
		Cast,
		Cooldown
	}
    public AbilityState abilityState;

    // Required weapons for use
    public Weapon.WeaponType[] requiredWeapons;
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
	// Index
	public int abilityID;
	// Is the ability togglable?
	public bool isTogglable;
}
