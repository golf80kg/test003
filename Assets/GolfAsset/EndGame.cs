using UnityEngine;
using UnityEngine.SceneManagement; 

public class EndGame : MonoBehaviour
{
    // ชื่อของ Scene ที่เป็นหน้าเมนูหลัก
    public string mainMenuName = "MainMenu"; 

    void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าผู้เล่นเป็นคนเดินมาชนใช่ไหม
        if (other.CompareTag("Player"))
        {
            // 1. สั่งให้ Database Manager หยุดจับเวลาและบันทึกข้อมูลลง JSON ทันที
            if (SimpleDatabaseManager.Instance != null)
            {
                SimpleDatabaseManager.Instance.FinishAndSaveGame();
                Debug.Log("บันทึกข้อมูลชื่อและเวลาลง JSON เรียบร้อย!");
            }

            // 2. โหลดหน้า Main Menu กลับมา
            SceneManager.LoadScene(mainMenuName);
        }
    }
}