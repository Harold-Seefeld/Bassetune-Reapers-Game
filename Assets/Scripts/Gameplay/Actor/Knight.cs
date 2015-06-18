using UnityEngine;
using System.Collections;

[AddComponentMenu("Actor/Knight")]
public class Knight : PlayerBase {
	public AbilityBase[] abilities;

	Animator anim;

	protected int currentAbility = -1;

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

        if (currentAbility != -1)
        {
            setTarget(ref target, ref targetPos);
        }
	}
	
	protected override void OnCastHotkey(Transform target, Vector3 position){
		//invokes the ability when the button is pressed
		if (Input.GetAxis("Attack1") > 0)
		{
            if (abilities.Length > 0 && abilities[0])
				abilities[0].Cast(target, position);

			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 1";
			#endif
		}
		else if (Input.GetAxis("Attack2") > 0)
		{
			if (abilities.Length > 1 && abilities[1])
				abilities[1].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 2";
			#endif
		}
		else if (Input.GetAxis("Attack3") > 0)
		{
			if (abilities.Length > 2 && abilities[2])
				abilities[2].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 3";
			#endif
		}
		else if (Input.GetAxis("Attack4") > 0)
		{
			if (abilities.Length > 3 && abilities[3])
				abilities[3].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 4";
			#endif
		}
		else if (Input.GetAxis("Attack5") > 0)
		{
			if (abilities.Length > 4 && abilities[4])
				abilities[4].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 5";
			#endif
		}
		else if (Input.GetAxis("Attack6") > 0)
		{
			if (abilities.Length > 5 && abilities[5])
				abilities[5].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 6";
			#endif
		}
		else if (Input.GetAxis("Attack7") > 0)
		{
			if (abilities.Length > 6 && abilities[6])
				abilities[6].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 7";
			#endif
		}
		else if (Input.GetAxis("Attack8") > 0)
		{
			if (abilities.Length > 7 && abilities[7])
				abilities[7].Cast(target, position);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 8";
			#endif
		}
	}

    protected override void OnCastLegacy()
    {
        settingTargetLegacy = true;
        if (Input.GetAxis("Attack1") > 0)
		{
            if (abilities.Length > 0 && abilities[0])
            {
                currentAbility = 0;
            }
		}
		else if (Input.GetAxis("Attack2") > 0)
		{
            if (abilities.Length > 1 && abilities[1])
            {
                currentAbility = 1;
            }
		}
		else if (Input.GetAxis("Attack3") > 0)
		{
            if (abilities.Length > 2 && abilities[2])
            {
                currentAbility = 2;
            }
		}
		else if (Input.GetAxis("Attack4") > 0)
		{
            if (abilities.Length > 3 && abilities[3])
            {
                currentAbility = 3;
            }
		}
		else if (Input.GetAxis("Attack5") > 0)
		{
            if (abilities.Length > 4 && abilities[4])
            {
                currentAbility = 4;
            }
		}
		else if (Input.GetAxis("Attack6") > 0)
		{
            if (abilities.Length > 5 && abilities[5])
            {
                currentAbility = 5;
            }
		}
		else if (Input.GetAxis("Attack7") > 0)
		{
            if (abilities.Length > 6 && abilities[6])
            {
                currentAbility = 6;
            }
		}
		else if (Input.GetAxis("Attack8") > 0)
		{
            if (abilities.Length > 7 && abilities[7])
            {
                currentAbility = 7;
            }
		}
	}

    protected override void setTarget(ref Transform target, ref Vector3 position)
    {
        #if UNITY_EDITOR
		debugLabelText.text = "Setting target for ability " + (currentAbility + 1) + " using legacy system";
		#endif
        if (settingTargetLegacy && currentAbility != -1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(ScreenToNavPos(Input.mousePosition, ref position, ref target) && target)
                {
                    abilities[currentAbility].Cast(target, position);
                    settingTargetLegacy = false;
                    #if UNITY_EDITOR
                    debugLabelText.text = "Used ability " + (currentAbility + 1) + " using legacy system";
		            #endif
                    currentAbility = -1;
                }
            }
            else if(Input.GetButtonDown("Cancel"))
            {
                settingTargetLegacy = false;
                #if UNITY_EDITOR
                debugLabelText.text = "Canceled targeting for ability " + (currentAbility + 1) + " using legacy system";
		        #endif
                currentAbility = -1;
            }
        }
    }
}
