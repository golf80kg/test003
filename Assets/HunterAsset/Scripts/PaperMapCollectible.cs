using UnityEngine;

public class PaperMapCollectible : MonoBehaviour
{
    // เปลี่ยนมาใช้ชื่อคลาส MapControllerHub เพื่อให้ตรงกับไฟล์ที่สอง
    public MapControllerHub playerSystem; 
    
    private bool canPickUp = false;

    void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            if (playerSystem != null)
            {
                playerSystem.ReceiveMap(); // ส่งคำสั่งไปที่ตัวผู้เล่น
                Destroy(gameObject);      // ทำลายไอเทมบนพื้น
            }
            else
            {
                Debug.LogError("กรุณาลาก Object Player มาใส่ในช่อง Player System ใน Inspector ก่อนครับ!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) canPickUp = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) canPickUp = false;
    }
}