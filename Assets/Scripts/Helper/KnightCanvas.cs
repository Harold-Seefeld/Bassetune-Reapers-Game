using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[AddComponentMenu("Helper/KnightCanvas")]
public class KnightCanvas : MonoBehaviour {
	[System.NonSerialized] public Image[] offensiveIcons;
	[System.NonSerialized] public Image[] defensiveIcons;

	void Start(){
		// Cache reference of skill images without it parent images
		var images = transform.FindChild("Skill Panel/Offensive Panel/Offensive Skills").GetComponentsInChildren<Image>();
		offensiveIcons = new Image[images.Length - 1];
		Array.Copy (images, 1, offensiveIcons, 0, images.Length - 1);

		images = transform.FindChild("Skill Panel/Defensive Panel/Defensive Skills").GetComponentsInChildren<Image>();
		defensiveIcons = new Image[images.Length - 1];
		Array.Copy (images, 1, defensiveIcons, 0, images.Length - 1);

		// TODO : Helper for Health, Time and Ally info
	}
}
