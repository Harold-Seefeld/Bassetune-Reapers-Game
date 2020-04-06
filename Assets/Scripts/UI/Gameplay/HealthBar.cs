using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	
	public LifeSystem lifeSystem;
	public Text lifeText;
	public Slider healthSlider;
	
	void Update() 
	{
		// Set value each frame
		healthSlider.maxValue = lifeSystem.maxHealth;
		healthSlider.minValue = 0;
		
		// Set the value to the current health
		healthSlider.value = lifeSystem.health;
		
		// Set value each frame
		lifeText.text = Mathf.Round(lifeSystem.health).ToString();
	}
}