using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Slideshow : MonoBehaviour {
	public Sprite[] Images;
	public float SecondsBetweenImages;

	private int currentImageIndex;
	private float lastImageSetTime;
	private Image imageObject;

	// Use this for initialization
	void Start () {
		imageObject = gameObject.GetComponent<Image> ();
		SetImage (0);
	}
	
	// Update is called once per frame
	void Update () {
		float secondsSinceLastImate = Time.time - lastImageSetTime;
		if (secondsSinceLastImate >= SecondsBetweenImages) {
			IncrementImage();
		}
	}

	private void SetImage(int index) {
		currentImageIndex = (index) % (Images.Length);
		imageObject.sprite = Images [currentImageIndex];
		lastImageSetTime = Time.time;
	}
	private void IncrementImage() {
		currentImageIndex = (currentImageIndex + 1) % (Images.Length);
		SetImage (currentImageIndex);
	}
}