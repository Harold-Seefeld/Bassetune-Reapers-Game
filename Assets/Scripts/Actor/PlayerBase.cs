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

	public GameObject cursor;
	protected NavMeshAgent agent;

	protected float mouseDownTimer = 0f;
	protected bool useDirectMouseControl = false;

    //for debugging purposes
    public GameObject debugLabel;
    public Text debugLabelText;


	void Start () {
		BaseStart ();
	}

	protected void BaseStart(){
		agent = GetComponent<NavMeshAgent> ();
        
        #if UNITY_EDITOR
            debugLabel = Instantiate(debugLabel) as GameObject;
            debugLabelText = debugLabel.GetComponent<Text>();
            debugLabel.transform.SetParent(GameObject.Find("InGameCanvas").transform);
        #else

        #endif
	}

	void Update(){
		BaseUpdate ();
	}

	protected void BaseUpdate () {
		if (Input.GetMouseButtonDown(0)){
			Vector3 destination = ScreenToNavPos(Input.mousePosition);
			agent.SetDestination (destination);
			mouseDownTimer = 0f;
			
			Destroy((GameObject)GameObject.Instantiate(cursor, destination, Quaternion.identity), 0.5f);
		} else if (Input.GetMouseButtonUp(0) && useDirectMouseControl){
			// If it's using direct mouse control and the mouse button no longer down. reset the path
			useDirectMouseControl = false;
			agent.ResetPath();
		} else if (Input.GetMouseButton(0) && mouseDownTimer > 0.25f){
			// Update new path every 0.25 second
			useDirectMouseControl = true;
			Vector3 destination = ScreenToNavPos(Input.mousePosition);

			// Uncomment this code if you want to directly manipulate the move and comment the code below this. not recomended
			// agent.Move((destination - transform.position).normalized * agent.speed * Time.deltaTime);

			agent.SetDestination(destination);
			mouseDownTimer = 0f;	// only calculate path every 0.25f seconds
		}

		Debug.DrawRay (transform.position, agent.velocity);
		mouseDownTimer += Time.deltaTime;
	}

	protected Vector3 ScreenToNavPos(Vector3 pos){
		Ray r = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 100, 1 << 8)){	// 1 << 8 is Terrain layer mask
			return hit.point;
		}
		return transform.position;
	}

    //called from the ui whenever an ability's slot is changed.
    public void OnAbilityAssign(AbilityBase ability, int targetSlot)
    {
        ability.slotAssigned = targetSlot;
    }
}