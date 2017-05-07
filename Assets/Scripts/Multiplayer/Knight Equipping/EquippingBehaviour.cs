using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippingBehaviour : MonoBehaviour {

    public GameObject _weaponPrefabCollection;
    public bool _devMode;

    protected GameObject _leftHand;
    protected GameObject _rightHand;
    protected GameObject _leftElbow;
    protected GameObject _rightElbow;

    private Dictionary<int, GameObject> _weaponsMap;

    protected void Awake() {
        _devMode = false;

        _leftHand = findByName(transform.gameObject, "Left_Hand_3_WeaponSocket");
        _rightHand = findByName(transform.gameObject, "Right_Hand_3_WeaponSocket");

        _leftElbow = findByName(transform.gameObject, "Left_Elbow_WeaponSocket");
        _rightElbow = findByName(transform.gameObject, "Right_Elbow_WeaponSocket");

        _weaponsMap = new Dictionary<int, GameObject>();
        Weapon[] weaponChilds = _weaponPrefabCollection.GetComponentsInChildren<Weapon>();
        foreach (Weapon each in weaponChilds) {
            int itemID = each.itemID;
            GameObject obj = each.gameObject;
            if (_weaponsMap.ContainsKey(itemID)) {
                Weapon weapon = _weaponsMap[itemID].GetComponent<Weapon>();
                Debug.LogError("Duplicated ItemID: {" + weapon.name + " AND " + each.itemName + "}");                    
            } else {
                _weaponsMap.Add(itemID, obj);
            }
        }

        if (!_devMode) {
            //CALL SERVER to obtain weapon itemID
            //use instantiateWeaponByID(itemID);
        }
    }

    protected GameObject attachWeaponToLeft(int weaponID) {
        if (isElbowWeapon(weaponID)) {
            return instantiateWeaponByID(weaponID, "ElbowHandleLeft", null, _leftElbow);
        } else {
            return instantiateWeaponByID(weaponID, "Handle", "HandleLeft", _leftHand);
        }
    }

    protected GameObject attachWeaponToRight(int weaponID) {
        if (isElbowWeapon(weaponID)) {
            return instantiateWeaponByID(weaponID, "ElbowHandleRight", null, _rightElbow);
        } else {
            return instantiateWeaponByID(weaponID, "Handle", "HandleRight", _rightHand);
        }
    }

    private bool isElbowWeapon(int weaponID) {
        GameObject weaponPrefab;
        if (!_weaponsMap.TryGetValue(weaponID, out weaponPrefab)) {
            return false;
        }
        if (weaponPrefab.transform.Find("ElbowHandleLeft") != null) return true;
        if (weaponPrefab.transform.Find("ElbowHandleRight") != null) return true;
        return false;
    }

    

    private GameObject instantiateWeaponByID(int id, string primaryHandle, string secondaryHandle, GameObject handSocket) {
        GameObject weaponPrefab;
        if (!_weaponsMap.TryGetValue(id, out weaponPrefab)) {
            return null;
        }

        Transform handSocketTransf = handSocket.transform;

        Transform weaponInstance = Instantiate(weaponPrefab.transform, handSocketTransf, false);
        Transform selectedHandle = weaponInstance.transform.Find(primaryHandle);
        if (selectedHandle == null) {
            selectedHandle = weaponInstance.transform.Find(secondaryHandle);
        }


        Quaternion diffQuat = Quaternion.Inverse(selectedHandle.rotation) * handSocketTransf.rotation;
        weaponInstance.rotation = weaponInstance.rotation * diffQuat;

        weaponInstance.localPosition = Vector3.zero;
        Vector3 diff = weaponInstance.position - selectedHandle.position;
        weaponInstance.position += diff;

        return weaponInstance.gameObject;
    }

    private static GameObject findByName(GameObject parent, string childName) {
        Transform[] childs = parent.transform.GetComponentsInChildren<Transform>();
        foreach (Transform each in childs) {
            GameObject eachObj = each.gameObject;
            if (eachObj.name == childName) return eachObj;
        }
        return null;
    }
    
}
