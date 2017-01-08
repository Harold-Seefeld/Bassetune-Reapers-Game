using UnityEngine;
using SocketIO;
using System.Collections;
using System.Collections.Generic;

public class UseCaller : MonoBehaviour {

    public static bool isKnight = true;
    public static List<CharacterData> selectedCharacters = new List<CharacterData>();

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
        if (selectedCharacters.Count == 0 && Server.instance.currentDefaultCharacter)
        {
            selectedCharacters.Add(Server.instance.currentDefaultCharacter);
        }

        // Inventory keybinds
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetButtonDown("Inventory" + i.ToString()))
            {
                Use(i + 9, "consumable");
            }
        }

        if (Input.GetButtonDown("Main-hand Attack"))
        {
            leftClicked = true;
        }
        else if (Input.GetButtonDown("Off-hand Attack"))
        {
            leftClicked = false;
        }
        else
        {
            return;
        }

        // Attack keybinds
        for (int i = 1; i <= 8; i++)
        {
            if (Input.GetButton("Attack" + i.ToString()))
            {
                Use(i - 1, "ability");
            }
        }

        // Defend keybinds
        for (int i = 1; i <= 3; i++)
        {
            if (Input.GetButton("Defend" + i.ToString()))
            {
                Use(i + 7, "ability");
            }
        }
    }

    private void Use(int slotIndex, string itemType)
    {
        ItemBase[] itemsAvailable = GetComponentsInChildren<ItemBase>();

        Debug.Log("Used: " + slotIndex.ToString());

        if (selectedCharacters.Count > 0)
        {
            CharacterData selectedCharacter = selectedCharacters[0];
            if (itemsAvailable.Length < slotIndex || itemsAvailable[slotIndex].itemID == 0)
            {
                return;
            }
            if (Server.instance.currentPlayerID != selectedCharacter.CharacterOwner)
            {
                return;
            }
            if (itemType == "consumable")
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                Vector2 direction = new Vector2(mousePosition.x, mousePosition.z);

                UseItem(itemsAvailable[slotIndex].itemID, selectedCharacter.CharacterID, direction);
            }
            else
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                Vector2 direction = new Vector2(mousePosition.x, mousePosition.z);

                UseAbility(itemsAvailable[slotIndex].itemID, direction, selectedCharacter.CharacterID);
            }
        }

        /* multiple cast: needs keybind
        ItemBase[] itemsAvailable = GetComponentsInChildren<ItemBase>();
        for (int i = 0; i < selectedCharacters.Count; i++)
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
        */
    }

    public void UseAbility(int abilityID, Vector2 target, int characterID)
    {
        JSONObject abilityUsage = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
        directionData.AddField("x", target.x);
        directionData.AddField("y", target.y);
        abilityUsage.AddField("target", directionData);
        abilityUsage.AddField("characterID", characterID);
        abilityUsage.AddField("abilityID", abilityID);
        abilityUsage.AddField("weapon", leftClicked ? 1 : 0);
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
