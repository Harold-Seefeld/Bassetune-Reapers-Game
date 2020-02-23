using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    private int currentImageIndex;
    private Image imageObject;

    public Sprite[] Images;
    private float lastImageSetTime;

    [Range(0.1f, 1000f)] public float SecondsBetweenImages;

    // Use this for initialization
    private void Start()
    {
        imageObject = gameObject.GetComponent<Image>();
        SetImage(0);
    }

    // Update is called once per frame
    private void Update()
    {
        var secondsSinceLastImate = Time.time - lastImageSetTime;
        if (secondsSinceLastImate >= SecondsBetweenImages) IncrementImage();
    }

    private void SetImage(int index)
    {
        currentImageIndex = index % Images.Length;
        imageObject.sprite = Images[currentImageIndex];
        lastImageSetTime = Time.time;
    }

    private void IncrementImage()
    {
        currentImageIndex = (currentImageIndex + 1) % Images.Length;
        SetImage(currentImageIndex);
    }
}