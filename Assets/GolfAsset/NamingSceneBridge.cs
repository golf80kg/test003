using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NamingSceneBridge : MonoBehaviour
{
    public TMP_InputField localNameInput;
    public string cutsceneSceneName = "CutScene"; 

    public void OnClickSaveAndStart()
    {
        if (localNameInput != null && string.IsNullOrWhiteSpace(localNameInput.text))
        {
            Debug.LogWarning("กรุณากรอกชื่อก่อนเริ่มเกม!");
            return;
        }

        if (SimpleDatabaseManager.Instance != null)
        {
            SimpleDatabaseManager.Instance.StartGameRecord();
            SceneManager.LoadScene(cutsceneSceneName);
        }
        else
        {
            Debug.LogError("หา SimpleDatabaseManager ไม่เจอ!");
        }
    }
}