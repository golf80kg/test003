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

    [Header("--- ตั้งค่าระยะการกดปุ่ม UI ---")]
    public float maxInteractDistance = 4.0f; // <--- ปรับระยะห่างที่ยอมให้คลิกปุ่มได้ตรงนี้ (หน่วยเป็นเมตร)
    
    private CharacterController controller;
    private float xRotation = 0f; 

    private GameObject currentHoveredButton;
    public bool isMouseFree = false;

    void Start()
    {
        LockMouse();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
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

        if (isMouseFree) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        controller.Move(gravity * Time.deltaTime);

        CheckCrosshairHover();

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
            // <--- จุดที่เพิ่มใหม่: เช็คว่าระยะเลเซอร์ที่ยิงไปโดน ไกลเกินระยะที่ตั้งไว้หรือไม่ --->
            if (result.distance > maxInteractDistance) continue; // ถ้าไกลเกินไป ข้ามไปเลย (ไม่ให้กด)

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

    public void ReturnToGame()
    {
        isMouseFree = false;
        LockMouse();
    }
}