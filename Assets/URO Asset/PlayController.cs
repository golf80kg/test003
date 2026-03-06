using UnityEngine;
using UnityEngine.EventSystems; // เพิ่มเข้ามาสำหรับเช็คการคลิก UI
using System.Collections.Generic; // เพิ่มเข้ามาสำหรับ List
using UnityEngine.UI; // เพิ่มเข้ามาเพื่อเรียกใช้ Button

public class PlayerController : MonoBehaviour
{
    [Header("ตั้งค่าการเดินและมุมกล้อง")]
    public float speed = 5.0f;
    public float mouseSensitivity = 200.0f;
    public Transform playerCamera; 
    
    private CharacterController controller;
    private float xRotation = 0f; 

    void Start()
    {
        // ล็อกเป้าเมาส์ให้อยู่ตรงกลางจอ ไม่ให้หลุดขอบ (ซ่อนเมาส์จริง)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // ดึงคอมโพเนนต์ Character Controller มาใช้งาน
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- 1. การมอง (หมุนกล้อง) ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // --- 2. การเดิน (Movement) ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        
        // แรงโน้มถ่วงจำลอง
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        controller.Move(gravity * Time.deltaTime);

        // --- 3. ระบบเล็งแล้วคลิกตอบคำถาม ---
        // เมื่อผู้เล่นกดคลิกเมาส์ซ้าย (0 คือเมาส์ซ้าย)
        if (Input.GetMouseButtonDown(0))
        {
            ShootCrosshairRaycast();
        }
    }

    // ฟังก์ชันยิงเลเซอร์จากเป้ากลางจอไปกดปุ่ม UI (แบบ World Space)
    private void ShootCrosshairRaycast()
    {
        // สร้างตัวแทนจุดคลิกไว้ที่ "กึ่งกลางหน้าจอ" เป๊ะๆ
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = new Vector2(Screen.width / 2f, Screen.height / 2f);

        List<RaycastResult> results = new List<RaycastResult>();
        
        // สั่งให้ระบบ UI ยิงเลเซอร์เช็คว่ากลางจอทับโดนอะไรบ้าง
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            // ตรวจสอบว่าวัตถุที่โดนเล็งอยู่ มีคอมโพเนนต์ Button หรือไม่
            Button btn = result.gameObject.GetComponentInParent<Button>();
            
            if (btn != null)
            {
                btn.onClick.Invoke(); // สั่งให้ปุ่มทำงานเหมือนเราเอาเมาส์ไปคลิกมันจริงๆ
                Debug.Log("คลิกโดนปุ่ม: " + btn.name);
                break; // โดนปุ่มเดียวแล้วหยุดเลย
            }
        }
    }
}