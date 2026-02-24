using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        // เพิ่ม 2 บรรทัดนี้เพื่อให้เมาส์โผล่มาคลิกปุ่มได้ครับ
        Cursor.lockState = CursorLockMode.None; // ปลดล็อคเมาส์
        Cursor.visible = true;                // แสดงตัวเมาส์
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void ExitGame()
    {
        Debug.Log("ปิดเกมแล้ว!");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        #else
            Application.Quit(); 
        #endif
    }
}