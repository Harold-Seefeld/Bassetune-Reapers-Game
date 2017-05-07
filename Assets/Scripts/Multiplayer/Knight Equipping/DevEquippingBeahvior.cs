using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/*
 * Development test extension of EquippingBehavior.
 * For Prod, use directly EquippingBehavior class 
 */

public class DevEquippingBeahvior : EquippingBehaviour {

    public GameObject _weaponSelectionUI;
    public GameObject _knightSelectionUI;
    public bool showHandle = false;

    private int _weaponID;
    private bool _equipLeft;
    private bool _equipRight;
    private bool _runEquipWeapon;

    private Weapon[] _weapons;

    protected new void Awake() {
        base.Awake();
        _devMode = true;
        _weaponID = -1;
        _equipLeft = true;
        _equipRight = false;
        _runEquipWeapon = false;

        _weapons = _weaponPrefabCollection.transform.GetComponentsInChildren<Weapon>();

        Dropdown _weaponDropDown = _weaponSelectionUI.transform.GetComponentInChildren<Dropdown>();
        List<string> options = new List<string>();
        options.Add("-- No Weapon [UP / DOWN] --");
        foreach (Weapon each in _weapons) {
            options.Add(each.itemName);
        }
        _weaponDropDown.AddOptions(options);

        _weaponDropDown.onValueChanged.AddListener((index) => {
            if (index == 0) {
                _weaponID = -1;
                _runEquipWeapon = true;
                return;
            }
            int weaponIndex = index--;
            Weapon selected = _weapons[index];
            _weaponID = selected.itemID;
            _runEquipWeapon = true;
        });


        ToggleGroup toogleGroup = _weaponSelectionUI.transform.GetComponentInChildren<ToggleGroup>();
        Toggle[] toggle = toogleGroup.transform.GetComponentsInChildren<Toggle>();
        Toggle left = toggle[0];
        Toggle right = toggle[1];

        left.onValueChanged.AddListener((enabled) => {
            _equipLeft = enabled;
            _runEquipWeapon = true;
        });
        right.onValueChanged.AddListener((enabled) => {
            _equipRight = enabled;
            _runEquipWeapon = true;
        });


        Toggle knightAnimToggle = _knightSelectionUI.transform.GetComponentInChildren<Toggle>();
        Animator anim = transform.GetComponent<Animator>();
        anim.enabled = knightAnimToggle.isOn;
        knightAnimToggle.onValueChanged.AddListener((enabled) => {
            anim.enabled = enabled;
        });
    }

    void Update() {
        if (!_devMode) return;


        Dropdown _weaponDropDown = _weaponSelectionUI.transform.GetComponentInChildren<Dropdown>();
        ToggleGroup toogleGroup = _weaponSelectionUI.transform.GetComponentInChildren<ToggleGroup>();
        Toggle[] toggle = toogleGroup.transform.GetComponentsInChildren<Toggle>();
        Toggle left = toggle[0];
        Toggle right = toggle[1];

        Toggle knightAnimToggle = _knightSelectionUI.transform.GetComponentInChildren<Toggle>();

        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            int currentValue = _weaponDropDown.value;
            if (currentValue == _weapons.Length + 1) return;
            _weaponDropDown.value = currentValue + 1;
        } else if (Input.GetKeyUp(KeyCode.UpArrow)) {
            int currentValue = _weaponDropDown.value;
            if (currentValue == 0) return;
            _weaponDropDown.value = currentValue - 1;
        } else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            if (!left.isOn) left.isOn = true;
        } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
            if (!right.isOn) right.isOn = true;
        } else if (Input.GetKeyUp(KeyCode.Space)) {
            knightAnimToggle.isOn = !knightAnimToggle.isOn;
        }


        if (!_runEquipWeapon) return;

        if (_leftHand.transform.childCount != 0) {
            Destroy(_leftHand.transform.GetChild(0).gameObject);
        }
        if (_rightHand.transform.childCount != 0) {
            Destroy(_rightHand.transform.GetChild(0).gameObject);
        }
        if (_leftElbow.transform.childCount != 0) {
            Destroy(_leftElbow.transform.GetChild(0).gameObject);
        }
        if (_rightElbow.transform.childCount != 0) {
            Destroy(_rightElbow.transform.GetChild(0).gameObject);
        }

        if (_weaponID != -1) {
            if (_equipLeft) attachWeaponToLeft(_weaponID);
            else if (_equipRight) attachWeaponToRight(_weaponID);
        }
        _runEquipWeapon = false;
    }

    void OnDrawGizmos() {
        if (!showHandle) return;

        GameObject currentSocket = null;
        Transform handle = null;

        if (_equipLeft && _leftHand != null && _leftHand.transform.childCount != 0) {
            currentSocket = _leftHand;
            handle = handle_leftHand(_leftHand);
        } else if (_equipRight && _rightHand != null && _rightHand.transform.childCount != 0) {
            currentSocket = _rightHand;
            handle = handle_rightHand(_rightHand);
        } else if (_equipRight && _rightElbow != null && _rightElbow.transform.childCount != 0) {
            currentSocket = _rightElbow;
            handle = handle_rightElbow(_rightElbow);
        } else if (_equipLeft && _leftElbow != null && _leftElbow.transform.childCount != 0) {
            currentSocket = _leftElbow;
            handle = handle_leftElbow(_leftElbow);
        }

        if (currentSocket == null) return;

        //Transform handleObj = currentSocket.transform.GetChild(0).GetChild(0);
        handle.gameObject.SetActive(true);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(currentSocket.transform.position, currentSocket.transform.forward);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(handle.position, handle.forward);
    }

    private Transform handle_rightElbow(GameObject socket) {
        Transform weapon = socket.transform.GetChild(0);
        return weapon.Find("ElbowHandleRight");
    }

    private Transform handle_leftElbow(GameObject socket) {
        Transform weapon = socket.transform.GetChild(0);
        return weapon.Find("ElbowHandleLeft");
    }

    private Transform handle_leftHand(GameObject socket) {
        Transform weapon = socket.transform.GetChild(0);
        Transform handle = weapon.Find("Handle");
        if (handle != null) return handle;
        else return weapon.Find("HandleLeft");
    }

    private Transform handle_rightHand(GameObject socket) {
        Transform weapon = socket.transform.GetChild(0);
        Transform handle = weapon.Find("Handle");
        if (handle != null) return handle;
        else return weapon.Find("HandleRight");
    }


}
