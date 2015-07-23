using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {
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
