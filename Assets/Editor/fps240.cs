using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [Header("Frame Rate Settings")]
    [SerializeField] private int targetFrameRate = 240;
    [SerializeField] private bool disableVSync = true;
    
    void Awake()
    {
        SetFrameRate();
    }
    
    void Start()
    {
        // Apply settings again in Start to ensure they stick
        SetFrameRate();
    }
    
    private void SetFrameRate()
    {
        // Disable VSync to allow higher frame rates
        if (disableVSync)
        {
            QualitySettings.vSyncCount = 0;
        }
        
        // Set target frame rate
        Application.targetFrameRate = targetFrameRate;
        
        Debug.Log($"Frame rate set to: {targetFrameRate} FPS, VSync: {(disableVSync ? "Disabled" : "Enabled")}");
    }
    
    // Optional: Method to change frame rate at runtime
    public void ChangeFrameRate(int newFrameRate)
    {
        targetFrameRate = newFrameRate;
        Application.targetFrameRate = targetFrameRate;
        Debug.Log($"Frame rate changed to: {targetFrameRate} FPS");
    }
    
    // Optional: Method to toggle VSync
    public void ToggleVSync()
    {
        disableVSync = !disableVSync;
        QualitySettings.vSyncCount = disableVSync ? 0 : 1;
        Debug.Log($"VSync: {(disableVSync ? "Disabled" : "Enabled")}");
    }
}