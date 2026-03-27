using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    // ลาก Audio Source จากจุดที่วางไว้ในแมพมาใส่ในช่องนี้
    public AudioSource remoteAudioSource; 
    
    // ตรวจสอบว่ามีอะไรมาชนกล่องนี้ไหม
    private void OnTriggerEnter(Collider other)
    {
        // 1. เช็กว่าสิ่งที่มาชนคือ Player ใช่หรือไม่
        if (other.CompareTag("Player"))
        {
            // 2. เช็กว่ามี Audio Source ใส่ไว้ไหม และตอนนี้เสียงไม่ได้กำลังเล่นอยู่
            if (remoteAudioSource != null && !remoteAudioSource.isPlaying)
            {
                // 3. สั่งให้เสียงเริ่มเล่น
                remoteAudioSource.Play();
                
                Debug.Log("เดินชนแล้ว! เสียงดังที่: " + remoteAudioSource.gameObject.name);
                
                // 4. [จุดสำคัญ] ทำลายตัวกล่อง Trigger (ตัวเอง) ทิ้งทันที
                // เพื่อให้ไม่สามารถกลับมาชนซ้ำได้อีกในเกมรอบนั้น
                Destroy(gameObject); 
            }
        }
    }
}