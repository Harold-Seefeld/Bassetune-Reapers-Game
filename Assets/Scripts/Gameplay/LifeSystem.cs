using UnityEngine;
using System.Collections;

public class RPG_LifeSystem : MonoBehaviour {
	
	// Health
	public float health = 100; // 100 by Default
	public float maxHealth = 100; // The maximum amount of health

	public bool isDead;
	public string deathAnimation;
	
	/// --------------------
	/// Instant Health Regen
	/// --------------------
	public void healthRegen (float amount) 
	{
		health = Mathf.Clamp (health + amount, 0, maxHealth);
	}
	
	// Automatic Health Rengeneration
	[System.NonSerialized]
	public bool healthRegenerating = true; // Is the player regenerating?

	public float healthRegenAmount = 5.0f; // How much health should be regenerated per second?
	public float healthRegenWaitTime = 3.0f; // How long is the cooldown until health starts being regenerated?
	/// 

	/// -------------------
	/// Toggle Health Regen
	/// -------------------
	public IEnumerator AutoHealthRegenToggle () 
	{

		healthRegenerating = false;
		
		yield return new WaitForSeconds (healthRegenWaitTime);
		
		healthRegenerating = true;
	}

	/// -----------------------------
	/// Restores Health Automatically
	/// -----------------------------
	public void AutoHealthRegen()
	{
		if (!isDead)
			health = Mathf.Clamp (health + (healthRegenAmount * Time.deltaTime), 0, maxHealth);
	}
	
	// Friendly Fire
	public bool recieveFriendlyFire;

	// Check if Ally or Enemy
	public string currentTeam; // Team that this player is on.
	public bool isEnemy (string attackingTeam) 
	{	
		if (currentTeam != attackingTeam)
		{
			return true;
		}
		
		return false;
	}
	///
	
	/// ------------
	/// Damage Input
	/// ------------
	public void damageTaken(float damage, string attackingTeam) 
	{
		// Reset automatic regeneration
		StopCoroutine("AutoHealthRegenToggle");
		StartCoroutine ("AutoHealthRegenToggle");
		///
		
		if (recieveFriendlyFire) 
		{
			health = Mathf.Clamp(health - damage, 0, maxHealth);

			// Kill player on low health
			if (health <= 0.49f)
			{
				playerDeath(attackingTeam);
				isDead = true;
			}
			///
		}
		
		else if (!recieveFriendlyFire) 
		{
			if (isEnemy(attackingTeam)) 
			{
				health = Mathf.Clamp(health - damage, 0, maxHealth);
				
				// Kill player on low health
				if (health <= 0.49f)
				{
					playerDeath(attackingTeam);
					isDead = true;
				}
				///
			}
		}
	}

	public Animator anim;
	private void playerDeath (string killingTeam) 
	{
		anim.Play (deathAnimation);

		StartCoroutine(RespawnTimer(10));
	}

	/// -------------------------------------------
	/// Allows a dialog to access remaining seconds
	/// -------------------------------------------		
	public int respawnTime;
	private IEnumerator RespawnTimer(int length)
	{
		respawnTime = length;

		while (respawnTime > 0)
		{
			yield return new WaitForSeconds(1);
			respawnTime--;
		}

		Destroy (gameObject);
	}
	
	/// ------------------
	/// Called Every Frame
	/// ------------------
	void Update() 
	{
		if (healthRegenerating) 
		{
			AutoHealthRegen ();
		}
	}

	void Start()
	{
		if (!anim)
			anim = GetComponent<Animator> ();
	}
}