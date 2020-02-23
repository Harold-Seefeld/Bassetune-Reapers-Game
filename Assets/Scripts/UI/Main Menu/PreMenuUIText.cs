using UnityEngine;

public class PreMenuUIText : MonoBehaviour
{

    private CanvasRenderer canvasRenderer;
    private bool keyPressed;

    public AudioClip keyPressSound;

    // Use this for initialization
    void Start()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !keyPressed)
        {
            keyPressed = true;

            AudioSource audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = keyPressSound;
            audioSource.Play();
        }

        if (keyPressed)
        {
            canvasRenderer.SetAlpha(canvasRenderer.GetAlpha() - Time.deltaTime);
        }

        if (canvasRenderer.GetAlpha() <= 0f)
        {
            this.enabled = false;
        }
    }
}
