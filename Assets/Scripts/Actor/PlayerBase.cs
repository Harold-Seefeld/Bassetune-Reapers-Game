﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * This is base Actor for all controllable actor.
 * If you overriding Unity function from this class, don't forget to call Base[FunctionToOverride] before writing your scripts.
 * Remember base.[FunctionToOverride] won't work due to unity reflection architectures.
 */

[AddComponentMenu("Actor/PlayerBase")]
public class PlayerBase : MonoBehaviour {
	
	public GameObject movecursor;
	public GameObject targetCursor;
	public InGameCanvas inGameCanvas = null;
	
	protected NavMeshAgent agent;
	
	protected float mouseDownTimer = 0f;
	protected bool useDirectMouseControl = false;

	protected Transform target;
	
	// for debugging purposes
	public GameObject debugLabel;
	protected Text debugLabelText;
	
	
	void Start () {
		BaseStart ();
	}
	
	protected void BaseStart(){
		agent = GetComponent<NavMeshAgent> ();
		targetCursor = Instantiate (targetCursor, Vector3.zero, targetCursor.transform.rotation) as GameObject;
		targetCursor.SetActive (false);
		
		#if UNITY_EDITOR
		debugLabel = Instantiate(debugLabel) as GameObject;
		debugLabelText = debugLabel.GetComponent<Text>();
		debugLabel.transform.SetParent(GameObject.Find("InGameCanvas").transform);
		#else
		debugLabel.SetActive(false);
		debugLabelText.text = "";
		#endif
	}
	
	void Update(){
		BaseUpdate ();
	}
	
	protected void BaseUpdate () {
		if (Input.GetMouseButtonDown(0)){
			// Go to position after receiving single click input
			Vector3 destination = Vector3.zero;
			if (ScreenToNavPos(Input.mousePosition, ref destination, ref target)){
				agent.SetDestination (destination);
				mouseDownTimer = 0f;
				
				Destroy((GameObject)GameObject.Instantiate(movecursor, destination, Quaternion.identity), 0.5f);
				
				#if UNITY_EDITOR
				debugLabelText.text = "Move To " + destination;
				#endif
			}
		} else if (Input.GetMouseButtonUp(0) && useDirectMouseControl){
			// Stop following cursor since mouse already up
			// If it's using direct mouse control and the mouse button no longer down. reset the path
			useDirectMouseControl = false;
			agent.ResetPath();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Unfollow Cursor";
			#endif
		} else if (Input.GetMouseButton(0) && mouseDownTimer > 0.25f){
			// Start following cursor
			// Update new path every 0.25 second
			useDirectMouseControl = true;
			Vector3 destination = Vector3.zero;
			if (ScreenToNavPos(Input.mousePosition, ref destination, ref target)){
				// Uncomment this code if you want to directly manipulate the move and comment the code below this. not recomended
				// agent.Move((destination - transform.position).normalized * agent.speed * Time.deltaTime);
				
				agent.SetDestination(destination);
				mouseDownTimer = 0f;	// only calculate path every 0.25f seconds
				
				#if UNITY_EDITOR
				debugLabelText.text = "Follow Cursor";
				#endif
			}	
		}

		// Update Target Cursor
		if (target){
			targetCursor.SetActive(true);
			targetCursor.transform.position = target.transform.position;
		}
		
		Debug.DrawRay (transform.position, agent.velocity);
		mouseDownTimer += Time.deltaTime;
		
		#if UNITY_EDITOR
		// Update Debug Label position
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, gameObject.transform.position);
		debugLabelText.rectTransform.anchoredPosition = screenPoint - debugLabelText.canvas.GetComponent<RectTransform> ().sizeDelta / 2f;
		#endif
	}
	
	protected bool ScreenToNavPos(Vector3 pos, ref Vector3 position, ref Transform target){
		Ray r = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 100, 1 << 8 | 1 << 9)){	// 1 << 8 is Terrain layer mask
			// Check wheter it's cast to other actor
			if ((hit.transform.tag == "Boss" || 
			    hit.transform.tag == "Knight" || 
			    hit.transform.tag == "Creature") &&
			    hit.transform.GetInstanceID() != transform.GetInstanceID()){
				target = hit.transform;
			}
			position = hit.point;
			return true;
		}
		return false;
	}
}