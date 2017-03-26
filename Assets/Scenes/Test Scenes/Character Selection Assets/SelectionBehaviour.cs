using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBehaviour : MonoBehaviour {

    private GameObject _currentSelection;

    void Start () {
        _currentSelection = null;
	}
	
	void Update () {
        if (!Input.GetMouseButtonUp(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            GameObject hitObject = hit.transform.root.gameObject;
            if (hitObject == null) return;

            GameObject found = lookUp(hitObject, "Selection");
            if (noSelection(found)) {
                deselectCurrent();
            }  else if (isSelectingTwice(found)) {
                //deselectCurrent();
            } else if (isSelectingAnOther(found)) {
                deselectCurrent();
                setAsCurrentSelected(hitObject, found);
            } else {
                setAsCurrentSelected(hitObject, found);
            }
        } else {
            deselectCurrent();
        }
	}

    private void setAsCurrentSelected(GameObject hitObject, GameObject found) {
        _currentSelection = found;
        _currentSelection.SetActive(true);
        UseCaller.selectedCharacter = hitObject.GetComponent<CharacterData>();
    }

    private bool isSelectingAnOther(GameObject found) {
        if (_currentSelection == null) return false;
        if (_currentSelection == found) return false;
        return true;
    }

    private bool isSelectingTwice(GameObject found) {
        if (_currentSelection == null) return false;
        if (_currentSelection != found) return false;
        return true;
    }

    private bool noSelection(GameObject found) {
        return found == null;
    }

    private void deselectCurrent() {
        if (_currentSelection == null) return;
        _currentSelection.SetActive(false);
        _currentSelection = null;
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
