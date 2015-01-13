using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SessionManager : MonoBehaviour {
	// Reference
	public Text username;
	public Text password;
	public Text notification;

	// Dummy Dev only, should be replaced by UI/Canvas manager
	public Canvas current;
	public Canvas next;

	// About Session : 0 means not in session, positive = auth success, negative failed to auth
	private int SessionId = 0;
	
	public int GetSession(){
		return SessionId;
	}

	public void CreateSession(){
		if (string.IsNullOrEmpty(username.text) || string.IsNullOrEmpty(password.text)){
			notification.text = "Username or Password can't be blank";
		} else {
			StartCoroutine(_CreateSession (username.text, password.text));
		}
	}

	public void CreateSession(string username, string password){
		StartCoroutine(_CreateSession (username, password));
	}	

	public void DestroySession(){
		StartCoroutine (_DestroySession ());
	}

	IEnumerator _CreateSession(string username, string password){
		yield return new WaitForEndOfFrame();

		notification.text = "Contacting Server !";
		// Create web form that will POST the login data
		WWWForm form = new WWWForm ();
		form.AddField ("username", username);
		form.AddField ("password", password);

		// Begin authentification
		// TODO : Server Auth and Session Manager
		WWW auth = new WWW ("http://www.mocky.io/v2/54b39e6e3575eb9509febaf3", form);

		// Wait until server respond
		yield return auth;

		if (!string.IsNullOrEmpty(auth.error)){
			notification.text = "Error : " + auth.error;
		} else {
			notification.text = "Login Success !";
			Debug.Log("Server Response : " + auth.text);
			int.TryParse(auth.text, out SessionId);
		}

		yield return new WaitForSeconds (1);
		current.enabled = false;
		next.enabled = true;
	}

	IEnumerator _DestroySession(){
		// 
		SessionId = 0;
		yield return 0;
	}
}
