using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject[] pages; 
    public GameObject tutorialPanel; 
    private int currentIndex = 0;

    void Start()
    {
        // เริ่มเกมให้ซ่อนไว้ก่อน และเตรียมหน้าแรก
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        UpdatePages();
    }

    void Update()
    {
        // ถ้าหน้าจอไม่เปิด ไม่ต้องทำงาน
        if (tutorialPanel == null || !tutorialPanel.activeSelf) return;

        // คลิกเมาส์ซ้ายเพื่อเปลี่ยนหน้า (วนลูป)
        if (Input.GetMouseButtonDown(0)) 
        {
            NextPage();
        }

        // กด Enter เพื่อปิดเอง (ถ้าต้องการปิดก่อนดูครบ)
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            SkipTutorial();
        }
    }

    // ฟังก์ชันสำหรับ LessonBoard เรียกใช้เพื่อรีเซ็ตหน้า 0 ทุกครั้งที่เดินเข้า
    public void ResetTutorial()
    {
        currentIndex = 0;
        UpdatePages();
    }

    public void NextPage()
    {
        if (pages.Length == 0) return;

        // ใช้การหารเอาเศษ (Modulo) เพื่อให้ Index วนกลับมาที่ 0 เมื่อเกินจำนวนหน้า
        // เช่น มี 3 หน้า: (0+1)%3 = 1, (1+1)%3 = 2, (2+1)%3 = 0 (วนกลับมาหน้าแรก)
        currentIndex = (currentIndex + 1) % pages.Length;
        
        UpdatePages();
        Debug.Log("เปลี่ยนเป็นหน้า: " + (currentIndex + 1));
    }

    public void SkipTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    void UpdatePages()
    {
        if (pages == null || pages.Length == 0) return;

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
            {
                // เปิดเฉพาะหน้าปัจจุบัน ปิดหน้าที่เหลือ
                pages[i].SetActive(i == currentIndex);
            }
        }
    }
}