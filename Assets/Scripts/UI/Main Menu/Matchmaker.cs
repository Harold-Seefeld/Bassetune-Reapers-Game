using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System.Collections;
using UnityEngine.SceneManagement;

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

    public GameObject matchDataObject;

    // Search timer
    private float time = 0;
    private float timeStart = 0;

	// Use this for initialization
	void Start ()
    {
        socket.Connect();

        // Set listeners for when a match is found
        socket.On(SocketIOEvents.Matchmaker.FOUND, MatchFound);
        socket.On(SocketIOEvents.Matchmaker.SEARCHING, MatchSearching);
        socket.On(SocketIOEvents.Matchmaker.INTERRUPTED, MatchInterrupted);
        socket.On(SocketIOEvents.Matchmaker.MATCH_CREATION, OnMatchCreation);
        socket.On(SocketIOEvents.Matchmaker.ACCEPTED, OnPlayerAccept);
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
            System.TimeSpan t = System.TimeSpan.FromSeconds(seconds);
            timeText.text = string.Format("Searching... " + "{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);
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
            socket.Emit(SocketIOEvents.Matchmaker.FIND, matchData);
        }

    }

    // Called when any user accepts a match
    public void OnPlayerAccept(SocketIOEvent socketEvent)
    {
        // TODO: Show an indication of players that have accepted
    }

    // Called when a match is being searched for
    public void MatchSearching(SocketIOEvent socketEvent)
    {
        // Change button text to cancel
        buttonText.text = "Cancel";
        // Start updating the searching counter
        timeStart = Time.timeSinceLevelLoad;
        timeText.text = "Searching... 00:00:00";
    }

    // Called when a match found is interrupted
    public void MatchInterrupted(SocketIOEvent socketEvent)
    {
        // Reset Timer
        timeStart = Time.timeSinceLevelLoad;
        // Hide match panel
        matchFoundPanel.SetActive(false);
    }

    // Called when a match is found
    public void MatchFound(SocketIOEvent socketEvent)
    {
        // Show match panel
        matchFoundPanel.SetActive(true);
        matchFoundPanel.GetComponent<RectTransform>().SetAsLastSibling();
    }

    // Called when a user wants to cancel a search
    public void CancelSearch()
    {
        socket.Emit(SocketIOEvents.Matchmaker.CANCEL);
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
        socket.Emit(SocketIOEvents.Matchmaker.ACCEPT);
    }

    // Called when a user wants to decline a match
    public void DeclineMatch()
    {
        socket.Emit(SocketIOEvents.Matchmaker.DECLINE);
        // Set searching time text to nothing
        timeText.text = "";
        // Change button text back to start searching
        buttonText.text = "Find Match";
    }

    // Called when a match instance has been created
    public void OnMatchCreation(SocketIOEvent socketEvent)
    {
        var matchIP = socketEvent.data.GetField("ip").str;
        var matchPort = socketEvent.data.GetField("port").str;
        var matchID = socketEvent.data.GetField("id").str;

        // Create an object to store the data and make a new scene
        GameObject matchObject = Instantiate(matchDataObject);
        Server matchData = matchObject.GetComponent<Server>();
        matchData.serverIP = matchIP;
        matchData.serverPort = matchPort;
        matchData.matchID = matchID;
        DontDestroyOnLoad(matchObject);

        // Load the gameplay level
        SceneManager.LoadScene(2);
    }

    public void SetMatchType(string name)
    {
        matchType = name;
    }

    public void SetPlayerSide(string name)
    {
        side = name;
    }
}
