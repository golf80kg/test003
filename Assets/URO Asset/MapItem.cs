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
            // แยกข้อความแจ้งเตือนให้ตรงกับปุ่ม
            if (isQuestionBoard)
            {
                isPlayerInRange = true;
                Debug.Log("กด E เพื่อโต้ตอบกับประตู");
            }
            else if (!isQuestionBoard && !isCollected)
            {
                isPlayerInRange = true;
                Debug.Log("กด Q เพื่อเก็บคำใบ้");
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
        // --- 1. สำหรับ "ประตูคำถาม" (รอรับปุ่ม E) ---
        if (isQuestionBoard && Input.GetKeyDown(KeyCode.E))
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

        // --- 2. สำหรับ "คำใบ้ (Note)" (รอรับปุ่ม Q) ---
        if (!isQuestionBoard && Input.GetKeyDown(KeyCode.Q))
        {
            // จังหวะที่ 1: เดินมาใกล้ + ยังไม่ได้เก็บ -> กด Q เพื่อ "เก็บ"
            if (isPlayerInRange && !isCollected)
            {
                CollectNote();
            }
            // จังหวะที่ 2: เก็บมาแล้ว + ยังไม่ได้เปิดอ่าน -> กด Q เพื่อ "เปิดอ่าน"
            else if (isCollected && !isReading)
            {
                OpenNote();
            }
            // จังหวะที่ 3: เปิดอ่านอยู่ -> กด Q เพื่อ "ปิด"
            else if (isCollected && isReading)
            {
                CloseNote();
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

        // แก้ข้อความแจ้งเตือนให้ตรงกับปุ่ม Q
        Debug.Log("เก็บโน้ตคำใบ้แล้ว! (กด Q เพื่อเปิดอ่าน)");
    }

    void OpenNote()
    {
        isReading = true;
        
        // ดึงข้อความคำใบ้ไปแสดงบน UI
        if (contentText != null) contentText.text = myHint;

        // เปิดหน้าต่าง UI ของโน้ต
        if (noteUI != null) noteUI.SetActive(true); 

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