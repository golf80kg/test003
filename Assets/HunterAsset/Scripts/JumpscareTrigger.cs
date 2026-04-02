using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    // ลาก JumpscareManager (ตัวที่ไม่หายไป) มาใส่ที่นี่
    public GhostAction ghostManager; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ghostManager != null)
            {
                ghostManager.StartGhostDash(); // สั่งให้ Manager ทำงาน
            }
            
            // ปิด Collider เพื่อไม่ให้ชนซ้ำ แทนการ Destroy ทันที (เพื่อรอให้เสียงเล่นจบถ้าคุณใส่เสียงไว้ที่นี่)
            GetComponent<Collider>().enabled = false; 
            
            // หรือถ้าจะทำลายทิ้ง ให้ไทำลายหลังจากผ่านปสัก 5 วินาที (เพื่อให้เสียงเล่นจนจบ)
            Destroy(gameObject, 5f); 
        }
    }
}