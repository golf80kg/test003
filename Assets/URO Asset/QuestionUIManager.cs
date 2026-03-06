using UnityEngine;
using TMPro; // ใช้ TextMeshPro สำหรับตัวหนังสือ
using UnityEngine.UI; // ใช้จัดการ Button

public class QuestionUIManager : MonoBehaviour
{
    [Header("ส่วนแสดงผล UI")]
    public GameObject questionPanel; // หน้าต่าง Panel รวม
    public TextMeshProUGUI questionText; // ข้อความคำถาม

    [Header("ปุ่มตัวเลือกทั้ง 4")]
    public Button[] choiceButtons; 
    public TextMeshProUGUI[] choiceTexts; // ข้อความบนปุ่ม (ลาก Text ของปุ่มมาใส่)

    private int correctIndex; // ตัวแปรเก็บว่าข้อนี้ ตัวเลือกไหนคือคำตอบที่ถูก

    void Start()
    {
        // ปิดหน้า UI ไว้ก่อนตอนเริ่มเกม
        questionPanel.SetActive(false);
    }

    // ฟังก์ชันนี้เอาไว้รับข้อมูล (RiddleData) มาแสดงบนหน้าจอ
    public void OpenQuestion(RiddleData riddle)
    {
        questionPanel.SetActive(true); // เปิดหน้า UI
        
        questionText.text = riddle.text; // แสดงคำถาม
        correctIndex = riddle.correctAnswerIndex; // จำไว้ว่าข้อไหนถูก (0, 1, 2, หรือ 3)

        // เอาข้อความจาก JSON มาใส่ในปุ่มทั้ง 4
        for (int i = 0; i < 4; i++)
        {
            if (i < riddle.choices.Length)
            {
                choiceTexts[i].text = riddle.choices[i];
            }
        }

        // ปลดล็อกเมาส์ให้ผู้เล่นขยับเมาส์ไปกดปุ่มได้
       // Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    // ฟังก์ชันนี้จะผูกกับปุ่ม เมื่อผู้เล่นกดปุ่มจะส่งหมายเลข (0-3) มาเช็ค
    public void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == correctIndex)
        {
            Debug.Log("ตอบถูก! ผ่านได้");
            // TODO: ใส่คำสั่งเปิดประตู หรือให้รางวัลผู้เล่นตรงนี้

            CloseQuestion(); // ตอบถูกแล้วปิดหน้าต่าง
        }
        else
        {
            Debug.Log("ตอบผิด! ลองใหม่นะ");
            // TODO: ใส่เสียงตอบผิด หรือลดเลือดตรงนี้
        }
    }

    public void CloseQuestion()
    {
        questionPanel.SetActive(false); // ปิดหน้า UI

        // ล็อกเมาส์กลับไปตรงกลางเพื่อให้ผู้เล่นหมุนมุมกล้องเดินต่อได้
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}