using UnityEngine;

public class LightFollower : MonoBehaviour
{
    [Header("ตัวไฟฉายที่เป็นต้นแบบ")]
    public Light targetLight; // ลาก Spot Light (ไฟฉาย) มาใส่ช่องนี้
    
    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        if (targetLight != null)
        {
            // สั่งให้ Point Light เปิด/ปิด ตามสถานะของไฟฉายเป๊ะๆ
            myLight.enabled = targetLight.enabled;
            
            // แถม: ถ้าอยากให้ความสว่าง (Intensity) เปลี่ยนตามด้วย (เผื่อไฟฉายหรี่)
            // myLight.intensity = targetLight.intensity; 
        }
    }
}