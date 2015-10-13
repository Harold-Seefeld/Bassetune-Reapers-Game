using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System.Collections;

public class Matchmaker : MonoBehaviour {

    //[SerializeField]
    //private string server = "matchmaker.bassetune.com";

    [SerializeField]
    private SocketIOComponent socket;

    // Match find variables
    public string matchType = "normal";
    public string matchPlayers = "3v1";
    public string side = "knight";
    public string uuid = "";
    public int partyID = 0;
    public string region = "US-East1";

    // UI change variables
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private GameObject matchFoundPanel;

    // Search timer
    private float time = 0;
    private float timeStart = 0;

	// Use this for initialization
	void Start ()
    {
        // Set the connection URL
        //socket.url = server;
        // Establish a connection with the server
        // socket.Connect();
        // Set listeners for when a match is found
        socket.On("found", MatchFound);
        socket.On("searching", MatchSearching);
        socket.On("int", MatchInterrupted);
        // Set UUID
        uuid = ((ClientData)FindObjectOfType(typeof(ClientData))).GetSession();
    }

    // Used every frame
    void Update()
    {
        // Update the timer for the search
        time = Time.timeSinceLevelLoad;
        // Update searching timer if it is searching
        if (timeText.text != "")
        {
            float seconds = time - timeStart;
            timeText.text = "Searching... " + string.Format("{0}:{1}", (int)seconds / 60, (int)seconds % 60);
        }
    }
	
	// Use this for starting a game search
	public void FindMatch() 
    {
        if (buttonText.text == "Cancel")
        {
            CancelSearch();
        }
        else
        {
            JSONObject matchData = new JSONObject(JSONObject.Type.OBJECT);
            matchData.AddField("matchType", matchType);
            matchData.AddField("matchPlayers", matchPlayers);
            matchData.AddField("side", side);
            matchData.AddField("uuid", uuid);
            matchData.AddField("partyID", partyID);
            matchData.AddField("region", region);
            socket.Emit("find", matchData);
        }

    }

    // Called when a match is being searched for
    public void MatchSearching(SocketIOEvent socketEvent)
    {
        // Change button text to cancel
        buttonText.text = "Cancel";
        // Start updating the searching counter
        timeStart = Time.timeSinceLevelLoad;
        timeText.text = "Searching... 0:00";
    }

    // Called when a match found is interrupted
    public void MatchInterrupted(SocketIOEvent socketEvent)
    {
        // Hide match panel
        matchFoundPanel.SetActive(false);
    }

    // Called when a match is found
    public void MatchFound(SocketIOEvent socketEvent)
    {
        // Show match panel
        matchFoundPanel.SetActive(true);
    }

    // Called when a user wants to cancel a search
    public void CancelSearch()
    {
        socket.Emit("cancel");
        // Set searching time text to nothing
        timeText.text = "";
        // Change button text back to start searching
        buttonText.text = "Find Match";
        // Hide match panel
        matchFoundPanel.SetActive(false);
    }

    // Called when a user wants to accept a match
    public void AcceptMatch()
    {
        socket.Emit("accept");
    }

    // Called when a user wants to decline a match
    public void DeclineMatch()
    {
        socket.Emit("decline");
        // Set searching time text to nothing
        timeText.text = "";
        // Change button text back to start searching
        buttonText.text = "Find Match";
    }
}
