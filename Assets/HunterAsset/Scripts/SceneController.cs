using UnityEngine;
using UnityEngine.SceneManagement; // ต้องใส่ตัวนี้ถึงจะเปลี่ยนฉากได้

public class SceneController : MonoBehaviour
{
    // ฟังก์ชันสำหรับไปหน้า Volume (ใส่ที่ปุ่ม Setting)
    public void GoToVolumeScene()
    {
        SceneManager.LoadScene("Volume"); // ใส่ชื่อ Scene ของคุณในเครื่องหมายคำพูด
    }

    // ฟังก์ชันสำหรับกลับหน้า Menu (ใส่ที่ปุ่ม Back)
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // ใส่ชื่อ Scene หลักของคุณ
    }
}