using UnityEngine;
using SocketIO;
using System.Collections;

public class UseCaller : MonoBehaviour {

    public bool isKnight = true;
    public static CharacterData[] selectedCharacters;

    private SocketIOComponent socket;
    private bool leftClicked = false;

    // Use this for initialization
    void Start()
    {
        // Get socket object
        socket = FindObjectOfType<SocketIOComponent>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButton(0))
        {
            leftClicked = true;
        }
        else if (Input.GetMouseButton(1))
        {
            leftClicked = false;
        }
        else
        {
            return;
        }

	    if (Input.GetKeyDown("y"))
        {
            Use(13, "consumable");
        }
        else if (Input.GetKeyDown("u"))
        {
            Use(14, "consumable");
        }
        else if (Input.GetKeyDown("i"))
        {
            Use(15, "consumable");
        }
    }

    private void Use(int slotIndex, string itemType)
    {
        if(selectedCharacters.Length == 0)
        {
            // TODO: selectedCharacters[0] = DefaultPlayerCharacter;
        }

        ItemBase[] itemsAvailable = GetComponentsInChildren<ItemBase>();
        for (int i = 0; i < selectedCharacters.Length; i++)
        {
            CharacterData selectedCharacter = selectedCharacters[i];
            if (itemsAvailable.Length >= slotIndex || itemsAvailable[slotIndex].itemID == 0)
            {
                continue;
            }
            if (selectedCharacter.CharacterEntity != selectedCharacters[i].CharacterEntity || Server.instance.currentPlayerID != selectedCharacter.CharacterOwner)
            {
                continue;
            }
            if (itemType == "consumable")
            {
                // TODO: Update for inventory ranged weapons
                UseItem(itemsAvailable[i].itemID, selectedCharacter.CharacterID, new Vector2(selectedCharacter.transform.forward.x, selectedCharacter.transform.forward.z));
            }
            else
            {
                //UseAbility();
            }
        }
    }

    public void UseAbility(int abilityID, Vector2 target, int characterID, int weaponID)
    {
        JSONObject abilityUsage = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
        directionData.AddField("x", target.x);
        directionData.AddField("y", target.y);
        abilityUsage.AddField("target", directionData);
        abilityUsage.AddField("characterID", characterID);
        abilityUsage.AddField("abilityID", abilityID);
        abilityUsage.AddField("weapon", weaponID);
        socket.Emit(SocketIOEvents.Output.Knight.ABILITY_START, abilityUsage);
    }

    public void UseItem(int itemID, int characterID, Vector2 target)
    {
        JSONObject itemUsage = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
        directionData.AddField("x", target.x);
        directionData.AddField("y", target.y);
        itemUsage.AddField("characterID", characterID);
        itemUsage.AddField("itemID", itemID);
        socket.Emit(SocketIOEvents.Output.Knight.USE_ITEM, itemUsage);
    }
}
