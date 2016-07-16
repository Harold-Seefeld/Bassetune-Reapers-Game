using UnityEngine;
using System.Collections;

public class CharacterDeathController : MonoBehaviour 
{
	public float health = 100f;

	public float resetAfterDeathTime = 5f;

	private float timer;
	private bool playerDead;

    void Update()
    {
        if (!CharacterManager.instance) return;

        foreach (CharacterData characterData in CharacterManager.instance.characterData)
        {
            health = characterData.CharacterHP;

            if (health <= 0f)
            {
                Animator anim = characterData.gameObject.GetComponent<Animator>();

                anim.SetBool("dead", true);

                //A.A: audio clip of player dying: todo implement sounds for different characters
                // AudioSource.PlayClipAtPoint(deathClip, transform.position);
            }
        }
	}

}
