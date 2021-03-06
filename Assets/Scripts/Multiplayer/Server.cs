﻿using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;
using System.Collections.Generic;
using DungeonGeneration;

public class Server : MonoBehaviour
{

    // Connection to the server
    public string serverIP = "";
    public string serverPort = "";

    // The current match ID
    public string matchID = "";

    // Dungeon Settings
    public string dungeonType;
    public GameObject[] dungeonBehaviourPrefabs;
    public int seed;

    public static Server instance;

    // Contains info about each player
    public class Player
    {
        public int id;
        public string username;
        public string nickname;
        public string side;
        public List<JSONObject> itemInventory = new List<JSONObject>();
        public List<JSONObject> abilityInventory = new List<JSONObject>();
    }
    public Player[] players;

    // Current character selections
    public int currentPlayerID = -1;
    private CharacterData defaultCharacter;
    public CharacterData currentDefaultCharacter {
        get { return defaultCharacter; }
        set { defaultCharacter = value; SelectionBehaviour.instance.SetSelected(value.gameObject); } }
    public Player currentPlayer;

    // Track when loading screen should dissapear
    public bool inventoryRecieved = false;
    public bool abilitiesRecieved = false;

    // Finish Game Screen
    public GameObject finishedGameUI;

    private string uuid;
    public SocketIOComponent connection = null;

    void Start()
    {
        instance = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Connects to the game server when the gameplay level was loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int level = scene.buildIndex;
        if (!connection && level == 2) 
        {
            // Create a socketIO object
            GameObject socketObject = new GameObject();
            connection = socketObject.AddComponent<SocketIOComponent>();
            /*
                TODO IMPORTANT: RE-ENABLE COMMENTED OUT CODE WHEN IN PRODUCTION MODE
            */
            connection.url = "ws://127.0.0.1:" + serverPort + "/socket.io/?EIO=4&transport=websocket";
            //connection.url = "ws://" + serverIP + ":" + serverPort + "/?EIO=4&transport=websocket";
            connection.Connect();
            connection.On("connect", GetServerData);
            connection.On(SocketIOEvents.Input.PLAYER, SetPlayerData);
            connection.On(SocketIOEvents.Input.Knight.ITEM_INVENTORY, SetItemInventory);
            connection.On(SocketIOEvents.Input.Knight.ABILITY_INVENTORY, SetAbilityInventory);
            connection.On(SocketIOEvents.Input.WIN_CONDITION_MET, ProcessWinCondition);
            connection.On("seed", SeedRecieved);

            uuid = GameObject.Find("Client Data").GetComponent<ClientData>().GetSession();
        }
        else if (level == 1 && connection)
        {
            // Remove connection once game is over
            Destroy(connection.gameObject);
        }
    }

    private void GetServerData(SocketIOEvent socket)
    {
        // Join the appropriate room
        JSONObject registerData = new JSONObject(JSONObject.Type.OBJECT);
        registerData.AddField("uuid", uuid);
        registerData.AddField("matchID", matchID);
        connection.Emit("join", registerData);
    }

    private void SetPlayerData(SocketIOEvent socket)
    {
        ClientData clientData = FindObjectOfType<ClientData>();
        List<JSONObject> socketData = socket.data.GetField("d").list;

        players = new Player[socketData.Count];
        for (var i = 0; i < players.Length; i++)
        {
            Player player = new Player();
            player.id = (int)socketData[i].GetField("i").n;
            player.username = socketData[i].GetField("u").str;
            player.nickname = socketData[i].GetField("n").str;
            player.side = socketData[i].GetField("s").str;
            players[i] = player;

            if (player.username == clientData.username)
            {
                currentPlayerID = player.id;
                currentPlayer = player;

                if (player.side == "knight")
                {
                    GameObject.Find("Boss Canvas").SetActive(false);
                }
                else
                {
                    GameObject.Find("Knight Canvas").SetActive(false);
                }
            }
        }

        // Set Default Character
        foreach (CharacterData characterData in CharacterManager.instance.characterData)
        {
            if (characterData.CharacterOwner == currentPlayerID)
            {
                // For knights
                if (characterData.CharacterEntity == 0 || characterData.CharacterEntity == 1)
                {
                    currentDefaultCharacter = characterData;
                    UseCaller.isKnight = true;
                }
                // For bosses
                if (characterData.CharacterEntity >= 3000 && characterData.CharacterEntity < 3400)
                {
                    currentDefaultCharacter = characterData;
                    UseCaller.isKnight = false;
                }
            }
        }
    }

