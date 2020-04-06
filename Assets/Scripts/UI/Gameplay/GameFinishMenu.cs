using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinishMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitToMainMenu()
    {
        StartCoroutine(_ExitToMainMenu());
    }

    IEnumerator _ExitToMainMenu()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(1);
    }
}
