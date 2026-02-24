using UnityEngine;
using UnityEngine.SceneManagement; // สำหรับการโหลดฉากหรือออกจากเกม

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // ลาก PauseMenu Panel มาใส่ช่องนี้
    private bool isPaused = false;

    void Update()
    {
        // ตรวจสอบการกดปุ่ม Escape (หรือปุ่มที่ต้องการ)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // ปิดหน้าจอ UI
        Time.timeScale = 1f;          // ให้เวลาในเกมเดินต่อปกติ
        isPaused = false;

        // ล็อกเมาส์กลับคืน (ถ้าเกมเป็นแนว First Person)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // เปิดหน้าจอ UI
        Time.timeScale = 0f;         // หยุดเวลาในเกมทั้งหมด (ตัวละคร/ผี จะนิ่งสนิท)
        isPaused = true;

        // ปลดล็อกเมาส์ให้กดปุ่มได้
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

public void ExitGame()
    {
        // ต้องคืนค่าเวลาให้กลับเป็นปกติ (1f) ก่อนโหลดฉากใหม่ 
        // ไม่อย่างนั้นหน้าเมนูหลักอาจจะค้างหรือทำงานผิดปกติได้
        Time.timeScale = 1f; 

        // สั่งให้โหลด Scene ลำดับที่ 0 (ซึ่งเราตั้งเป็นหน้า MainMenu ใน Build Settings)
        SceneManager.LoadScene(0); 
    }
}