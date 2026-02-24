using UnityEngine;
using UnityEngine.SceneManagement; // สำคัญมาก: ต้องมีเพื่อใช้คำสั่งเปลี่ยน Scene

public class EndGame : MonoBehaviour
{
    // ชื่อของ Scene ที่เป็นหน้าเมนูหลัก
    public string mainMenuName = "MainMenu"; 

    void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าผู้เล่นเป็นคนเดินมาชนใช่ไหม
        if (other.CompareTag("Player"))
        {
            // โหลดหน้า Main Menu กลับมา
            SceneManager.LoadScene(mainMenuName);
        }
    }
}