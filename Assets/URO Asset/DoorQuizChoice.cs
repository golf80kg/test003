using UnityEngine;
using TMPro;

public class DoorQuizChoice : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject quizPanel;      // หน้าต่างคำถาม
    public int correctButtonID = 1;   // **เฉลย: ปุ่มไหนถูก? (ใส่เลข 1, 2, 3 หรือ 4)**

    [Header("Door Settings")]
    public Transform doorHinge;       // บานพับประตู
    public float openAngle = 90f;
    public float speed = 2.0f;
    
    [Header("Interact Settings")]
    public GameObject interactText;   // ป้าย 3D "Press E"

    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Quaternion startRot;
    private Quaternion endRot;

    void Start()
    {
        if (doorHinge != null)
        {
            startRot = doorHinge.localRotation;
            endRot = Quaternion.Euler(0, openAngle, 0) * startRot;
        }
        
        // ซ่อน UI ตอนเริ่ม
        if (quizPanel != null) quizPanel.SetActive(false);
        if (interactText != null) interactText.SetActive(false);
    }

    void Update()
    {
        // 1. กด E เพื่อเปิดหน้าต่าง
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Q) && !isOpen && !quizPanel.activeSelf)
        {
            OpenQuiz();
        }

        // 2. หมุนประตู
        if (isOpen && doorHinge != null)
        {
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, endRot, Time.deltaTime * speed);
        }
    }

    void OpenQuiz()
    {
        quizPanel.SetActive(true);
        if (interactText != null) interactText.SetActive(false);
        
        // ปลดเมาส์ให้กดปุ่มได้
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // *** ฟังก์ชันเช็คคำตอบ (เอาไปผูกกับปุ่ม) ***
    // เราจะส่งเลข 1, 2, 3, 4 เข้ามาเช็ค
    public void SelectAnswer(int buttonID)
    {
        if (buttonID == correctButtonID)
        {
            // ตอบถูก!
            Debug.Log("ถูกต้องนะคร้าบ!");
            isOpen = true;
            CloseQuiz();
        }
        else
        {
            // ตอบผิด!
            Debug.Log("ผิดครับ! ลองใหม่");
            // (ถ้าอยากให้ตอบผิดแล้วปิดหน้าต่างเลย ก็เรียก CloseQuiz() ตรงนี้ได้)
        }
    }

    void CloseQuiz()
    {
        quizPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // ล็อกเมาส์กลับ
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isPlayerNear = true;
            if (interactText != null) interactText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (interactText != null) interactText.SetActive(false);
            CloseQuiz();
        }
    }
}