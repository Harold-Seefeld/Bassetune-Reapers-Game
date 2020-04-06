﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisplay : MonoBehaviour {

    public Slider healthSlider;
	
	// Update is called once per frame
	void Update () {
        // Show either default character or first selected characters health
        if (UseCaller.selectedCharacter)
        {
            CharacterData selectedCharacter = UseCaller.selectedCharacter;
            healthSlider.maxValue = selectedCharacter.CharacterMaxHP;
            healthSlider.value = selectedCharacter.CharacterHP;
        }
        else
        {
            if (!Server.instance) return;
            if (!Server.instance.currentDefaultCharacter) return;

            CharacterData selectedCharacter = Server.instance.currentDefaultCharacter;
            healthSlider.maxValue = selectedCharacter.CharacterMaxHP;
            healthSlider.value = selectedCharacter.CharacterHP;
        }
    }
}
