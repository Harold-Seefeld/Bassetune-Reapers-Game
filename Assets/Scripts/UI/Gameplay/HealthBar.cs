using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public Slider healthSlider;
    private CharacterData character;

    // Use this for initialization
    void Start () {
        character = GetComponent<CharacterData>();

        healthSlider.maxValue = character.CharacterMaxHP;
        healthSlider.value = character.CharacterHP;
    }
	
	// Update is called once per frame
	void Update () {
        healthSlider.transform.LookAt(Camera.main.transform.position);

        healthSlider.maxValue = character.CharacterMaxHP;
        healthSlider.value = character.CharacterHP;
    }
}
