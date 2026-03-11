using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("ตั้งค่าการเดินและมุมกล้อง")]
    public float speed = 5.0f;
    public float mouseSensitivity = 200.0f;
    public Transform playerCamera; 
    
    private CharacterController controller;
    private float xRotation = 0f; 

    private GameObject currentHoveredButton;

    // ตัวแปรเช็คว่าเมาส์เป็นอิสระอยู่ไหม
    public bool isMouseFree = false;

    void Start()
    {
        LockMouse();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- กด ESC เพื่อสลับล็อก/ปลดล็อกเมาส์อย่างเดียว (ให้ PauseManager คุมระบบ UI แทน) ---
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMouseFree = !isMouseFree; 

            if (isMouseFree)
            {
                UnlockMouse(); 
            }
            else
            {
                LockMouse(); 
            }
        }

        // ถ้าปลดล็อกเมาส์อยู่ ให้ข้ามคำสั่งเดินและหันหน้าจอไปเลย (เพื่อไม่ให้จอหันตามเมาส์)
        if (isMouseFree) return;

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
        
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        controller.Move(gravity * Time.deltaTime);

        // --- 3. เช็คการเล็งเป้า (ทำงานตลอดเวลาเพื่อเปลี่ยนสีปุ่ม) ---
        CheckCrosshairHover();

        // --- 4. ระบบเล็งแล้วคลิกตอบคำถาม ---
        if (Input.GetMouseButtonDown(0))
        {
            if (currentHoveredButton != null)
            {
                Button btn = currentHoveredButton.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.Invoke();
                }
            }
        }
    }

    private void CheckCrosshairHover()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = new Vector2(Screen.width / 2f, Screen.height / 2f);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        GameObject hitButton = null;

        foreach (RaycastResult result in results)
        {
            Button btn = result.gameObject.GetComponentInParent<Button>();
            
            if (btn != null && btn.interactable)
            {
                hitButton = btn.gameObject;
                break; 
            }
        }

        if (hitButton != currentHoveredButton)
        {
            if (currentHoveredButton != null)
            {
                ExecuteEvents.Execute(currentHoveredButton, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            }

            if (hitButton != null)
            {
                ExecuteEvents.Execute(hitButton, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            }

            currentHoveredButton = hitButton;
        }
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- ฟังก์ชันนี้เอาไว้ให้ปุ่ม RESUME มาเรียกใช้ เพื่อล็อกเมาส์กลับเข้าที่ ---
    public void ReturnToGame()
    {
        isMouseFree = false;
        LockMouse();
    }
}