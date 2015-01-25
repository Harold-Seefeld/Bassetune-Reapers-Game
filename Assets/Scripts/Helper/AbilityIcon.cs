﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Helper/AbilityIcon")]
public class AbilityIcon : MonoBehaviour {
	public static Sprite iconNone;

	public Image icon;
	public Outline outline;
	public Text timer;

	void Awake () {
		if (iconNone == null) {
			iconNone = Resources.Load<Sprite> ("Materials/UI/Ability/_none");
		}

		icon = GetComponentInChildren<Image> ();
		outline = GetComponent<Outline> ();
		timer = GetComponentInChildren<Text> ();
	}
}
