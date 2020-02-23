﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[AddComponentMenu("Helper/InGameCanvas")]
public class InGameCanvas : MonoBehaviour {
	[System.NonSerialized] public AbilityIcon[] abilities;

	void Awake(){
		// Cache reference
		abilities = transform.Find("Skill Panel").GetComponentsInChildren<AbilityIcon>();

		// TODO : Helper for Health, Time and Ally info
	}
}
