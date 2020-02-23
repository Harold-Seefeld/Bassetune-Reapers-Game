using UnityEngine;

public class PreMenuCanvas : MonoBehaviour
{

    public GameObject menu;
    public GameObject characterMenu;

    private CanvasRenderer canvasRenderer;
    private bool keyPressed;

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
        }

        if (keyPressed)
        {
            canvasRenderer.SetAlpha(canvasRenderer.GetAlpha() - Time.deltaTime / 1f);
        }

        if (canvasRenderer.GetAlpha() <= -0.3f)
        {
            menu.SetActive(true);
            characterMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
