using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SocketIO;
using UnityEngine.EventSystems;

public class Chat : MonoBehaviour {

    public static bool isOpen = false;

    public string target = "F";

    public GameObject chatBox;
    public GameObject chatPanel;
    public GameObject chatBar;
    public GameObject chatLabel;

    [SerializeField] private GameObject targetAllButton;
    [SerializeField] private GameObject targetFriendButton;

	private SocketIOComponent socket;
	
	// Use this for initialization
	void Start () {
        socket = Server.instance.connection;
		socket.On(SocketIOEvents.talk, UpdateChatbox);

        chatPanel.transform.parent.gameObject.GetComponent<CanvasRenderer>().SetAlpha(1f);
    }

    void Update()
    {
        if (Input.GetButtonDown("Chat"))
        {
            // Send any message in the input field
            SendMessage();
            // Toggle chatbox open
            chatBox.gameObject.SetActive(!chatBox.gameObject.activeSelf);
            isOpen = !isOpen;
            if (isOpen) chatPanel.transform.parent.gameObject.GetComponent<CanvasRenderer>().SetAlpha(1);
            if (!isOpen) chatPanel.transform.parent.gameObject.GetComponent<CanvasRenderer>().SetAlpha(0);
            // Focus on the textbox if it's open
            EventSystem.current.SetSelectedGameObject(chatBar, null);
            chatBar.GetComponent<InputField>().OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }

	void UpdateChatbox (SocketIOEvent e) {
		int id = (int)e.data.GetField("id").n;
		string msg = e.data.GetField("msg").str;
        string target = e.data.GetField("t").str;
        // Find player that shares the ID
        string username = "";
        foreach (Server.Player player in Server.instance.players)
        {
            if (player.id == id)
            {
                username = player.nickname;
            }
        }

        GameObject chatMessage = (GameObject)Instantiate(chatLabel, Vector3.zero, Quaternion.identity);
        chatMessage.GetComponent<Text>().text = "[" + target + "] " + username + ": " + msg;
        chatMessage.transform.SetParent(chatPanel.transform);

        // TODO: Transition text in
        chatPanel.transform.parent.gameObject.GetComponent<Image>().CrossFadeAlpha(1, 0.2f, true);

        // Fade out after 3 seconds
        StopCoroutine(FadeOut());
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3);
        chatPanel.transform.parent.gameObject.GetComponent<Image>().CrossFadeAlpha(0, 1, true);
    }

    public void SetTarget(string newTarget)
    {
        target = newTarget;
    }

    public void SendMessage()
    {
        string message = chatBar.GetComponent<InputField>().text;

        if (message == "") return;

        chatBar.GetComponent<InputField>().text = "";

        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("message", message);
        data.AddField("target", target);
        socket.Emit(SocketIOEvents.talk, data);
    }
}
