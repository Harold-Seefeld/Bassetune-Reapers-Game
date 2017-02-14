using UnityEngine;
using System;
using SocketIO;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : MonoBehaviour {

    public SocketIOComponent socket;
	public List<CharacterData> characterData;

    public static CharacterManager instance;

    private JSONObject locationsToSend = new JSONObject(JSONObject.Type.OBJECT);
    public PrefabStore characterPrefabs;

    // Use this for initialization
    void Start () {
        instance = this;

		socket = FindObjectOfType<SocketIOComponent>();
		// Listen out for new character creations
		socket.On(SocketIOEvents.Input.CHAR_CREATED, CreateCharacter);
		socket.On(SocketIOEvents.Input.HP, UpdateHP);
        socket.On(SocketIOEvents.move, UpdateLocations);

        // Listen for equipment changes
        socket.On(SocketIOEvents.Input.Knight.START_CHANGE_EQUIPPED, StartEquipAnimation);
        socket.On(SocketIOEvents.Input.Knight.END_CHANGE_EQUIPPED, UpdateKnightInventory);
    }

    void Update ()
    {
        // Return if no locations to send
        if (locationsToSend.Count == 0)
        {
            return;
        }
        socket.Emit(SocketIOEvents.move, locationsToSend);
        // Empty locations to send array
        locationsToSend = new JSONObject(JSONObject.Type.OBJECT);
    }

	void CreateCharacter(SocketIOEvent e)
    {
        StartCoroutine(CreateCharacter(Server.instance.currentPlayerID != 1, e));
	}

    IEnumerator CreateCharacter(bool playerIDSet, SocketIOEvent e)
    {
        // Delay
        if (!playerIDSet)
        {
            yield return new WaitForSeconds(1);
        }

        // Check if character already exists
        for (var i = 0; i < characterData.Count; i++)
        {
            if (characterData[i].CharacterID == (int)e.data.GetField("I").n)
            {
                // Don't create a new character
                yield break;
            }
        }
        // TODO: Create character with given data (assign meshes, etc use a prefab)
        Vector3 location = new Vector3(e.data.GetField("L").GetField("x").n, 1.35f, e.data.GetField("L").GetField("y").n);
        // Search for character, given a set of prefabs and an entity id
        GameObject newCharacter;
        for (var i = 0; i < characterPrefabs.prefabs.Length; i++)
        {
            if ((int)e.data.GetField("E").n == characterPrefabs.prefabs[i].GetComponent<ItemBase>().itemID)
            {
                newCharacter = (GameObject)Instantiate(characterPrefabs.prefabs[i], location, Quaternion.identity);

                CharacterData newCharacterData = newCharacter.AddComponent<CharacterData>();
                newCharacterData.CharacterEntity = (int)e.data.GetField("E").n;
                newCharacterData.CharacterHP = (int)e.data.GetField("H").n;
                newCharacterData.CharacterID = (int)e.data.GetField("I").n;
                newCharacterData.CharacterOwner = (int)e.data.GetField("O").n;
                newCharacterData.CharacterMaxHP = (int)e.data.GetField("M").n;
                // Add character data to the list
                characterData.Add(newCharacterData);
                // Allow character to be selected
                newCharacter.AddComponent<UnityEngine.UI.Extensions.CharacterSelectable>();

                // Set Default Character
                if (Server.instance.currentPlayerID == newCharacterData.CharacterOwner)
                {
                    // For knights
                    if (newCharacterData.CharacterEntity == 0 || newCharacterData.CharacterEntity == 1)
                    {
                        Server.instance.currentDefaultCharacter = newCharacterData;
                        UseCaller.isKnight = true;
                    }
                    // For bosses
                    if (newCharacterData.CharacterEntity >= 3000 || newCharacterData.CharacterEntity < 3200)
                    {
                        Server.instance.currentDefaultCharacter = newCharacterData;
                        UseCaller.isKnight = false;
                    }
                }

                break;
            }
        }
    }

	void UpdateHP(SocketIOEvent e)
    {
        var data = e.data.GetField("d").list;
		for (int n = 0; n < e.data.Count; n++)
        {
			for (int i = 0; i < characterData.Count; i++)
            {
				if ((int)data[n].GetField("i").n == characterData[i].CharacterID)
                {
					characterData[i].CharacterHP = Convert.ToInt16(data[n].GetField("h").n);
					i = characterData.Count;
				}
			}
		}
	}

    public void UpdateLocations(SocketIOEvent e)
    {
        // Update locations based on the recieved json
        List<JSONObject> data = e.data.GetField("d").list;
        for (int i = 0; i < data.Count; i++)
        {
            int recievedCharacterID = (int)data[i].GetField("i").n;
            for (int n = 0; n < characterData.Count; n++)
            {
                if (characterData[n].CharacterID == recievedCharacterID)// && characterData[n].CharacterOwner != Server.instance.currentPlayerID)
                {
                    // TODO: Assign locations to character movement agents for smoothness
                    Vector3 newPos = new Vector3(data[i].GetField("l").list[0].n, 1.35f, data[i].GetField("l").list[1].n);
                    characterData[n].gridPlayer.currentDestination = newPos;
                }
            }
        }
        // Example: {"d":[{"i":"1","l":[0,525.3192]},{"i":"2","l":[0,525.3192]}]}
    }

    public void AddLocation(string characterID, Vector2 location)
    {
        JSONObject locationData = new JSONObject(JSONObject.Type.ARRAY);
        locationData.Add(location.x);
        locationData.Add(location.y);
        locationsToSend.AddField(characterID, locationData);
    }

    public void UpdateKnightInventory(SocketIOEvent e)
    {
        int playerID = (int)e.data.GetField("p").n;
        for (int i = 0; i < Server.instance.players.Length; i++)
        {
            if (playerID != Server.instance.players[i].id) continue;
            var player = Server.instance.players[i];

            // Slot change
            if (e.data.HasField("a") && e.data.HasField("b"))
            {
                JSONObject slot1 = null;
                JSONObject slot2 = null;

                for (int n = 0; n < player.itemInventory.Count; n++)
                {
                    if (e.data.GetField("a").n == player.itemInventory[n].list[2].n)
                    {
                        slot1 = player.itemInventory[n];
                    }
                    else if (e.data.GetField("b").n == player.itemInventory[n].list[2].n)
                    {
                        slot2 = player.itemInventory[n];
                    }
                }

                if (slot1 != null && slot2 !=null)
                {
                    int temp = (int)slot1.list[2].n;
                    slot1.list[2].n = slot2.list[2].n;
                    slot2.list[2].n = temp;
                }
            }
            // Equip type change
            else
            {
                int slotID = (int)e.data.GetField("i").n;
                int target = (int)e.data.GetField("t").n;
                // Linear search array for any existing tags and overwrite them
                foreach (JSONObject slot in player.itemInventory)
                {
                    if ((int)slot.list[2].n == slotID)
                    {
                        slot.list[3].n = target;

                    }
                    else
                    {
                        if (target == 2 || target == 3 || target == 9)
                        {
                            if (target == 2)
                            {
                                if ((int)slot.list[3].n == target || (int)slot.list[3].n == 3 || (int)slot.list[3].n == 9)
                                {
                                    slot.list[3].n = 0;
                                }
                            }
                            else if (target == 3)
                            {
                                if ((int)slot.list[3].n == target || (int)slot.list[3].n == 2 || (int)slot.list[3].n == 9)
                                {
                                    slot.list[3].n = 0;
                                }
                            }
                            else if (target == 9)
                            {
                                if ((int)slot.list[3].n == target || (int)slot.list[3].n == 3 || (int)slot.list[3].n == 2)
                                {
                                    slot.list[3].n = 0;
                                }
                            }
                        }
                        else
                        {
                            if ((int)slot.list[3].n == target)
                            {
                                slot.list[3].n = 0;
                            }
                        }
                    }
                }
            }
        }

        // Update menus
        AbilityMenu.instance.UpdateMenu();
        InventoryMenu.instance.UpdateMenu();

        // TODO: Update item equipped
    }

    public void StartEquipAnimation(SocketIOEvent e)
    {
        // TODO: Start Equipping Animation
    }

}
