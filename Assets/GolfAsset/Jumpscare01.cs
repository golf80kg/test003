using System.Collections; // เพิ่มบรรทัดนี้เพื่อแก้ Error IEnumerator
using UnityEngine;

public class Jumpscare01 : MonoBehaviour
{
    public GameObject JumpScareImg; // ช่องสำหรับลาก JumpscareImg จาก Canvas มาใส่
    public AudioSource audioSource; // ช่องสำหรับลาก Audio Source มาใส่

    void Start()
    {
        // ปิดรูปผีไว้ก่อนเสมอเมื่อเริ่มเกม
        if (JumpScareImg != null) JumpScareImg.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าผู้เล่นเดินมาชน (ต้องใช้ g ตัวเล็ก)
        if (other.gameObject.CompareTag("Player"))
        {
            JumpScareImg.SetActive(true); // โชว์รูปผี
            if (audioSource != null) audioSource.Play(); // เล่นเสียงผี

            StartCoroutine(DisableJumpscare()); // เริ่มนับถอยหลังปิดรูป
        }
    }

    IEnumerator DisableJumpscare()
    {
        yield return new WaitForSeconds(1.0f); // โชว์ผีค้างไว้ 2 วินาที
        JumpScareImg.SetActive(false);
        
        // ทำลายจุด Trigger ทิ้งไปเลย เพื่อไม่ให้ผีโผล่ซ้ำซ้อน
        Destroy(gameObject); 
    }
}