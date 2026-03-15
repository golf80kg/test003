using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class JumpscareBox : MonoBehaviour
{
    [Header("Settings")]
    public GameObject mainPlayer;       
    public GameObject scareCamera;      
    public PlayableDirector ghostTimeline; 
    
    [Tooltip("ใส่เวลาที่ต้องการให้กล้องแช่อยู่ที่ผี (วินาที) เช่น 0.5 หรือ 1.0")]
    public float scareDuration = 0.8f; 

    [Header("Audio (Optional)")]
    public AudioSource scareAudioSource; // ลาก Audio Source ที่มีเสียงยาวๆ มาใส่ช่องนี้

    public bool destroyAfterUse = true;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(PlayTheScare());
        }
    }

    IEnumerator PlayTheScare()
    {
        isTriggered = true;

        // 1. สลับกล้อง
        if (mainPlayer != null) mainPlayer.SetActive(false);
        if (scareCamera != null) scareCamera.SetActive(true);

        // 2. สั่งเล่นเสียง (ถ้ามี) - เสียงจะดังอิสระไม่เกี่ยวกับเวลา Timeline
        if (scareAudioSource != null) 
        {
            scareAudioSource.Play();
        }

        // 3. สั่งเล่น Timeline (ภาพผีพุ่ง)
        if (ghostTimeline != null)
        {
            ghostTimeline.Play();
            
            // ใช้เวลาที่เรากำหนดเองแทน ghostTimeline.duration
            yield return new WaitForSeconds(scareDuration);
        }

        // 4. สลับกลับมาที่เดิมทันที (แม้เสียงจะยังไม่จบ)
        if (scareCamera != null) scareCamera.SetActive(false);
        if (mainPlayer != null) mainPlayer.SetActive(true);

        // ทำลาย Box นี้ทิ้งหลังจากผ่านไป 5 วินาที (เพื่อให้เสียงเล่นจนจบก่อนถูกลบ)
        if (destroyAfterUse) 
        {
            // ปิด Collider เพื่อไม่ให้ชนซ้ำ
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 5f); 
        }
    }
}