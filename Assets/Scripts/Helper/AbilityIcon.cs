using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Helper/AbilityIcon")]
public class AbilityIcon : MonoBehaviour {
	public static Sprite iconNone = Resources.Load<Sprite> ("Materials/UI/Ability/_none");

	[System.NonSerialized] public Image icon;
	[System.NonSerialized] public Text timer;

	void Start () {
		icon = GetComponent<Image> ();
		timer = GetComponentInChildren<Text> ();
	}
}
