using UnityEngine;
using SocketIO;
using System.Collections;
using System.Collections.Generic;

public class UseCaller : MonoBehaviour {

    public static bool isKnight = true;
    public static CharacterData selectedCharacter
    {
        get
        {
            if (SelectionBehaviour.instance._currentSelection)
            {
                return SelectionBehaviour.instance._currentSelection.GetComponent<CharacterData>();
            }
            else return null;
        }
        set
        {
            SelectionBehaviour.instance.SetSelected(value.gameObject);
        }
    }

    private SocketIOComponent socket;
    private bool leftClicked = false;

    // Use this for initialization
    void Start()
    {
        // Get socket object
        socket = FindObjectOfType<SocketIOComponent>();
        // Get minimap if possible
        if (GameObject.Find("MinimapCam")) miniMapCamera = GameObject.Find("MinimapCam").GetComponent<Camera>();
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
                return;
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
                    return;
                }
                else if (Input.GetButtonDown("Off-hand Attack"))
                {
                    leftClicked = false;
                    Use(i - 1, "ability");
                    return;
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
                    return;
                }
                else if (Input.GetButtonDown("Off-hand Attack"))
                {
                    leftClicked = false;
                    Use(i + 7, "ability");
                    return;
                }
            }
        }

        if (Input.GetButtonDown("Move"))
        {
            UpdateDestination();
            return;
        }

        if (Input.GetButtonDown("Selection"))
        {
            SelectionBehaviour.instance.CheckForSelection();
            return;
        }
    }

    private void Use(int slotIndex, string itemType)
    {
        //selectedCharacter = Server.instance.currentDefaultCharacter;
        Server.Player player = Server.instance.currentPlayer;

        Debug.Log("Used: " + slotIndex.ToString());

        if (Server.instance.currentPlayerID != selectedCharacter.CharacterOwner)
        {
            selectedCharacter = Server.instance.currentDefaultCharacter;
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

    private Camera miniMapCamera;
    private void UpdateDestination()
    {
        if (Server.instance && Server.instance.currentPlayerID != selectedCharacter.CharacterOwner)
        {
            selectedCharacter = Server.instance.currentDefaultCharacter;
        }

        if (miniMapCamera && Input.mousePosition.x > (Screen.width / 10) * 7F && Input.mousePosition.y < (Screen.height / 10) * 3.5F)
        {
            // Minimap Camera
            Ray ray = miniMapCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Emitting X, Z to server for verification
                CharacterManager.instance.AddLocation(selectedCharacter.CharacterID.ToString(), new Vector2(hit.point.x, hit.point.z));
            }
        }
        else
        {
            // Main Camera
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                CharacterManager.instance.AddLocation(selectedCharacter.CharacterID.ToString(), new Vector2(hit.point.x, hit.point.z));
            }
        }
    }
}
