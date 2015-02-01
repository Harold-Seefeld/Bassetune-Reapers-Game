using UnityEngine;
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

	protected int currentAbility;

	// Used for target reference cache
	protected Transform castTarget;
	protected Vector3 castPosition;

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
		// Control logic
		bool smartcast = Input.GetButton ("SmartCast");
		if (smartcast){
			OnSmarCast();
		} else if (Input.GetMouseButtonDown(0)){
			OnMoveTo();
		} else if (Input.GetMouseButtonUp(0) && useDirectMouseControl){
			OnFollowCursorEnd();
		} else if (Input.GetMouseButton(0) && mouseDownTimer > 0.25f){
			OnFollowCursorBegin();
		}

		if (castTarget || smartcast){
			currentAbility = OnCastHotkey(castTarget, castPosition);
			targetCursor.SetActive(true);
			targetCursor.transform.position = castTarget ? castTarget.transform.position : castPosition + new Vector3(0, 0.01f);
		} else {
			targetCursor.SetActive(false);
		}
		
		mouseDownTimer += Time.deltaTime;

		// Debug only
		Debug.DrawRay (transform.position, agent.velocity);
		#if UNITY_EDITOR
		// Update Debug Label position
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, gameObject.transform.position);
		debugLabelText.rectTransform.anchoredPosition = screenPoint - debugLabelText.canvas.GetComponent<RectTransform> ().sizeDelta / 2f;
		#endif
	}
	
	protected bool ScreenToNavPos(Vector3 pos, ref Vector3 position, ref Transform target){
		Ray r = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 100, 1 << 8 | 1 << 9)){	// Terrain and Character layer mask
			target = null;
			// Check wheter it's cast to other actor
			if ((hit.transform.tag == "Boss" || 
			    hit.transform.tag == "Knight" || 
			    hit.transform.tag == "Creature") &&
			    hit.transform.GetInstanceID() != transform.GetInstanceID()){
				target = hit.transform;
				position = hit.transform.position;
			} else if (hit.transform.GetInstanceID() == transform.GetInstanceID()){
				position = hit.transform.position;
				return false;
			} else {
				position = hit.point;
			}
			return true;
		}
		return false;
	}

	protected virtual void OnSmarCast(){
		// Update target and cursor position
		ScreenToNavPos(Input.mousePosition, ref castPosition, ref castTarget);
		// Reset path
		agent.ResetPath();
		// Rotate actor toward cursor
		Vector3 direction = (castPosition - transform.position).normalized;
		if (direction != Vector3.zero){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5);
		}
		#if UNITY_EDITOR
		debugLabelText.text = "SmartCast Mode";
		#endif
	}

	protected virtual void OnMoveTo(){
		// Go to position if receive valid input
		if (ScreenToNavPos(Input.mousePosition, ref castPosition, ref castTarget)){
			agent.SetDestination (castPosition);
			mouseDownTimer = 0f;
			
			Destroy((GameObject)GameObject.Instantiate(movecursor, castPosition, Quaternion.identity), 0.5f);
			
			#if UNITY_EDITOR
			debugLabelText.text = "Move To " + castPosition;
			#endif
		}
	}

	protected virtual void OnFollowCursorBegin(){
		// Start following cursor
		// Update new path every 0.25 second
		useDirectMouseControl = true;
		if (ScreenToNavPos(Input.mousePosition, ref castPosition, ref castTarget)){
			// Uncomment this code if you want to directly manipulate the move and comment the code below this. not recomended
			// agent.Move((destination - transform.position).normalized * agent.speed * Time.deltaTime);
			
			agent.SetDestination(castPosition);
			mouseDownTimer = 0f;	// only calculate path every 0.25f seconds
			
			#if UNITY_EDITOR
			debugLabelText.text = "Follow Cursor";
			#endif
		}	
	}

	protected virtual void OnFollowCursorEnd(){
		// Stop following cursor since mouse already up
		// If it's using direct mouse control and the mouse button no longer down. reset the path
		useDirectMouseControl = false;
		agent.ResetPath();
		
		#if UNITY_EDITOR
		debugLabelText.text = "Unfollow Cursor";
		#endif
	}
	
	// Override this function to provide different keybind setup
	protected virtual int OnCastHotkey(Transform target, Vector3 position){ return -1; }
}