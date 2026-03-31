using UnityEngine;
using System.Collections;

public class JumpscareBlock : MonoBehaviour
{
    [Header("Target Model")]
    public GameObject modelToShow;    // ลากโมเดลผีที่ปิดอยู่มาใส่
    
    [Header("Settings")]
    public float stayTime = 2.0f;     // จะให้โผล่มากี่วินาที
    public AudioSource scareSound;    // เสียง (ถ้ามี)
    
    private bool isUsed = false;      // กันไม่ให้ทำงานซ้ำ

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าคนชนคือ Player และยังไม่เคยทำงาน
        if (other.CompareTag("Player") && !isUsed)
        {
            StartCoroutine(TriggerEvent());
        }
    }

    IEnumerator TriggerEvent()
    {
        isUsed = true; // ล็อกไว้ไม่ให้เกิดซ้ำ

        // 1. สั่งให้โมเดลปรากฏ
        if (modelToShow != null)
        {
            modelToShow.SetActive(true);
        }

        // 2. เล่นเสียง
        if (scareSound != null)
        {
            scareSound.Play();
        }

        // 3. รอตามเวลาที่ตั้งไว้
        yield return new WaitForSeconds(stayTime);

        // 4. สั่งให้โมเดลหายไป
        if (modelToShow != null)
        {
            modelToShow.SetActive(false);
        }

        // 5. ทำลายบล็อกทิ้ง (ทางเลือก: เพื่อไม่ให้ขวางทางหรือเปลืองทรัพยากร)
        Destroy(gameObject);
    }
}