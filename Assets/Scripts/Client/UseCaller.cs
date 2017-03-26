using UnityEngine;
using SocketIO;
using System.Collections;
using System.Collections.Generic;

public class UseCaller : MonoBehaviour {

    public static bool isKnight = true;
    public static CharacterData selectedCharacter;

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
        // Inventory keybinds
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetButtonDown("Inventory" + i.ToString()))
            {
                Use(i + 19, "consumable");
            }
        }

        // Attack keybinds
        for (int i = 1; i <= 8; i++)
        {
            if (Input.GetButton("Attack" + i.ToString()))
            {
                if (Input.GetButtonDown("Main-hand Attack"))
                {
                    leftClicked = true;
                    Use(i - 1, "ability");
                }
                else if (Input.GetButtonDown("Off-hand Attack"))
                {
                    leftClicked = false;
                    Use(i - 1, "ability");
                }
            }
        }

        // Defend keybinds
        for (int i = 1; i <= 3; i++)
        {
            if (Input.GetButton("Defend" + i.ToString()))
            {
                if (Input.GetButtonDown("Main-hand Attack"))
                {
                    leftClicked = true;
                    Use(i + 7, "ability");
                }
                else if (Input.GetButtonDown("Off-hand Attack"))
                {
                    leftClicked = false;
                    Use(i + 7, "ability");
                }
            }
        }
    }

    private void Use(int slotIndex, string itemType)
    {
        selectedCharacter = Server.instance.currentDefaultCharacter;
        Server.Player player = Server.instance.currentPlayer;

        Debug.Log("Used: " + slotIndex.ToString());

        if (Server.instance.currentPlayerID != selectedCharacter.CharacterOwner)
        {
            return;
        }
        if (itemType == "consumable")
        {
            for (int n = 0; n < player.itemInventory.Count; n++)
            {
                if (player.itemInventory[n].list[2].n == slotIndex && player.itemInventory[n].list[0].n != 0)
                    UseItem(slotIndex, selectedCharacter.CharacterID);
            }
        }
        else if (itemType == "ability")
        {
            Vector2 direction = Vector2.zero;

            // Main Camera
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                direction = new Vector2(hit.point.x, hit.point.z);
            }

            if (direction == Vector2.zero) return;

            for (int n = 0; n < player.abilityInventory.Count; n++)
            {
                if (player.abilityInventory[n].list[2].n == slotIndex && player.abilityInventory[n].list[0].n != 0)
                    UseAbility(slotIndex, direction, selectedCharacter.CharacterID);
            }
        }
    }

    public void UseAbility(int slotID, Vector2 target, int characterID)
    {
        JSONObject abilityUsage = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject directionData = new JSONObject(JSONObject.Type.OBJECT);
        directionData.AddField("x", target.x);
        directionData.AddField("y", target.y);
        abilityUsage.AddField("target", directionData);
        abilityUsage.AddField("characterID", characterID);
        abilityUsage.AddField("slotID", slotID);
        abilityUsage.AddField("weapon", leftClicked ? 0 : 1);
        socket.Emit(SocketIOEvents.Output.Knight.ABILITY_START, abilityUsage);
    }

    public void UseItem(int slotID, int characterID)
    {
        JSONObject itemUsage = new JSONObject(JSONObject.Type.OBJECT);
        itemUsage.AddField("characterID", characterID);
        itemUsage.AddField("slotID", slotID);
        socket.Emit(SocketIOEvents.Output.Knight.USE_ITEM, itemUsage);
    }
}
