using UnityEngine;
using System.Collections;

public class ClientData : MonoBehaviour
{
    public string username;
    public string nickname;

    private string sessionID;
    private string gameVersion;

    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetSession(string session)
    {
        sessionID = session;
    }

    public string GetSession()
    {
        return sessionID;
    }

    public void SetVersion(string version)
    {
        gameVersion = version;
    }

    public string GetVersion()
    {
        return gameVersion;
    }

}
