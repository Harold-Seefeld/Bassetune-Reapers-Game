using UnityEngine;

public class MenuFadeIn : MonoBehaviour
{
    private CanvasRenderer canvasRenderer;

    // Use this for initialization
    private void Start()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();
        canvasRenderer.SetAlpha(0.1f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (canvasRenderer.GetAlpha() < 1f)
            canvasRenderer.SetAlpha(canvasRenderer.GetAlpha() + Time.deltaTime);
        else if (canvasRenderer.GetAlpha() >= 1f) enabled = false;
    }
}