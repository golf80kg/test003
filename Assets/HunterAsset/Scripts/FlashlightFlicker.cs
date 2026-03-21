using UnityEngine;
using System.Collections;

public partial class FlashlightFlicker : MonoBehaviour
{
    private Light flashlight;
    public float minTime = 0.05f; // เวลาต่ำสุดที่ไฟจะดับหรือติด
    public float maxTime = 0.2f;  // เวลาสูงสุดที่ไฟจะดับหรือติด

    void Start()
    {
        flashlight = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // สลับสถานะ เปิด/ปิด
            flashlight.enabled = !flashlight.enabled;
            
            // สุ่มเวลาเพื่อความสมจริง (ไม่ให้จังหวะเท่ากันเกินไป)
            float randomTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randomTime);
        }
    }
}