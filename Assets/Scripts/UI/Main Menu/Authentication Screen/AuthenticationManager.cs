using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Collections;

public class AuthenticationManager : MonoBehaviour
{
    // Connections to the face server
    [SerializeField]
    private string server = "bassetune.com";
    private string loginSite = "";
    private string registerSite = "";

    // References to UI fields
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

    // Reference to Client data
    private ClientData clientData;

    public void Start()
    {
        loginSite = server + "/login";
        registerSite = server + "/register";

        clientData = new GameObject().AddComponent<ClientData>();
    }

    private bool IsValidEmail(string strIn)
    {
        // Return true if strIn is in valid e-mail format.
        return Regex.IsMatch(strIn, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" +
            "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
    }

    private bool IsValidUsername(string strIn)
    {
        // Return true if strIn is in valid
        return Regex.IsMatch(strIn, "^[a-zA-Z0-9]{2,15}$");
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
        if (string.IsNullOrEmpty(usernameText.text) || string.IsNullOrEmpty(passwordText.text))
        {
            notification.text = "Username or Password can't be blank";
        }
        else
        {
            CreateSession(usernameText.text, passwordText.text);
        }
    }

    void CreateSession(string username, string password)
    {
        // Begin authentication
        WWWForm www = new WWWForm();
        www.AddField("username", username);
        www.AddField("password", password);
        WWW w = new WWW(loginSite, www.data);
        StartCoroutine(Login(w));

        // Update notification text
        notification.text = "Contacting Server...";
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
            // Set clients session
            clientData.SetSession(w.text);
            // Change scene to main menu which should be the next scene in the scene order
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }

    public void Register()
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
        else if (nicknameText.text == "")
        {
            notification.text = "Please specify a nickname.";
            return;
        }
        else if (bDayDText.text == "" || bDayMText.text == "" || bDayYText.text == "" || Convert.ToInt16(bDayYText.text) < 1920 ||
            bDayDText.text.Length != 2 || bDayMText.text.Length != 2 || bDayYText.text.Length != 4 || bDayDText.text.Contains("-") ||
            bDayYText.text.Contains("-") || bDayMText.text.Contains("-") || Convert.ToInt16(bDayMText.text) > 12 || Convert.ToInt16(bDayDText.text) > 31)
        {
            notification.text = "Please specify a valid birthdate.";
            return;
        }
        else if (!IsValidEmail(emailText.text))
        {
            notification.text = "Please specify a valid email.";
            return;
        }
        else if (usernameText.text.Length < 3)
        {
            notification.text = "Usernames must be a minimum of 3 characters long.";
            return;
        }
        else if (passwordText.text.Length < 6)
        {
            notification.text = "Passwords must be a minimum of 3 characters long.";
            return;
        }

        DoRegister(usernameText.text, nicknameText.text, passwordText.text, emailText.text, bDayYText.text, bDayMText.text, bDayDText.text);
    }

    public void DoRegister(string User, string Nickname, string Pass, string Email, string BdayY, string BdayM, string BdayD)
    {
        WWWForm www = new WWWForm();
        www.AddField("username", User);
        www.AddField("nickname", Nickname);
        www.AddField("email", Email);
        www.AddField("password", Pass);
        www.AddField("BdayY", BdayY);
        www.AddField("BdayM", BdayM);
        www.AddField("BdayD", BdayD);
        WWW w = new WWW(registerSite, www.data);
        StartCoroutine(Register(w));
    }

    IEnumerator Register(WWW w)
    {
        yield return w;
        if (w.text == "Registration Succeeded.")
        {
            notification.text = "Registration Complete. A verification email has been sent to the email address provided.";
        }
        if (w.text.Contains("username_UNIQUE"))
        {
            notification.text = "This username already exists. Please choose another one.";
        }
        if (w.text.Contains("email_UNIQUE"))
        {
            notification.text = "This email is already in use.";
        }
        if (w.error != null)
        {
            Debug.Log(w.error);
            notification.text = "An error occured. Please try again later.";
        }
    }
}
