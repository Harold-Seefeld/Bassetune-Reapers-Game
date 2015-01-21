// by dmongs
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
	}

	void Update(){
		// Call base update function
		BaseUpdate ();

		// Set animation movement variable
		anim.SetFloat (AniMoveSpeedName, agent.velocity.magnitude);
	}
}
