using System;
using UnityEngine;

[AddComponentMenu("Helper/InGameCanvas")]
public class InGameCanvas : MonoBehaviour
{
    [NonSerialized] public AbilityIcon[] abilities;

    private void Awake()
    {
        // Cache reference
        abilities = transform.Find("Skill Panel").GetComponentsInChildren<AbilityIcon>();

        // TODO : Helper for Health, Time and Ally info
    }
}