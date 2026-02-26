using UnityEngine;
using TMPro;

public class MapItem : MonoBehaviour 
{
    [Header("ข้อมูลที่กระดานนี้ถืออยู่ (รับจาก Database)")]
    public string myContent;

    [Header("ตั้งค่า UI หน้าจอโน้ต")]
    public GameObject noteUI;           
    public TextMeshProUGUI contentText; 

    private bool isPlayerInRange = false;
    public bool isCollected = false; // เช็คว่าเก็บโน้ตมาไว้กับตัวหรือยัง
    private bool isReading = false;   // เช็คว่ากำลังเปิดอ่านอยู่ไหม

    // 1. รับข้อมูลจาก DatabaseManager
    public void Setup(string content) 
    {
        myContent = content;
    }

    // 2. เช็คระยะผู้เล่นเดินเข้า-ออก
    private void OnTriggerEnter(Collider other)
    {
        // ถ้ายังไม่ได้เก็บ และผู้เล่นเดินมาใกล้
        if (!isCollected && other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("กด E เพื่อเก็บโน้ต");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    // 3. รอรับคำสั่งกดปุ่ม E
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

    // --- ฟังก์ชันการทำงานต่างๆ ---

    void CollectNote()
    {
        isCollected = true;
        isPlayerInRange = false; 

        // ทำให้สมุดโน้ต 3D ที่พื้นหายไป (เสมือนเก็บเข้ากระเป๋าแล้ว)
        // **เราใช้วิธีปิดการมองเห็นและการชนแทนการ Destroy เพื่อให้สคริปต์ยังคอยรับปุ่ม E ได้อยู่**
        if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false; 
        if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = false; 

        Debug.Log("เก็บโน้ตแล้ว! (สามารถกด E เพื่อเปิดอ่านได้ตลอดเวลา)");
    }

    void OpenNote()
    {
        isReading = true;
        
        // ดึงข้อความไปแสดงบน UI
        if (contentText != null) contentText.text = myContent;

        // เปิดหน้าต่าง UI
        noteUI.SetActive(true); 
        
        // ปลดล็อกเมาส์ให้ผู้เล่น
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("เปิดอ่านโน้ต");
    }

    void CloseNote()
    {
        isReading = false;
        
        // ปิดหน้าต่าง UI
        noteUI.SetActive(false); 

        // ล็อกเมาส์คืนเพื่อให้เดินต่อได้
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("ปิดโน้ตแล้ว");
    }

    // --- ฟังก์ชันสำหรับให้บล็อคล่องหนเรียกใช้เพื่อทำลายไอเทมทิ้ง ---
    public void RemoveFromPlayer()
    {
        // ถ้าสมุดโน้ตถูกเก็บเข้ากระเป๋ามาแล้ว ค่อยทำลายทิ้ง
        if (isCollected)
        {
            // เช็คเผื่อผู้เล่นกำลังเปิดหน้าต่างกระดาษโน้ตค้างไว้ตอนเดินชนบล็อก
            if (isReading) 
            {
                CloseNote(); // สั่งให้ปิดหน้าต่าง UI ลงก่อน
            }
            
            Debug.Log("ทำลายคำใบ้ " + gameObject.name + " ทิ้งแล้ว!");
            Destroy(gameObject); // ทำลายทิ้งถาวร! (จะกด E เปิดไม่ได้อีก)
        }
    }
}