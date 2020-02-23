using System.Threading;
using SocketIO;
using UnityEngine;

public class SocketIOTest : MonoBehaviour
{
    private SocketIOComponent socket;

    // Use this for initialization
    private void Start()
    {
        var go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.Connect();

        Thread.Sleep(1000);

        socket.On("boop", TestBoop);
        socket.Emit("beep");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void TestBoop(SocketIOEvent e)
    {
        Debug.Log("boop received");
        socket.Emit("beep");
    }
}