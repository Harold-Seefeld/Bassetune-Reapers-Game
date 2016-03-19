using UnityEngine;
using System.Collections;

public class CharacterData : MonoBehaviour {

    // Properties of character itself
	public int CharacterID;
	public int CharacterHP;
	public int CharacterMaxHP;
	public int CharacterOwner;
	public int CharacterEntity;
    public string CharacterType;

    // Gameobject Properties
    public GridPlayer gridPlayer;

    void Start()
    {
        gridPlayer = GetComponent<GridPlayer>();
    }

}
