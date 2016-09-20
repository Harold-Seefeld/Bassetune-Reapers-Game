using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SocketIO;

public class Chat : MonoBehaviour {

    public string target = "F";

    public GameObject chatBox;
    public GameObject chatBar;
    public GameObject chatLabel;

    [SerializeField] private GameObject targetAllButton;
    [SerializeField] private GameObject targetFriendButton;

	private SocketIOComponent socket;
	
	// Use this for initialization
	void Start () {
		// Get socket object
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On(SocketIOEvents.talk, UpdateChatbox);		
	}

	// Use this for initialization
	void UpdateChatbox (SocketIOEvent e) {
		int id = (int)e.data.GetField("id").n;
		string msg = e.data.GetField("msg").str;
        // Find player that shares the ID
        string username = "";
        foreach (Server.Player player in Server.instance.players)
        {
            if (player.id == id)
            {
                username = player.username;
            }
        }
        // TODO: Make this work with a chatbox
        GameObject chatMessage = (GameObject)Instantiate(chatLabel, Vector3.zero, Quaternion.identity);
        chatMessage.GetComponentsInChildren<Text>()[0].text = username;
        chatMessage.GetComponentsInChildren<Text>()[1].text = msg;
    }

	void SendChatMessage (string message) {
		JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
		data.AddField("message", message);
		socket.Emit(SocketIOEvents.talk, data);
	}

    public void SetTarget(string newTarget)
    {
        target = newTarget;
    }

    public void SendMessage()
    {
        string message = chatBar.GetComponentInChildren<Text>().text;

        if (message != "")
        {
            SendChatMessage(message);
        }
    }
}
