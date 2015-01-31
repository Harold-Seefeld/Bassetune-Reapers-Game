using UnityEngine;
using System.Collections;

[AddComponentMenu("Actor/Knight")]
public class Knight : PlayerBase {
	public AbilityBase[] abilities;

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
	
	protected override void OnCastHotkey(bool smartcast, Transform target, Vector3 position){
		//invokes the ability when the button is pressed
		if (Input.GetAxis("Attack1") > 0)
		{
            if (abilities.Length > 0 && abilities[0])
				abilities[0].Cast();

			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 1";
			#endif
		}
		else if (Input.GetAxis("Attack2") > 0)
		{
			if (abilities.Length > 1 && abilities[1])
				abilities[1].Cast();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 2";
			#endif
		}
		else if (Input.GetAxis("Attack3") > 0)
		{
			if (abilities.Length > 2 && abilities[2])
				abilities[2].Cast();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 3";
			#endif
		}
		else if (Input.GetAxis("Attack4") > 0)
		{
			if (abilities.Length > 3 && abilities[3])
				abilities[3].Cast();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 4";
			#endif
		}
		else if (Input.GetAxis("Attack5") > 0)
		{
			if (abilities.Length > 4 && abilities[4])
				abilities[4].Cast();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 5";
			#endif
		}
		else if (Input.GetAxis("Attack6") > 0)
		{
			if (abilities.Length > 5 && abilities[5])
				abilities[5].Cast();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 6";
			#endif
		}
		else if (Input.GetAxis("Attack7") > 0)
		{
			if (abilities.Length > 6 && abilities[6])
				abilities[6].Cast();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 7";
			#endif
		}
		else if (Input.GetAxis("Attack8") > 0)
		{
			if (abilities.Length > 7 && abilities[7])
				abilities[7].Cast();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 8";
			#endif
		}
	}
}
