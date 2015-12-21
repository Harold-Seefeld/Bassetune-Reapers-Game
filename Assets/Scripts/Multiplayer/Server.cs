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
    public string uuid;

    // Connects to the game server when the gameplay level was loaded
    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            GameObject connectionManager = GameObject.Find("Connection Manager");
            SocketIOComponent connection = connectionManager.GetComponent<SocketIOComponent>();
            connection.url = serverIP + ":" + serverPort;
            connection.Connect();

            uuid = GameObject.Find("Client Data").GetComponent<ClientData>().GetSession();
            StartCoroutine(GetServerData(connection));
        }
    }

    private IEnumerator GetServerData(SocketIOComponent connection)
    {
        // Wait for 2 seconds for connection to establish
        yield return new WaitForSeconds(2);
        // Request Data
        JSONObject registerData = new JSONObject(JSONObject.Type.OBJECT);
        registerData.AddField("uuid", uuid);
        connection.Emit("register", registerData);
    }

}
