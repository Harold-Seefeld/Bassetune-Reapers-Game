using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;

public class Server : MonoBehaviour {

    // Connection to the server
    public string serverIP = "";
    public string serverPort = "";

    // The current match ID
    public string matchID = "";

    public static Server instance;

	// Contains info about each player
	public class Player
    {
		public int id;
		public string username;
		public string nickname;
		public string side;
        public List<JSONObject> itemInventory;
	}
	public Player[] players;
	public int currentPlayerID;

    private string uuid;
    private SocketIOComponent connection = null;

    void Start()
    {
        instance = this;
    }

    // Connects to the game server when the gameplay level was loaded
    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            connection = FindObjectOfType<SocketIOComponent>();
            /*
                TODO IMPORTANT: RE-ENABLE COMMENTED OUT CODE WHEN IN PRODUCTION MODE
            */
            connection.url = "ws://127.0.0.1:" + serverPort + "/socket.io/?EIO=4&transport=websocket";
            //connection.url = "ws://" + serverIP + ":" + serverPort + "/?EIO=4&transport=websocket";
            connection.Connect();
            connection.On("connect", GetServerData);
            connection.On(SocketIOEvents.Input.PLAYER, SetPlayerData);
            connection.On(SocketIOEvents.Input.Knight.ITEM_INVENTORY, SetItemInventory);

            uuid = GameObject.Find("Client Data").GetComponent<ClientData>().GetSession();
        }
    }

    private void GetServerData(SocketIOEvent socket)
    {
        // Join the appropriate room
        JSONObject registerData = new JSONObject(JSONObject.Type.OBJECT);
        registerData.AddField("uuid", uuid);
        registerData.AddField("matchID", matchID);
        connection.Emit("join", registerData);
        //StartCoroutine(GetServerData());
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
    }

    private void SetItemInventory(SocketIOEvent socket)
    {
        JSONObject socketData = socket.data;
        for (var i = 0; i < players.Length; i++)
        {
            int id = (int)socketData.GetField("id").n;
            if (id != players[i].id)
            {
                continue;
            }
            players[i].itemInventory = socketData.GetField("i").list;
        }
    }

}
