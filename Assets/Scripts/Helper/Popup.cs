using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Helper/Popup")]
public class Popup : MonoBehaviour {
	public  static Popup instance = null;

	public RectTransform rectTransform;
	public Text nameText;
	public Text typeText;
	public Text descriptionText;

	void Awake () {
		if (instance){
			Destroy(gameObject);
			return;
		}
		instance = this;
		rectTransform = GetComponent<RectTransform> ();
		nameText = transform.FindChild ("Name").GetComponent<Text>();
		typeText = transform.FindChild ("Type").GetComponent<Text>();
		descriptionText = transform.FindChild ("Description").GetComponent<Text>();
		gameObject.SetActive (false);
	}

	// Display function for ability popup
	public void Display(Vector3 position, string _name, string _type, string _description){
		rectTransform.position = position;
		nameText.text = "Name : " + _name;
		typeText.text = "Type : " + _type;
		descriptionText.text = "Description : " + _description;
	}

	// TODO : Add more overloading function for diferent kind of popup
}
