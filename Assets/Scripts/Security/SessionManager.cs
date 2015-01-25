using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SessionManager : MonoBehaviour {

	private string loginSite = "http://ec2-54-173-162-102.compute-1.amazonaws.com/login";
	private string registerSite = "http://ec2-54-173-162-102.compute-1.amazonaws.com/register";

	// Reference
	public Text usernameText;
	public InputField passwordText;
	public Text nicknameText;
	public Text emailText;
	public Text bDayYText;
	public Text bDayMText;
	public Text bDayDText;
	public Text notification;
	public Text textHelp1;
	public Text textHelp2;
	public Text textHelp3;
	public Text textHelp4;
	
	
	public Canvas current;
	public Canvas next;

	// About Session : 0 means not in session, positive = auth success, negative failed to auth
	private string SessionId = "0";
	
	public string GetSession()
	{
		return SessionId;
	}

	public void Start()
	{
		DontDestroyOnLoad(this);
	}

	public void CreateSession()
	{
		if (emailText.transform.parent.gameObject.activeInHierarchy)
		{
			emailText.transform.parent.gameObject.SetActive(false);
			nicknameText.transform.parent.gameObject.SetActive(false);
			bDayYText.transform.parent.parent.gameObject.SetActive(false);
			textHelp1.transform.gameObject.SetActive(false);
			textHelp2.transform.gameObject.SetActive(false);
			textHelp3.transform.gameObject.SetActive(false);
			textHelp4.transform.gameObject.SetActive(false);
			
			return;
		}
		if (string.IsNullOrEmpty(usernameText.text) || string.IsNullOrEmpty(passwordText.text)){
			notification.text = "Username or Password can't be blank";
		} 
		else 
		{
			StartCoroutine(_CreateSession (usernameText.text, passwordText.text));
		}
	}

	public void CreateSession(string username, string password)
	{
		StartCoroutine(_CreateSession (username, password));
	}	

	public void DestroySession(){
		StartCoroutine (_DestroySession ());
	}

	IEnumerator _CreateSession(string username, string password) 
	{
		yield return new WaitForEndOfFrame();
		notification.text = "Contacting Server !";
	    
		// Begin authentication
		DoLogin(usernameText.text, passwordText.text);
	}

	IEnumerator _DestroySession(){
		// 
		SessionId = "0";
		yield return 0;
	}
	
	void DoLogin(string User, string Pass) 
	{

		WWWForm www = new WWWForm();
		www.AddField("username",User);
		www.AddField("password",Pass);
		WWW w = new WWW (loginSite, www.data);
		StartCoroutine(Login(w));
	}
	
	IEnumerator Login(WWW w) 
	{
		yield return w;
		if (w.text == "Unsuccessful Login." || w.text == "error") 
		{
			notification.text = "Unsuccessful Login.";
		}
		else if (w.error != null) 
		{
			Debug.Log(w.error);
			notification.text = "An error occured. Please try again later.";
		}
		else 
		{
			notification.text = "Successful Login.";
			SessionId = w.text;
			yield return new WaitForSeconds (1);
			current.enabled = false;
			next.enabled = true;
		}
	}

	public void Register () 
	{
		if (!emailText.transform.parent.gameObject.activeInHierarchy)
		{
			emailText.transform.parent.gameObject.SetActive(true);
			nicknameText.transform.parent.gameObject.SetActive(true);
			bDayYText.transform.parent.parent.gameObject.SetActive(true);
			textHelp1.transform.gameObject.SetActive(true);
			textHelp2.transform.gameObject.SetActive(true);
			textHelp3.transform.gameObject.SetActive(true);
			textHelp4.transform.gameObject.SetActive(true);

			return;
		}

			DoRegister(usernameText.text, nicknameText.text, passwordText.text, emailText.text, bDayYText.text, bDayMText.text, bDayDText.text);
	}
	
	public void DoRegister(string User, string Nickname, string Pass, string Email, string BdayY, string BdayM, string BdayD) {
		WWWForm www = new WWWForm();
		www.AddField("username",User);
		www.AddField("nickname",Nickname);
		www.AddField("email", Email);
		www.AddField("password", Pass);
		www.AddField("BdayY", BdayY);
		www.AddField("BdayM", BdayM);
		www.AddField("BdayD", BdayD);
		WWW w = new WWW (registerSite, www.data);
		StartCoroutine(Register(w));
	}
	
	IEnumerator Register(WWW w) 
	{
		yield return w;
		Debug.Log (w.text);
		if (w.text == "User has been created!") 
		{
			Debug.Log ("Registration Succeeded");
			notification.text = "Registration Completed";
		}
		if (w.text == "A user with this name already exists, please choose another one!") 
		{
			Debug.Log ("Username has already been taken");
			notification.text = "This username already exists!";
		}
		if (w.text == "A user with this email already exists, please choose another one!") 
		{
			Debug.Log ("Email has already been taken");
			notification.text = "This email already exists!";
		}
		if (w.error != null){
			Debug.Log(w.error);
			notification.text = "An error occured. Please try again later.";
		}
	}
}
