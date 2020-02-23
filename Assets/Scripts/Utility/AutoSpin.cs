using UnityEngine;

[AddComponentMenu("Utility/AutoSpin")]
public class AutoSpin : MonoBehaviour
{
	public float x;
	public float y;
	public float z;

	void Update()
	{
		transform.Rotate(new Vector3(x, y, z) * Time.deltaTime);
	}
}
