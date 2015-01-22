// by dmongs
using UnityEngine;
using System.Collections;

[AddComponentMenu("Actor/Knight")]
public class Knight : PlayerBase {
	Animator anim;

	// animation parmeters
	const string AniMoveSpeedName = "MoveSpeed";
	const string AniAttackedName = "Attacked";

    //by sonarsound---
    public AbilityBase[] abilities;
    //---

	void Start () {
		// Call base start function
		BaseStart ();

		// initialize references
		anim = GetComponent<Animator> ();
        abilities = new AbilityBase[8];
	}

	void Update(){
		// Call base update function
		BaseUpdate ();

		// Set animation movement variable
		anim.SetFloat (AniMoveSpeedName, agent.velocity.magnitude);
        //if any pressed ability hotkey
        onPressedAbilityHotkey();
	}

    void onPressedAbilityHotkey()
    {
        //by sonarsound
        //invokes the ability when the button is pressed
        if (Input.GetAxis("Attack1") > 0)
        {
            abilities[0].Invoke(gameObject.GetComponent<PlayerBase>());
            #if UNITY_EDITOR
                debugLabelText.text = "Use Ability 1";
            #else

            #endif
        }
        else if (Input.GetAxis("Attack2") > 0)
        {
            abilities[1].Invoke(gameObject.GetComponent<PlayerBase>());
            #if UNITY_EDITOR
                debugLabelText.text = "Use Ability 2";
            #else

            #endif
        }
        else if (Input.GetAxis("Attack3") > 0)
        {
            abilities[2].Invoke(gameObject.GetComponent<PlayerBase>());
            #if UNITY_EDITOR
                debugLabelText.text = "Use Ability 3";
            #else

            #endif
        }
        else if (Input.GetAxis("Attack4") > 0)
        {
            abilities[3].Invoke(gameObject.GetComponent<PlayerBase>());
            #if UNITY_EDITOR
                debugLabelText.text = "Use Ability 4";
            #else

            #endif
        }
        else if (Input.GetAxis("Attack5") > 0)
        {
            abilities[4].Invoke(gameObject.GetComponent<PlayerBase>());
            #if UNITY_EDITOR
                debugLabelText.text = "Use Ability 5";
            #else

            #endif
        }
        else if (Input.GetAxis("Attack6") > 0)
        {
            abilities[5].Invoke(gameObject.GetComponent<PlayerBase>());
            #if UNITY_EDITOR
                debugLabelText.text = "Use Ability 6";
            #else

            #endif
        }
        else if (Input.GetAxis("Attack7") > 0)
        {
            abilities[6].Invoke(gameObject.GetComponent<PlayerBase>());
            #if UNITY_EDITOR
                debugLabelText.text = "Use Ability 7";
            #else

            #endif
        }
        else if (Input.GetAxis("Attack8") > 0)
        {
            abilities[7].Invoke(gameObject.GetComponent<PlayerBase>());
            #if UNITY_EDITOR
                debugLabelText.text = "Use Ability 8";
            #else

            #endif
        }
    }
}
