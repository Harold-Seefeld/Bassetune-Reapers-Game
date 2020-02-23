using UnityEngine;

public class PreMenuLogoTransition : MonoBehaviour
{
    private CanvasRenderer canvasRenderer;
    private bool keyPressed;

    // Use this for initialization
    private void Start()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKeyDown && !keyPressed) keyPressed = true;

        if (keyPressed) canvasRenderer.SetAlpha(canvasRenderer.GetAlpha() - Time.deltaTime / 1f);

        if (canvasRenderer.GetAlpha() <= 0f) enabled = false;
    }
}