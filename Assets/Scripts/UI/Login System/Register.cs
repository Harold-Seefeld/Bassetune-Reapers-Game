using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour {

	public InternetCalls calls;
	public string Username;
	public string Password;
	public string Email;
	public string ConfirmPassword;
	public GUIStyle GUINormal;
	public GUIStyle GUITitle;
	public GUIStyle GUIErrors;
	public GUIStyle GUIButton;
	public string IPAddress;
	private bool IsValidEmail(string strIn){return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");}
	
	private void OnGUI(){
		/*
		 * WARNING : PLEASE DON'T USE OUTDATED AND EXPENSIVE UNITY GUI FUNCTION - SURYA
		 */
		
		GUILayout.BeginArea (new Rect ((Screen.width / 2) + 94, (Screen.height / 2) - 40, 256, 24), GUIContent.none, "box");
		GUILayout.Label ("Register", GUITitle);
		GUILayout.EndArea ();
		
		// Register
		
		GUILayout.BeginArea (new Rect ((Screen.width / 2) + 30, (Screen.height / 2) - 10, 384, 256), GUIContent.none, "box");
		GUILayout.BeginVertical();
		GUILayout.Label ("Username", GUINormal);
		Username = GUILayout.TextField (Username);
		GUILayout.EndVertical ();

		GUILayout.BeginVertical();
		GUILayout.Label ("Email", GUINormal);
		Email = GUILayout.TextField (Email);
		GUILayout.EndVertical ();

		GUILayout.BeginVertical();
		GUILayout.Label ("Password", GUINormal);
		Password = GUILayout.PasswordField (Password, "*"[0], 24);
		GUILayout.EndVertical ();

		GUILayout.BeginVertical();
		GUILayout.Label ("Confirm Password", GUINormal);
		ConfirmPassword = GUILayout.PasswordField (ConfirmPassword, "*"[0], 24);
		GUILayout.EndVertical ();

		if (Username.Length > 3){
			if (IsValidEmail(Email)){
				if (Password.Length < 6) {
					GUILayout.Button ("Register");
					GUILayout.Label ("Password must be 6-24 characters long.", GUIErrors);
				}

				else if (Password.Length >=6) {
					if (Password == ConfirmPassword) {
						if (GUILayout.Button ("Register")) {
							calls.DoRegister (Username, Password, Email);
					}

					}else if (Password != ConfirmPassword) {
						GUILayout.Button ("Register");
						GUILayout.Label ("Passwords don't match.", GUIErrors);
					}
				}

			}else if (!IsValidEmail(Email)){
				GUILayout.Button ("Register");
				GUILayout.Label ("Please enter a valid email.", GUIErrors);
			}

		}else if (Username.Length <= 3) {
			GUILayout.Button ("Register");
			GUILayout.Label ("Username must be 4-24 characters long.", GUIErrors);
		}

		GUILayout.Label (InternetCalls.registrationStatus, GUIErrors);
		GUILayout.EndArea ();

	}
}
