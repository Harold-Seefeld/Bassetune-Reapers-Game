using SocketIO;
using UnityEngine;

public class SocketIOTest : MonoBehaviour
{

    private SocketIOComponent socket;

    // Use this for initialization
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.Connect();

        System.Threading.Thread.Sleep(1000);

        socket.On("boop", TestBoop);
        socket.Emit("beep");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TestBoop(SocketIOEvent e)
    {
        Debug.Log("boop received");
        socket.Emit("beep");
    }
}
