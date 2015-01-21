using UnityEngine;
using System.Collections;


public class InternetCalls : MonoBehaviour {
	private string loginSite = "http://saphrusservers.atwebpages.com/worb521/login.php";
	private string registerSite = "http://saphrusservers.atwebpages.com/worb521/register.php";
	private string IPSite = "http://saphrusservers.atwebpages.com/worb521/ip.php";
	public static string IPAddress = "";
	public static string registrationStatus = "";
	public static string loginStatus = "";

	public void CheckIP()
	{
		WWW w = new WWW (IPSite);
		StartCoroutine(IP(w));
		Debug.Log (IPAddress);
	}
	
	IEnumerator IP(WWW w)
	{
		yield return w;
		IPAddress = w.text;
	}


	public void DoLogin(string User, string Pass)
	{
		WWWForm www = new WWWForm();
		www.AddField("user",User);
		www.AddField("password",Pass);
		WWW w = new WWW (loginSite, www);
		StartCoroutine(Login(w));
	}

	IEnumerator Login(WWW w)
	{
		yield return w;
		if (w.text == "Login Succeeded") 
		{
			Debug.Log ("Login Complete");
			loginStatus = "Login Succeeded";
		}
		else if (w.text == "Password is incorrect") 
		{
			Debug.Log ("Password was entered incorrectly");
			loginStatus = "Username or Password is invalid.";
		}
		else if (w.text == "User doesn't exist") 
		{
			Debug.Log ("User doesn't exist");
			loginStatus = "Username or Password is invalid.";
		}
		else 
		{
			Debug.Log(w.error);
			loginStatus = "An error occured. Please try again later.";
		}
	}
	
	public void DoRegister(string User, string Pass, string Email)
	{
		WWWForm www = new WWWForm();
		www.AddField("user",User);
		www.AddField("password",Pass);
		www.AddField ("email", Email);
		
		WWW w = new WWW (registerSite, www);
		StartCoroutine(Register(w));
	}

	IEnumerator Register(WWW w)
	{
		yield return w;
		if (w.text == "User has been created!") 
		{
			Debug.Log ("Registration Succeeded");
			registrationStatus = "Registration Completed";
		}
		else if (w.text == "A user with this name already exists, please choose another one!") 
		{
			Debug.Log ("Username has already been taken");
			registrationStatus = "This username already exists!";
		}
		else if (w.text == "A user with this email already exists, please choose another one!") 
		{
			Debug.Log ("Email has already been taken");
			registrationStatus = "This email already exists!";
		}
		else
		{
			Debug.Log(w.error);
			registrationStatus = "An error occured. Please try again later.";
		}
	}
}

