using UnityEngine;

public class NoteSystem : MonoBehaviour
{
    public GameObject noteUI;     
    
    private bool isPlayerNear = false;
    private bool isReading = false;  // เช็กว่ากำลังเปิดหน้าจออ่านอยู่หรือไม่

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // จังหวะที่ 1: ถ้าอยู่ใกล้ และยังไม่ได้เปิดอ่าน (กดครั้งแรกเพื่อเก็บและอ่านทันที)
            if (isPlayerNear && !isReading)
            {
                PickUpAndRead();
            }
            // จังหวะที่ 2: ถ้าหน้าจอเปิดอ่านอยู่ (กดครั้งที่สองเพื่อปิด)
            else if (isReading)
            {
                CloseNote();
            }
        }
    }

    void PickUpAndRead()
    {
        isReading = true;
        
        // 1. แสดง UI โน้ตขึ้นมาทันที
        noteUI.SetActive(true); 
        
        // 2. ปลดล็อกเมาส์ให้ผู้เล่น
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 3. ทำให้ตัวโน้ต 3D ที่พื้นหายไป (เสมือนเก็บมาแล้ว)
        // ใช้การปิด Renderer และ Collider เพื่อให้สคริปต์ยังทำงานต่อได้
        if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false; 
        if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = false; 

        Debug.Log("เก็บโน้ตและเปิดอ่านทันที");
    }

    void CloseNote()
    {
        isReading = false;
        noteUI.SetActive(false); // ปิดหน้าจอ UI

        // ล็อกเมาส์คืนเพื่อให้ตัวละครหมุนมุมกล้องและเดินได้ปกติ
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // (เลือกได้): ถ้าอยากให้โน้ตหายไปถาวรหลังอ่านจบ ให้ใช้คำสั่งนี้
        // Destroy(gameObject); 
        
        Debug.Log("ปิดโน้ตแล้ว");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = false;
    }
}