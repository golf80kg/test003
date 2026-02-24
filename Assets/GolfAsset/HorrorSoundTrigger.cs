using UnityEngine;

public class HorrorSoundTrigger : MonoBehaviour
{
    public AudioSource horrorAudio; // ช่องสำหรับใส่เสียงหลอน
    public bool playOnlyOnce = true; // ตั้งค่าว่าจะให้เล่นแค่ครั้งเดียวหรือไม่
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าเป็นผู้เล่นเดินมาชน และยังไม่เคยเล่นเสียง (กรณีตั้งให้เล่นครั้งเดียว)
        if (other.gameObject.CompareTag("Player") && !hasPlayed)
        {
            if (horrorAudio != null)
            {
                horrorAudio.Play();
                hasPlayed = true;
                Debug.Log("เล่นเสียงหลอนแล้ว!");

                // ถ้าตั้งให้เล่นครั้งเดียว ให้ทำลายวัตถุนี้ทิ้งหลังจากเสียงจบ (หรือทันที)
                if (playOnlyOnce)
                {
                    // ทำลายจุด Trigger ทิ้งเพื่อไม่ให้กินทรัพยากรเครื่อง
                    Destroy(gameObject, horrorAudio.clip.length); 
                }
            }
        }
    }
}