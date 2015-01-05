using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class LoginInfo : MonoBehaviour {

	public Login login;
	private string Username;
	private string usernameCheck;
	private string randomString;

	void Awake () 
	{
		DontDestroyOnLoad (this);

		Username = login.User;
		usernameCheck = MD5String.Md5Sum (Username, randomString);
		StartCoroutine(checkStringValid ());
	}

	private string RandomString(int size, bool lowerCase)
	{
		StringBuilder builder = new StringBuilder();
		System.Random random = new System.Random();
		char ch;
		for(int i =1; i < size+1; i++)
		{
			ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26* random.NextDouble()+65)));
			builder.Append(ch);
		}
		if(lowerCase)
			return builder.ToString().ToLower();
		else
			return builder.ToString();
	}

	// Checks if username has been modified
	IEnumerator checkStringValid()
	{
		usernameCheck = MD5String.Md5Sum (Username, randomString);
		if (MD5String.Md5Sum(Username, randomString) != usernameCheck)
		{
			Application.Quit();
		}

		yield return new WaitForSeconds (2);
		StartCoroutine(checkStringValid ());
		randomString = RandomString (10, false);
	}

}
