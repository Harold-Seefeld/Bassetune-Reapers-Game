using UnityEngine;
using System.Collections;
using SocketIO;

public class Server : MonoBehaviour {

    // Connection to the server
    public string serverIP = "";
    public string serverPort = "";

    // The current match ID
    public string matchID = "";

	// Contains info about each player
	public class Player
    {
		public int id;
		public string username;
		public string nickname;
		public string side;
	}
	public Player[] players;
	public int currentPlayerID;

    private string uuid;
    private SocketIOComponent connection = null;

    // Connects to the game server when the gameplay level was loaded
    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            GameObject connectionManager = GameObject.Find("Connection Manager");
            connection = connectionManager.GetComponent<SocketIOComponent>();
            /*
                TODO IMPORTANT: RE-ENABLE COMMENTED OUT CODE WHEN IN PRODUCTION MODE
            */
            connection.url = "ws://127.0.0.1:" + serverPort + "/socket.io/?EIO=4&transport=websocket";
            //connection.url = "ws://" + serverIP + ":" + serverPort + "/?EIO=4&transport=websocket";
            connection.Connect();
            connection.On("connect", GetServerData);

            uuid = GameObject.Find("Client Data").GetComponent<ClientData>().GetSession();
        }
    }

    private void GetServerData(SocketIOEvent socket)
    {
        // Create the character manager
        if (FindObjectsOfType<CharacterManager>() == null)
        {
            gameObject.AddComponent<CharacterManager>();
        }

        // Join the appropriate room
        JSONObject registerData = new JSONObject(JSONObject.Type.OBJECT);
        registerData.AddField("uuid", uuid);
        registerData.AddField("matchID", matchID);
        connection.Emit("join", registerData);
    }

}
