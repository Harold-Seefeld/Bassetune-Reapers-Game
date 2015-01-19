// by dmongs
using UnityEngine;
using System.Collections;

public class Knight : Character {
	
	// singleton --->
	protected static Knight instance = null;
	public static Knight Instance { get { return instance; } }
	
	void Awake () {
		if ( instance != null && instance != this ) { Destroy ( this.gameObject ); return; }
		else { instance = this; }
	}
	// <--- singleton


}
