using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 200.0f;
    public Transform playerCamera; 
    
    // 1. เพิ่มตัวแปรสำหรับอ้างอิง Character Controller
    private CharacterController controller;
    float xRotation = 0f; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        // 2. สั่งให้โค้ดไปดึงคอมโพเนนต์ Character Controller มาใช้งาน
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- 1. การมอง (เหมือนเดิม) ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // --- 2. การเดิน (แก้ไขใหม่) ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // สร้างแรงเคลื่อนที่ตามทิศทางที่ตัวละครหันหน้าไป
        Vector3 move = transform.right * x + transform.forward * z;

        // 3. ใช้คำสั่ง controller.Move แทน transform.Translate
        // คำสั่งนี้จะเช็ก Collision ให้โดยอัตโนมัติ ชนกำแพงแล้วจะหยุดทันที
        controller.Move(move * speed * Time.deltaTime);
        
        // แถม: ใส่แรงโน้มถ่วงแบบง่ายๆ เพื่อให้ตัวละครไม่ลอยเวลาขึ้นบันได
        // (ถ้าไม่มีบรรทัดนี้ เวลาเดินลงบันไดตัวละครจะลอยกลางอากาศครับ)
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        controller.Move(gravity * Time.deltaTime);
    }
}