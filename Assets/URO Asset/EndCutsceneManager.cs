using UnityEngine;
using UnityEngine.Playables; 
using UnityEngine.SceneManagement;

public class EndCutsceneManager : MonoBehaviour
{
    private PlayableDirector timeline;

    [Header("ชื่อไฟล์หน้าเมนูหลัก (พิมพ์ให้เป๊ะ)")]
    public string mainMenuSceneName = "MainMenu"; 

    void Start()
    {
        // 1. ระบบจับคู่อัตโนมัติ (ดึง PlayableDirector ที่อยู่ใน Object เดียวกันมาใช้)
        timeline = GetComponent<PlayableDirector>();
        
        if (timeline != null)
        {
            // สั่งว่าถ้า Timeline เล่นจบ ให้เรียกฟังก์ชัน GoToMainMenu
            timeline.stopped += GoToMainMenu;
        }
    }

    void Update()
    {
        // 2. ระบบกดข้ามคัทซีน (กด Escape หรือ Spacebar ก็ได้)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu(timeline);
        }
    }

    // ฟังก์ชันนี้จะทำงานอัตโนมัติเมื่อคัทซีนเล่นจบ หรือเมื่อผู้เล่นกดข้าม
    void GoToMainMenu(PlayableDirector director)
    {
        Debug.Log("คัทซีนจบแล้ว (หรือถูกกดข้าม)! กำลังวาร์ปกลับเมนูหลัก...");

        // 🚨 สำคัญมาก! ต้องปลดล็อกเมาส์คืนให้ผู้เล่น ไม่งั้นจะคลิกเมนูไม่ได้
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // สั่งโหลดหน้าเมนูหลัก
        SceneManager.LoadScene(mainMenuSceneName);
    }
}