using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {

	public InternetCalls calls;
	public string User;
	public string Password;
	public GUIStyle GUINormal;
	public GUIStyle GUITitles;
	public GUIStyle GUIErrors;
	public string IPAddress;
	
	private void OnGUI(){
		GUILayout.BeginArea (new Rect ((Screen.width / 2) - 350, (Screen.height / 2) - 40, 256, 24), GUIContent.none, "box");
		GUILayout.Label ("Login", GUITitles);
		GUILayout.EndArea ();
		
		// Login
		
		GUILayout.BeginArea (new Rect ((Screen.width / 2) - 414, (Screen.height / 2) - 10, 384, 142), GUIContent.none, "box");
		GUILayout.BeginVertical();
		GUILayout.Label ("Username", GUINormal);
		User = GUILayout.TextField (User);
		GUILayout.EndVertical ();
		GUILayout.BeginVertical();
		GUILayout.Label ("Password", GUINormal);
		Password = GUILayout.PasswordField (Password, '*', 24);
		GUILayout.EndVertical ();
		if (GUILayout.Button ("Login")){
			calls.DoLogin(User, Password);
		}
		GUILayout.Label (InternetCalls.loginStatus + " ", GUIErrors);	
		GUILayout.EndArea ();
	}
}