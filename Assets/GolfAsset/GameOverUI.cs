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
        Debug.Log("ปุ่ม Restart ถูกกดแล้ว!"); 

        if (SimpleDatabaseManager.Instance != null)
        {
            // 1. สั่งให้นับเวลาต่อจากเดิม
            SimpleDatabaseManager.Instance.SetTimerRunning(true);
            
            // 2. ถาม Database ว่า "ล่าสุดเล่นค้างไว้ที่ฉากไหน?" 
            // (ดึงค่ามาจาก lastSceneName ที่เราเขียนไว้ใน Manager)
            string sceneToLoad = SimpleDatabaseManager.Instance.GetLastSceneName();
            
            Debug.Log("กำลังกลับไปเกิดใหม่ที่ฉาก: " + sceneToLoad);
            
            // 3. โหลดฉากล่าสุดที่บันทึกไว้
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            // กรณีหา Instance ไม่เจอ ให้กลับไป Scene1 เป็นค่าเริ่มต้น
            Debug.LogWarning("ไม่พบ Database Instance! โหลด Scene1 แทน");
            SceneManager.LoadScene("Scene1");
        }
    }

    // เพิ่มฟังก์ชันสำหรับปุ่มกลับเมนูหลัก (ถ้ามี)
    public void MainMenu()
    {
        if (SimpleDatabaseManager.Instance != null)
        {
            SimpleDatabaseManager.Instance.BackToMenu();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}