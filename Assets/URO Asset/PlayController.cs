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

    // --- เพิ่มตัวแปรสำหรับจำว่าตอนนี้เป้าเล็ง "ทับปุ่มไหนอยู่" ---
    private GameObject currentHoveredButton;

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

        // --- 3. เช็คการเล็งเป้า (ทำงานตลอดเวลาเพื่อเปลี่ยนสีปุ่ม) ---
        CheckCrosshairHover();

        // --- 4. ระบบเล็งแล้วคลิกตอบคำถาม ---
        if (Input.GetMouseButtonDown(0))
        {
            // ถ้าคลิกซ้ายในขณะที่เป้ากำลังเล็งปุ่มอยู่พอดี
            if (currentHoveredButton != null)
            {
                Button btn = currentHoveredButton.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.Invoke(); // สั่งให้ปุ่มทำงาน
                    Debug.Log("คลิกโดนปุ่ม: " + btn.name);
                }
            }
        }
    }

    // ฟังก์ชันยิงเลเซอร์ตรวจสอบว่าเป้าทับปุ่มอยู่ไหม (เอาไว้ทำสี Highlight)
    private void CheckCrosshairHover()
    {
        // สร้างตัวแทนจุดคลิกไว้ที่ "กึ่งกลางหน้าจอ" เป๊ะๆ
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = new Vector2(Screen.width / 2f, Screen.height / 2f);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        GameObject hitButton = null;

        // ค้นหาว่าเลเซอร์ชนกับปุ่มไหม
        foreach (RaycastResult result in results)
        {
            Button btn = result.gameObject.GetComponentInParent<Button>();
            
            // เช็คว่าเป็นปุ่ม และปุ่มนั้นยังกดได้อยู่ (interactable)
            if (btn != null && btn.interactable)
            {
                hitButton = btn.gameObject;
                break; // เจอตัวแรกที่โดนแล้วหยุดหาเลย
            }
        }

        // ถ้าเป้าเล็งขยับไปโดนปุ่มใหม่ (หรือเลื่อนออกจากปุ่มเดิม)
        if (hitButton != currentHoveredButton)
        {
            // 1. สั่งให้ปุ่มเก่า (ที่เลิกเล็งแล้ว) กลับเป็นสี Normal ปกติ
            if (currentHoveredButton != null)
            {
                ExecuteEvents.Execute(currentHoveredButton, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            }

            // 2. สั่งให้ปุ่มใหม่ (ที่เพิ่งเล็งโดน) เปลี่ยนเป็นสี Highlighted
            if (hitButton != null)
            {
                ExecuteEvents.Execute(hitButton, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            }

            // อัปเดตความจำใหม่ ว่าตอนนี้เป้ากำลังทับปุ่มไหนอยู่
            currentHoveredButton = hitButton;
        }
    }
}