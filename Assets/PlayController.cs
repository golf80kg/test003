using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 200.0f;
    public Transform playerCamera; // เพิ่มตัวแปรสำหรับคุมกล้อง

    float xRotation = 0f; // ตัวแปรเก็บมุมก้มเงย

    void Start()
    {
        // สั่งล็อกเมาส์ให้อยู่กลางจอ (จะได้ไม่หลุดเฟรมเวลาหมุนเยอะๆ)
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- 1. การมอง (Mouse Look) ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // หมุนตัวซ้าย-ขวา (หมุนที่ตัว Capsule)
        transform.Rotate(Vector3.up * mouseX);

        // คำนวณการก้ม-เงย
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ล็อกไม่ให้คอหักหมุนครบรอบ

        // สั่งหมุนที่ "กล้อง" โดยตรง
        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // --- 2. การเดิน (Movement) ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }
}