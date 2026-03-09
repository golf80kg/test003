using UnityEngine;

public class DoorController1 : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 45f; 
    public float smoothSpeed = 3f;
    
    [Header("Settings")]
    public Transform player; // ลาก Object Player มาใส่ที่นี่
    public float interactionDistance = 3f; // ระยะที่อนุญาตให้กดเปิดได้ (เมตร)

    private Quaternion targetRotation;
    private Quaternion defaultRotation;

    void Start()
    {
        defaultRotation = transform.localRotation;
        
        // ถ้าลืมลาก Player มาใส่ ให้พยายามหาอัตโนมัติจาก Tag "Player"
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        // คำนวณระยะห่างระหว่างประตูกับ Player
        float distance = Vector3.Distance(transform.position, player.position);

        // เช็คว่ากดปุ่ม E และอยู่ในระยะที่กำหนดหรือไม่
        if (Input.GetKeyDown(KeyCode.E) && distance <= interactionDistance)
        {
            isOpen = !isOpen;
        }

        // การหมุนประตู
        if (isOpen)
        {
            targetRotation = Quaternion.Euler(0, openAngle, 0);
        }
        else
        {
            targetRotation = defaultRotation;
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}