    private void SetItemInventory(SocketIOEvent socket)
    {
        inventoryRecieved = true;

        JSONObject socketData = socket.data;
        for (var i = 0; i < players.Length; i++)
        {
            int id = (int)socketData.GetField("id").n;
            if (id != players[i].id)
            {
                continue;
            }

            players[i].itemInventory = socketData.GetField("i").list;

            // Find attached knight
            GameObject knight = null;
            foreach (CharacterData character in CharacterManager.instance.characterData)
            {
                if (character.CharacterOwner == id)
                {
                    knight = character.gameObject;
                }
            }
            EquippingBehaviour equippingBehaviour = knight.GetComponent<EquippingBehaviour>();

            foreach (JSONObject slot in players[i].itemInventory)
            {
                if ((int)slot.list[3].n == 2)
                {
                    equippingBehaviour.attachWeaponToRight((int)slot.list[0].n);
                }
                else if ((int)slot.list[3].n == 3)
                {
                    equippingBehaviour.attachWeaponToLeft((int)slot.list[0].n);
                }
                else if ((int)slot.list[3].n == 9)
                {
                    equippingBehaviour.attachWeaponToRight((int)slot.list[0].n);
                }
            }

            if (id == currentPlayerID && players[i].side == "knight")
            {
                KnightUIManager.instance.UpdateInventory();
            }
        }
    }

    private void SetAbilityInventory(SocketIOEvent socket)
    {
        abilitiesRecieved = true;

        JSONObject socketData = socket.data;
        for (var i = 0; i < players.Length; i++)
        {
            int id = (int)socketData.GetField("id").n;
            if (id != players[i].id)
            {
                continue;
            }

            players[i].abilityInventory = socketData.GetField("i").list;

            if (id == currentPlayerID && players[i].side == "knight")
            {
                KnightUIManager.instance.UpdateInventory();
            }
        }
    }

    private void ProcessWinCondition(SocketIOEvent socket)
    {
        JSONObject socketData = socket.data;
        string side = socket.data.GetField("side").str;
        string type = socket.data.GetField("type").str;

        if (side == "knight")
        {
            if (type == "progress" && dungeonType == "normal")
            {
                // TODO: Loading screen

                // Load the lord dungeon level
                SceneManager.LoadSceneAsync(2);
            }
            else if (type == "progress" && dungeonType == "lord")
            {
                // TODO: Loading Screen

                // Load the normal dungeon level
                SceneManager.LoadSceneAsync(2);
            }
            else if (type == "finish")
            {
                // TODO: Knight Win
                Instantiate(finishedGameUI);

                Destroy(gameObject);
            }
        }
        else
        {
            // TODO: Lord Win
        }
    }

    void SeedRecieved(SocketIOEvent ev)
    {
        seed = (int)ev.data.GetField("s").n;
        dungeonType = ev.data.GetField("t").str;

        // Select Dungeon Settings
        int generationIndex = 0;
        if (dungeonType == "lord")
        {
            generationIndex = 1;
        }

        GameObject dungeonGeneratorObject = Instantiate(dungeonBehaviourPrefabs[generationIndex]);
        DungeonGeneratorBehaviour dungeonGenerator = dungeonGeneratorObject.GetComponentInChildren<DungeonGeneratorBehaviour>();

        dungeonGenerator._seed = seed;
        dungeonGenerator.generateDungeon();
    }

}
