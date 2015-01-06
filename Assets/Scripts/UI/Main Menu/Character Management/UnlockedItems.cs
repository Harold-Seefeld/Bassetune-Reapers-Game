using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UnlockedItems : MonoBehaviour {

	public string[] unlockedItems;

	[SerializeField]
	private string invisibleString = "<color=#ffffffff>N/A</color>";
	
	// Use this for initialization
	private void Start () 
	{
		UpdateItems();
	}
	
	// Update the item list
	public void UpdateItems () 
	{
		Text[] textList = GetComponentsInChildren<Text>() as Text[];

		// Set all text invisible
		for (int i = 0; i < textList.Length; i++)
		{
			if (textList[i].transform.parent == transform)
				textList[i].text = invisibleString;
		}

		// Set the item text
		int il = 0;
		for (int i = 0; i < unlockedItems.Length; i++)
		{
			if (textList[il].transform.parent == transform)
			{
				textList[il].text = unlockedItems[i];
				textList[il].gameObject.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
			} else {
				i--;
			}
			il++;
		}

		// Disable non-active buttons
		for (int i = 0; i < textList.Length; i++)
		{
			if (textList[i].text == invisibleString)
			{
				textList[i].gameObject.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
			}
		}
	}
}
