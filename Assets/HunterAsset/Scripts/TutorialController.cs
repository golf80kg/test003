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
            if (pages.Length == 0) return;
            currentIndex = (currentIndex + 1) % pages.Length;
            UpdatePages();
            Debug.Log("เปลี่ยนเป็นหน้า: " + (currentIndex + 1));
        }
    }

    // ฟังก์ชันสำหรับ LessonBoard เรียกใช้เพื่อรีเซ็ตหน้า 0 ทุกครั้งที่เปิดใช้งาน
    public void ResetTutorial()
    {
        currentIndex = 0;
        UpdatePages();
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