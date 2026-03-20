using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour
{
    public GameObject mapUI;       // ลาก MapPanel มาใส่ที่นี่
    public bool hasMap = false;    // เช็คว่าเก็บแผนที่หรือยัง
    private bool isMapOpen = false;

    void Update()
    {
        // ถ้าเก็บแผนที่แล้ว และกดปุ่ม M
        if (hasMap && Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    void ToggleMap()
    {
        isMapOpen = !isMapOpen;
        mapUI.SetActive(isMapOpen);

        // หยุดเวลาเกมขณะเปิดแผนที่ (Optional)
        Time.timeScale = isMapOpen ? 0f : 1f; 
        
        // จัดการเรื่อง Cursor (ถ้าเป็นเกม First Person)
        Cursor.visible = isMapOpen;
        Cursor.lockState = isMapOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // ฟังก์ชันสำหรับเรียกใช้เมื่อกด E ที่ไอเทม
    public void PickUpMap()
    {
        hasMap = true;
        Debug.Log("Map Collected! Press M to open.");
    }
}