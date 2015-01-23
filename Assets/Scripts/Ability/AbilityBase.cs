using UnityEngine;
using System.Collections;

/*
 * This is base Ability class, to create custom ability simply override this class.
 * This class will be utilized by Inventory System or Ability system or Actor class which hold all it's ability scripts and will invoke the ability.
 * 
 * If you overriding Unity function from this class, don't forget to call Base[FunctionToOverride] before writing your scripts.
 * Remember base.[FunctionToOverride] won't work due to unity reflection architectures.
 */

[AddComponentMenu("Ability/AbilityBase")]
public class AbilityBase : MonoBehaviour {
	// About the ability, used in UI
	public Sprite icon;
	public string title;
	public string description;
	
	// Required weapons, use bitwise format
	public int requiredWeapons;
	// % of damage ratio
	public float damageRatio;
	// Time needed during attack
	public float attackTime;
	// Time needed for cooling down
	public float cooldownTime;
	// Animation for this ability, TODO: Adjust with actor animator
	public string anim;
	// Prefabs for effects
	public GameObject effect;
	
	// Timer for use internally
	float timer;
	
	//slot of the inventory assigned to the ability
	public int slotAssigned;
	
	void Start(){
		BaseStart ();
	}
	
	protected void BaseStart(){
		//currently it only registers the ability to the knights since we still dont have the boss script.
		AddAbilityToIndex(gameObject.GetComponent<Knight>(), slotAssigned);
	}
	
	public void AddAbilityToIndex(Knight k, int index)
	{
		k.abilities[index] = this;
	}
	
	void Update(){
		BaseUpdate ();
	}
	
	protected void BaseUpdate(){
		timer += Time.deltaTime;
		
		// TODO: Do Update according attack and cooldown time
	}
	
	public virtual void Invoke(PlayerBase p){
		// Check if ability is usable. TODO: replace 10 with weapon flags
		if (!IsUsable(10)) return;
		
		// Run animation
		// p.animation.Play (anim);
		
		// Instantiate effect
		// Instantiate (effect, p.transform.position, p.transform.rotation);
	}
	
	public bool IsUsable(int weaponBits){
		return ((weaponBits & requiredWeapons) == requiredWeapons);
	}
}
