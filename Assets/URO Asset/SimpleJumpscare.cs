using UnityEngine;
using System.Collections; // ขาดบรรทัดนี้ไม่ได้เวลาใช้ Coroutine

public class SimpleJumpscare : MonoBehaviour
{
    [Header("ก้อน UI ที่จะให้เด้งขึ้นมา")]
    public GameObject jumpscareUI; // ตัว Panel รูปผี
    
    [Header("ลำโพงที่เก็บเสียงกรี๊ด")]
    public AudioSource scareAudioSource; // ตัว Audio Source ของเสียง

    [Header("ค้างภาพไว้กี่วินาที")]
    public float duration = 1.0f; // เวลาที่ผีจะอยู่บนจอ

    private bool isScared = false; // ตัวแปรเช็คว่า Jumpscare ทำงานไปหรือยัง (เพื่อไม่ให้เล่นซ้ำ)

    // ฟังก์ชันนี้จะทำงานอัตโนมัติเมื่อมีบางอย่างเดินเข้ามาในกรอบ Trigger
    private void OnTriggerEnter(Collider other)
    {
        // เช็คว่าคนที่เดินมาชนคือ "ผู้เล่น" และ Jumpscare ยังไม่เคยเล่นมาก่อน
        if (other.CompareTag("Player") && !isScared)
        {
            StartCoroutine(ActivateScare()); // เรียกใช้ Coroutine เพื่อเล่น Jumpscare
        }
    }

    // Coroutine สำหรับเล่นเสียง เปิดภาพ และปิดภาพตามเวลา
    IEnumerator ActivateScare()
    {
        isScared = true; // ล็อกไว้ไม่ให้เล่นซ้ำ
        Debug.Log("Jumpscare!!!");

        // 1. เปิดสวิตช์ UI ภาพผี
        if (jumpscareUI != null) jumpscareUI.SetActive(true);

        // 2. สั่งเล่นเสียง
        if (scareAudioSource != null) scareAudioSource.Play();

        // 3. รอตามเวลาที่กำหนด
        yield return new WaitForSeconds(duration);

        // 4. หลังจากรอเสร็จ สั่งปิด UI ภาพผี
        if (jumpscareUI != null) jumpscareUI.SetActive(false);

        // (เลือกได้) ทำลายทิ้งไปเลยเพื่อประหยัดทรัพยากร หรือปล่อยไว้เผื่อคุณมีระบบรีเซ็ต
        // Destroy(gameObject); 
    }
}