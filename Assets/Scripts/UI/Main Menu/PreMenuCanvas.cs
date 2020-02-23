using UnityEngine;

public class PreMenuCanvas : MonoBehaviour
{
    private CanvasRenderer canvasRenderer;
    public GameObject characterMenu;
    private bool keyPressed;

    public GameObject menu;

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

        if (canvasRenderer.GetAlpha() <= -0.3f)
        {
            menu.SetActive(true);
            characterMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}