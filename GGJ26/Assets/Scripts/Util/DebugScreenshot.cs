using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugScreenshot : MonoBehaviour
{
    public InputAction screenshotInput;
    int scaleValue = 1;

    private void Start()
    {
        if(scaleValue < 1)
        {
            scaleValue = 1;
        }
        screenshotInput.Enable();
        screenshotInput.performed += ctx => TakeScreenshot();
    }
    public void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png", scaleValue);
    }
}
