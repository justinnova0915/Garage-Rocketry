using UnityEngine;
using UnityEngine.UI; // ✅ Using Unity's legacy UI Text

public class VRDebugOverlay : MonoBehaviour
{
    public Text debugText; // ✅ Legacy UI Text Component
    private string logMessages = "";

    void Awake()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // ✅ Only display normal logs, ignore errors and warnings
        if (type == LogType.Log)
        {
            logMessages = logString + "\n" + logMessages;
            if (logMessages.Length > 1000) // ✅ Limit message size
            {
                logMessages = logMessages.Substring(0, 1000);
            }
            debugText.text = logMessages; // ✅ Update the UI text
        }
    }
}
