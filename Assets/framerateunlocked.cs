using UnityEngine;

public class UnlockFramerate : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;           // Ensure VSync is OFF
        Application.targetFrameRate = -1;         // Uncap framerate
    }
}