using UnityEngine;
using System.Collections;

public class NoiseMaker : MonoBehaviour 
{
	public GameObject PlatformType; 
	public int Size = 50; 
	public float Scale = 7.00f; 
	public bool EnableHeight = true; 
	public float ScaleModifier = 5f; 
	public bool Move = false; 
	public float OffSetHeight = 2.5f;

	void Start () 
	{ 
		for(int X = 0; X < Size; X++) 
		{ 
			for(int Z = 0; Z < Size; Z++) 
			{ 
				GameObject C = Instantiate(PlatformType, new Vector3(X, 0, Z), Quaternion.identity) as GameObject; C.transform.parent = transform; 
			} 
		} 
	} 

	void Update () 
	{ 
		UpdateTransform (); 
	} 

	void UpdateTransform() 
	{ 
		foreach (Transform Child in transform) 
		{ 
			float Height = Mathf.PerlinNoise(Child.transform.position.x/Scale, Child.transform.position.z/Scale); 
			SetMatColor(Child, Height); 
			if (EnableHeight == true) 
			{ 
				ApplyHeight(Child, Height); 
			} 
		} 
	} 

	void SetMatColor(Transform Child, float Height) 
	{ 
		Child.GetComponent<Renderer>().material.color = new Color (Height, Height, Height, Height); 
	} 

	void ApplyHeight(Transform Child, float Height) 
	{ 
		int YValue = Mathf.RoundToInt (Height * ScaleModifier); 
		Vector3 NewVec3 = new Vector3 (Child.transform.position.x, YValue, Child.transform.position.z); 
		Child.transform.position = NewVec3; 
	} 

	float Seed(float Scale, float ScaleModifier)
	{
		int count = 1;
		Random.seed = 89789;
		while(count < Size)
		{
			this.RoomGeneration((Random.value/count));
			count += 1;
		}
	
	
	}

	int RoomGeneration(int value)
	{
		//researching the room creation with perlin-noise

		return Random.value;
	}

}
