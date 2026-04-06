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
    public float maxInteractDistance = 4.0f; // 
    
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
        //การคำนวนความไวของเมาส์
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);//หมุนเมาส์ซ้ายขวา
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);//หมุนแค่กล้องไม่ใช่ตัว
        }

        float x = Input.GetAxis("Horizontal");//เดินซ้ายขวา
        float z = Input.GetAxis("Vertical");//เดินหน้าหลัง

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);//ขยับตามทิศ
        
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        controller.Move(gravity * Time.deltaTime);

        CheckCrosshairHover();
       
        if (Input.GetMouseButtonDown(0))//ตรวจว่ากดคลิ้กซ้ายอยู่มั้ย
        {
            if (currentHoveredButton != null)//เช็คว่าCrosshairชี้ปุ่มอยู่มั้ย
            {
                Button btn = currentHoveredButton.GetComponent<Button>();//ดึงปุ่มออกจากGameobject เหลือแต่ Objectเพื่อสั่งให้ทำงานได้
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

        List<RaycastResult> results = new List<RaycastResult>();//ยิง Raycastกลางจอผ่านเพื่อหาว่ามี UI  อะไรอยู่ตรงนั้น
        EventSystem.current.RaycastAll(pointerData, results);

        GameObject hitButton = null;//เก็บตัวแปรปุ่มที่เจอ

        foreach (RaycastResult result in results)// วนลูปผ่านทุกสิ่งที่ Raycast ยิงไปโดน
        {
            // เช็คว่าระยะเลเซอร์ที่ยิงไปโดน ไกลเกินระยะที่ตั้งไว้หรือไม่ 
            if (result.distance > maxInteractDistance) continue; 

            Button btn = result.gameObject.GetComponentInParent<Button>();
            
            if (btn != null && btn.interactable)//เช็คว่าเจอปุ่มแล้วกดได้มั้ย
            {
                hitButton = btn.gameObject;//เก็บปุ่มที่เจอไว้ 
                break; 
            }
        }

        if (hitButton != currentHoveredButton)
        {
            if (currentHoveredButton != null)
            {//เลื่อนเมาส์ออกให้ปุ่มเหมือนเดิม
                ExecuteEvents.Execute(currentHoveredButton, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            }

            if (hitButton != null)
            {//เลื่อนเมาส์เข้าให้มีhighlight
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