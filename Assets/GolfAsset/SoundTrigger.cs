using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    // ลาก Audio Source จากจุดที่วางไว้มาใส่ในช่องนี้
    public AudioSource remoteAudioSource; 
    
    // ตรวจสอบว่ามีอะไรมาชนกล่องนี้ไหม
    private void OnTriggerEnter(Collider other)
    {
        // เช็กว่าสิ่งที่มาชนคือ Player ใช่หรือไม่
        if (other.CompareTag("Player"))
        {
            // ถ้าใช่ ให้สั่งเล่นเสียงที่จุดกำเนิดเสียงที่ตั้งไว้
            if (remoteAudioSource != null && !remoteAudioSource.isPlaying)
            {
                remoteAudioSource.Play();
                Debug.Log("เดินชนแล้ว! เสียงดังที่: " + remoteAudioSource.gameObject.name);
                
                // ถ้าอยากให้ชนครั้งเดียวแล้วจบไปเลย ให้ลบกล่องทิ้ง (เอา // ออกถ้าต้องการ)
                // Destroy(gameObject); 
            }
        }
    }
}