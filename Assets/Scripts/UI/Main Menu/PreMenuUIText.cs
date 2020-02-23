using UnityEngine;

public class PreMenuUIText : MonoBehaviour
{
    private CanvasRenderer canvasRenderer;
    private bool keyPressed;

    public AudioClip keyPressSound;

    // Use this for initialization
    private void Start()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKeyDown && !keyPressed)
        {
            keyPressed = true;

            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = keyPressSound;
            audioSource.Play();
        }

        if (keyPressed) canvasRenderer.SetAlpha(canvasRenderer.GetAlpha() - Time.deltaTime);

        if (canvasRenderer.GetAlpha() <= 0f) enabled = false;
    }
}