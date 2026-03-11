using UnityEngine;
using TMPro; // จำเป็นต้องใช้สำหรับ UI กระดาษโน้ต

public class MapItem : MonoBehaviour 
{
    [Header("--- ระบบประตูคำถาม ---")]
    public QuestionUIManager uiManager; 
    public RiddleData myRiddle; 
    private bool isQuestionBoard = false; 

    [Header("--- ระบบกระดานคำใบ้ (Note) ---")]
    public string myHint; 
    public GameObject noteUI;           
    public TextMeshProUGUI contentText; 
    
    private bool isPlayerInRange = false;
    public bool isCollected = false; 
    private bool isReading = false;   

    // 1. ฟังก์ชันรับข้อมูลสำหรับ "ประตูคำถาม"
    public void SetupQuestion(RiddleData riddleData) 
    {
        myRiddle = riddleData;
        isQuestionBoard = true;
    }

    // 2. ฟังก์ชันรับข้อมูลสำหรับ "กระดานคำใบ้"
    public void SetupHint(string hintText) 
    {
        myHint = hintText;
        isQuestionBoard = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ถ้าเป็นประตู หรือ (เป็นคำใบ้ที่ยังไม่ได้เก็บ) ถึงจะขึ้นให้กด E
            if (isQuestionBoard || (!isQuestionBoard && !isCollected))
            {
                isPlayerInRange = true;
                Debug.Log("กด E เพื่อโต้ตอบ");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        // เมื่อผู้เล่นกดปุ่ม E
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // --- ถ้ากระดานนี้คือ "ประตูคำถาม" ---
            if (isQuestionBoard)
            {
                if (isPlayerInRange && myRiddle != null)
                {
                    if (uiManager != null) 
                    {
                        uiManager.OpenQuestion(myRiddle);
                    }
                    else 
                    {
                        Debug.LogError("ลืมใส่ UIManager ให้กับ MapItem นี้ครับ!");
                    }
                }
            }
            // --- ถ้ากระดานนี้คือ "คำใบ้ (Note)" ---
            else
            {
                // จังหวะที่ 1: เดินมาใกล้ + ยังไม่ได้เก็บ -> กด E เพื่อ "เก็บ"
                if (isPlayerInRange && !isCollected)
                {
                    CollectNote();
                }
                // จังหวะที่ 2: เก็บมาแล้ว + ยังไม่ได้เปิดอ่าน -> กด E เพื่อ "เปิดอ่าน"
                else if (isCollected && !isReading)
                {
                    OpenNote();
                }
                // จังหวะที่ 3: เปิดอ่านอยู่ -> กด E เพื่อ "ปิด"
                else if (isCollected && isReading)
                {
                    CloseNote();
                }
            }
        }
    }

    // --- ฟังก์ชันการทำงานของกระดาษโน้ต ---

    void CollectNote()
    {
        isCollected = true;
        isPlayerInRange = false; 

        // ทำให้สมุดโน้ต 3D ที่พื้นหายไป
        if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false; 
        if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = false; 

        Debug.Log("เก็บโน้ตคำใบ้แล้ว! (กด E เพื่อเปิดอ่าน)");
    }

    void OpenNote()
    {
        isReading = true;
        
        // ดึงข้อความคำใบ้ไปแสดงบน UI
        if (contentText != null) contentText.text = myHint;

        // เปิดหน้าต่าง UI ของโน้ต
        if (noteUI != null) noteUI.SetActive(true); 

        // (ลบคำสั่งปลดล็อกเมาส์ออกแล้ว เพื่อให้ใช้เป้ากลางจอได้ตามปกติ)
        Debug.Log("เปิดอ่านโน้ต");
    }

    void CloseNote()
    {
        isReading = false;
        
        // ปิดหน้าต่าง UI
        if (noteUI != null) noteUI.SetActive(false); 

        Debug.Log("ปิดโน้ตแล้ว");
    }

    // --- ฟังก์ชันสำหรับ NoteResetter ไว้ลบกระดาษโน้ตทิ้ง ---
    public void RemoveFromPlayer()
    {
        // ถ้าเป็นโน้ตที่เก็บมาแล้ว ให้ทำลายทิ้ง
        if (!isQuestionBoard && isCollected)
        {
            if (isReading) 
            {
                CloseNote(); // ถ้าเปิดค้างไว้อยู่ ให้ปิด UI ก่อน
            }
            
            Debug.Log("ทำลายคำใบ้ " + gameObject.name + " ทิ้งแล้ว!");
            Destroy(gameObject); // ลบออกจากเกมถาวร
        }
    }
}