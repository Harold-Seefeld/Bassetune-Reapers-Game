﻿using UnityEngine;
using System.Collections;

public class MenuFadeTextIn : MonoBehaviour {

	private CanvasRenderer canvasRenderer;
	private CanvasRenderer[] childrenCanvas;
	
	// Use this for initialization
	void Start () 
	{
		canvasRenderer = GetComponent<CanvasRenderer> ();
		canvasRenderer.SetAlpha (0f);

		childrenCanvas = GetComponentsInChildren<CanvasRenderer> ();

		foreach (CanvasRenderer canvas in childrenCanvas)
		{
			canvas.SetAlpha(0f);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach (CanvasRenderer canvas in childrenCanvas)
		{
			if (canvas.GetAlpha() < 1)
			{
				canvas.SetAlpha(canvas.GetAlpha() + Time.deltaTime * 12);
				break;
			}
		}
	}

}
