using UnityEngine;
using System.Collections;

[AddComponentMenu("Actor/Knight")]
public class Knight : PlayerBase {
	public AbilityBase[] abilities;

	Animator anim;
	// animation parmeters
	const string AniMoveSpeedName = "MoveSpeed";
	const string AniAttackedName = "Attacked";

    //when this actor is using smart cast
    public bool isUsingSmartCast;
    //target actor of the current ability
    public GameObject targetActor;
    
    //the ability index currently selected
    public int currentAbility;

	void Start () {
		// Call base start function
		BaseStart ();

        currentAbility = -1;

		// initialize references
		anim = GetComponent<Animator> ();
		inGameCanvas = GameObject.Find ("Knight Canvas").GetComponent<InGameCanvas> ();
        targetActor = gameObject;

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
		//if any pressed ability hotkey
		OnPressedAbilityHotkey();
        if (Input.GetMouseButtonDown(0))
        {
            MousePressed();
        }
	}
	
	void OnPressedAbilityHotkey()
	{
		//invokes the ability when the button is pressed
		if (Input.GetAxis("Attack1") > 0)
		{
            if (abilities.Length > 0 && abilities[0])
                currentAbility = 0;

			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 1";
			#endif
		}
		else if (Input.GetAxis("Attack2") > 0)
		{
			if (abilities.Length > 1 && abilities[1])
                currentAbility = 1;
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 2";
			#endif
		}
		else if (Input.GetAxis("Attack3") > 0)
		{
			if (abilities.Length > 2 && abilities[2])
                currentAbility = 2;
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 3";
			#endif
		}
		else if (Input.GetAxis("Attack4") > 0)
		{
			if (abilities.Length > 3 && abilities[3])
                currentAbility = 3;
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 4";
			#endif
		}
		else if (Input.GetAxis("Attack5") > 0)
		{
			if (abilities.Length > 4 && abilities[4])
                currentAbility = 4;
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 5";
			#endif
		}
		else if (Input.GetAxis("Attack6") > 0)
		{
			if (abilities.Length > 5 && abilities[5])
                currentAbility = 5;
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 6";
			#endif
		}
		else if (Input.GetAxis("Attack7") > 0)
		{
			if (abilities.Length > 6 && abilities[6])
                currentAbility = 6;
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 7";
			#endif
		}
		else if (Input.GetAxis("Attack8") > 0)
		{
			if (abilities.Length > 7 && abilities[7])
                currentAbility = 7;
			
			#if UNITY_EDITOR
			debugLabelText.text = "Use Ability 8";
			#endif
		}
	}

    void MousePressed()
    {
        Debug.Log("Mouse Down");
        RaycastHit hit;
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (targetActor != null)
        {
            targetActor.GetComponent<ParticleSystem>().enableEmission = false;
        }

        if (currentAbility != -1)
        {
            bool abilityTargetFound = false;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100) || abilityTargetFound == true)
            {
                return;
            }
            else if(hit.collider.gameObject.tag == "Knight" || hit.collider.gameObject.tag == "Boss")
            {
                abilities[currentAbility].Cast();
                abilityTargetFound = true;
                targetActor = hit.collider.gameObject;
                #if UNITY_EDITOR
                    Debug.Log(currentAbility + " and " + hit.collider.gameObject.tag);
                #endif
            }
            else if (hit.collider.gameObject.tag == "Creature" && !hit.collider.isTrigger)
            {
                //as this detects the child capsule collider, we must get the parent object. note that if you make changes
                //to the creature prefab it may not work if you add more colliders as child objects with different sizes.
                //please make those child objects' layer "Ignore Raycast"
                abilities[currentAbility].Cast();
                abilityTargetFound = true;
                targetActor = hit.collider.gameObject.transform.parent.gameObject;
                #if UNITY_EDITOR
                    Debug.Log(currentAbility + " and " + hit.collider.gameObject.transform.parent.gameObject.tag);
                #endif
            }
            currentAbility = -1;
        }
        targetActor.GetComponent<ParticleSystem>().enableEmission = true;
    }
}
