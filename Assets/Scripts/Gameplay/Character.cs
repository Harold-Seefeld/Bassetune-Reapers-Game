// by dmongs
using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	protected Animator anim;

	public const int Layer = 9;

	// animation parmeters
	const string AniMoveSpeedName = "MoveSpeed";
	const string AniAttackedName = "Attacked";


	void Start () {
		anim = GetComponent<Animator> ();
	}

	// animator - change state
	public void SetAniParamMoveSpeed ( float moveSpeed ) {
		anim.SetFloat (AniMoveSpeedName, moveSpeed);
	}
	
	public void SetAniParamAttacked ( bool isAttacked ) {
		anim.SetBool (AniAttackedName, isAttacked);
	}
}
