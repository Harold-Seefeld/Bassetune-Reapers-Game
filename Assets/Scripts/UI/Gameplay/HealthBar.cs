using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public LifeSystem lifeSystem;
    public Text lifeText;

    private void Update()
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