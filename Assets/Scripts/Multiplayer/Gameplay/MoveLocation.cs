using UnityEngine;
using System.Collections;
using SocketIO;

public class MoveLocation : MonoBehaviour {

	private SocketIOComponent socket;

	// Use this for initialization
	IEnumerator Start () {
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		yield return new WaitForSeconds(2f);
		JSONObject testObject = new JSONObject(JSONObject.Type.OBJECT);
		testObject.AddField("ye", "1");
		socket.Emit("createRoom", testObject);
		yield return new WaitForSeconds(2f);
		socket.Emit("joinRoom", new JSONObject());
		yield return new WaitForSeconds(2f);
		while (true) {
			yield return new WaitForSeconds(1f);
			socket.GetComponent<CharacterLocation>().AddLocation("1", new Vector2(transform.position.x, transform.position.y + Time.time));	socket.GetComponent<CharacterLocation>().AddLocation("1", new Vector2(transform.position.x, transform.position.y + Time.time));
			socket.GetComponent<CharacterLocation>().AddLocation("2", new Vector2(transform.position.x, transform.position.y + Time.time));	socket.GetComponent<CharacterLocation>().AddLocation("1", new Vector2(transform.position.x, transform.position.y + Time.time));
		}	
		
	}
}
