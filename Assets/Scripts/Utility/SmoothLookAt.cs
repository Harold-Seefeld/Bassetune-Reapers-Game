using UnityEngine;
using System.Collections;

/*
 * Original Version : https://gist.github.com/tracend/893624
 */

public class SmoothLookAt : MonoBehaviour {
	public Transform target = null;
	public float damping = 6f;
	public bool smooth = true;

	void Start () {
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
	
	void LateUpdate () {
		if (target){
			if (smooth){
				// Look at and damping the rotation
				Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
			} else {
				// Use internal function
				transform.LookAt(target);
			}
		}
	}
}
