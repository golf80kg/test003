using UnityEngine;

public class LessonBoard : MonoBehaviour
{
    public GameObject tutorialPanel; // ลาก TutorialPanel (ที่มีสคริปต์ TutorialController) มาใส่

    private void Start()
    {
        // เริ่มเกมให้มั่นใจว่าปิดอยู่
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialPanel != null)
            {
                // 1. เปิดหน้าจอ
                tutorialPanel.SetActive(true);

                // 2. สั่งให้ TutorialController รีเซ็ตกลับไปหน้าแรก (หน้า 0)
                TutorialController controller = tutorialPanel.GetComponent<TutorialController>();
                if (controller != null)
                {
                    controller.ResetTutorial(); 
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // เมื่อเดินออก ให้ปิดหน้าจอทันที เพื่อให้พร้อมสำหรับครั้งต่อไป
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(false);
            }
        }
    }
}