using UnityEngine;
using System.Collections;

[AddComponentMenu("Actor/Knight")]
public class Knight : PlayerBase {
	Animator anim;

	// animation parmeters
	const string AniMoveSpeedName = "MoveSpeed";
	const string AniAttackedName = "Attacked";

	void Start () {
		// Call base start function
		BaseStart ();

		// initialize references
		anim = GetComponent<Animator> ();
		inGameCanvas = GameObject.Find ("Knight Canvas").GetComponent<InGameCanvas> ();
		
		for (int i = 0; i < abilities.Length; ++i){
			if (abilities[i]){
				abilities[i] = Instantiate(abilities[i]) as AbilityBase;
				abilities[i].OnEquipBegin(this, i);
			}
		}
	}
	
	void Update(){
		// Call base update function
		BaseUpdate ();
		
		// Set animation movement variable
		anim.SetFloat (AniMoveSpeedName, agent.velocity.magnitude);
	}
	
	protected override int OnCastHotkey(Transform target, Vector3 position){
		//invokes the ability when the button is pressed
		if (Input.GetButton("Attack1"))
		{
            if (abilities.Length > 0 && abilities[0])
				abilities[0].Cast(target, position);

			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 1";
			#endif
			return 0;
		}
		else if (Input.GetButton("Attack2"))
		{
			if (abilities.Length > 1 && abilities[1])
				abilities[1].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 2";
			#endif
			return 1;
		}
		else if (Input.GetButton("Attack3"))
		{
			if (abilities.Length > 2 && abilities[2])
				abilities[2].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 3";
			#endif
			return 2;
		}
		else if (Input.GetButton("Attack4"))
		{
			if (abilities.Length > 3 && abilities[3])
				abilities[3].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 4";
			#endif
			return 3;
		}
		else if (Input.GetButton("Attack5"))
		{
			if (abilities.Length > 4 && abilities[4])
				abilities[4].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 5";
			#endif
			return 4;
		}
		else if (Input.GetButton("Attack6"))
		{
			if (abilities.Length > 5 && abilities[5])
				abilities[5].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 6";
			#endif
			return 5;
		}
		else if (Input.GetButton("Attack7"))
		{
			if (abilities.Length > 6 && abilities[6])
				abilities[6].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 7";
			#endif
			return 6;
		}
		else if (Input.GetButton("Attack8"))
		{
			if (abilities.Length > 7 && abilities[7])
				abilities[7].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 8";
			#endif
			return 7;
		}

		// Not casting anything
		return -1;
	}
}
