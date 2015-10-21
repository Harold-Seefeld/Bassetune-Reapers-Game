using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {

    // Connection to the server
    public string serverIP = "";
    public string serverPort = "";

    // The current match ID
    public string matchID = "";

	// Contains info about each player
	public class Player {
		public int id;
		public string username;
		public string nickname;
		public string side;
	}
	public Player[] players;
	public int currentPlayerID;
}
