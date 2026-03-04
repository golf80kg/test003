using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    void Start()
    {
        // บังคับให้เมาส์โชว์และกดได้แน่นอน
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        Debug.Log("ปุ่ม Restart ถูกกดแล้ว!"); // เช็คใน Console ว่าบรรทัดนี้ขึ้นไหม

        if (SimpleDatabaseManager.Instance != null)
        {
            SimpleDatabaseManager.Instance.SetTimerRunning(true);
            Debug.Log("สั่งให้ Database นับเวลาต่อแล้ว");
        }

        // โหลดฉากเกม (เปลี่ยน "Scene1" เป็นชื่อฉากของคุณ)
        SceneManager.LoadScene("Scene1"); 
    }
}