using UnityEngine;
using System.Collections;
using SocketIO;

public class MapReciever : MonoBehaviour {

	private SocketIOComponent socket;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On(SocketIOEvents.Input.MAP, CreateMap);
	}
	
	// Update is called once per frame
	void CreateMap (SocketIOEvent e) {
		// e.data is an array
		for (int i = 0; i < e.data.Count; i++) {
			// map data fits into following variables for each wall
			// x1, x2, y1,y2
			float x1 = e.data[i].GetField("x1").n;
			float x2 = e.data[i].GetField("x2").n;
			float y1 = e.data[i].GetField("y1").n;
			float y2 = e.data[i].GetField("y2").n;
		}
	}

	// TODO: Implement map parsing + generate visuals
}
