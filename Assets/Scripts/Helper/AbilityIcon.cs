﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Helper/AbilityIcon")]
public class AbilityIcon : MonoBehaviour {
	public static Sprite iconNone;

	public AbilityBase _ability;
	public RectTransform rectTransform;
	public Image icon;
	public Outline outline;
	public Text timer;

	void Awake () {
		if (iconNone == null) {
			iconNone = Resources.Load<Sprite> ("Materials/UI/Ability/_none");
		}

		rectTransform = GetComponent<RectTransform> ();
		icon = GetComponentInChildren<Image> ();
		outline = GetComponent<Outline> ();
		timer = GetComponentInChildren<Text> ();
	}

	public AbilityBase ability {
		get {
			return _ability;
		}
		set {
			_ability = value;
			if (_ability)
				icon.sprite = _ability.icon;
			else 
				icon.sprite = iconNone;
		}
	}

	public void OnPointerEnter(){
		if (!_ability) return;

		if (!Popup.instance) 
			Debug.LogError("Please make sure you have Popup Gameobject on your scene");

		Popup.instance.gameObject.SetActive (true);
		Popup.instance.Display (rectTransform.position + new Vector3 (0, 70),
		                        _ability.abilityName,
		                        _ability.abilityType.ToString(),
		                        _ability.description);
	}

	public void OnPointerExit(){
		if (!_ability) return;

		if (!Popup.instance) 
			Debug.LogError("Please make sure you have Popup Gameobject on your scene");

		Popup.instance.gameObject.SetActive (false);
	}
}
