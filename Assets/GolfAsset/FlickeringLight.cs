using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private Light lightSource;
    
    [Header("ลากโมเดลผีมาใส่ช่องนี้")]
    public GameObject ghost; 

    [Header("ตั้งค่าแสงไฟ")]
    public float minIntensity = 0.2f; 
    public float maxIntensity = 1.8f; 
    public float flickerSpeed = 0.08f; 

    [Header("โอกาสที่ผีจะโผล่ (0-100%)")]
    public float ghostChance = 30f; 

    void Start()
    {
        lightSource = GetComponent<Light>();
        // เริ่มต้นให้ผีหายไปก่อน
        if (ghost != null) ghost.SetActive(false);
        
        InvokeRepeating("Flicker", 0, flickerSpeed);
    }

    void Flicker()
    {
        if (lightSource != null)
        {
            // 1. สุ่มไฟกระพริบ
            float ranIntensity = Random.Range(minIntensity, maxIntensity);
            lightSource.intensity = ranIntensity;

            // 2. เช็คจังหวะไฟเพื่อคุมผี
            if (ghost != null)
            {
                // ถ้าไฟสว่างวาบ (มากกว่า 1.2) ให้สุ่มโอกาสว่าผีจะโผล่ไหม
                if (ranIntensity > 1.2f) 
                {
                    float chance = Random.Range(0f, 100f);
                    if (chance < ghostChance) 
                    {
                        ghost.SetActive(true); // ผีโผล่ตอนไฟสว่างแวบๆ
                    }
                }
                else 
                {
                    ghost.SetActive(false); // ถ้าไฟหรี่หรือมืด ให้ผีหายไปทันที
                }
            }
        }
    }
}