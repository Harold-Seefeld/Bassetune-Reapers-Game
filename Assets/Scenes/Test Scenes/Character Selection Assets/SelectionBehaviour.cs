using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBehaviour : MonoBehaviour {

    public static SelectionBehaviour instance;

    public GameObject _currentSelection;

    void Start () {
        instance = this;
	}
	
	public void CheckForSelection () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            GameObject hitObject = hit.transform.root.gameObject;
            if (hitObject == null) return;

            GameObject found = lookUp(hitObject, "Selection");
            if (found) {
                if (_currentSelection) deselectCurrent();
                setAsCurrentSelected(hitObject, found);
            }
        }
	}

    public void SetSelected(GameObject character)
    {
        character = character.transform.root.gameObject;
        GameObject found = lookUp(character, "Selection");
        if (found)
        {
            if (_currentSelection) deselectCurrent();
            setAsCurrentSelected(character, found);
        }
    }

    private void setAsCurrentSelected(GameObject hitObject, GameObject found) {
        _currentSelection = hitObject;
        found.SetActive(true);
    }

    private void deselectCurrent()
    {
        GameObject found = lookUp(_currentSelection, "Selection");
        found.SetActive(false);
    }

    private bool noSelection(GameObject found) {
        return found == null;
    }

    private GameObject lookUp(GameObject parent, string name) {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs) {
            if (t.name == name) {
                return t.gameObject;
            }
        }
        return null;
    }
}
