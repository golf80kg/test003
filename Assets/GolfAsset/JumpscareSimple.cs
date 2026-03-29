using UnityEngine;
using System.Collections;

public class JumpscareSimple : MonoBehaviour
{
    [Header("Settings")]
    public GameObject mainPlayer;       
    public GameObject scareCamera;      
    public float scareDuration = 0.5f; // เวลาที่กล้องจะแช่ที่ผี

    [Header("Audio")]
    public AudioSource scareAudio;      

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(PlaySimpleScare());
        }
    }

    IEnumerator PlaySimpleScare()
    {
        isTriggered = true;

        // 1. สลับกล้อง (ปิดตัวผู้เล่น เปิดกล้องผี)
        if (mainPlayer != null) mainPlayer.SetActive(false);
        if (scareCamera != null) scareCamera.SetActive(true);

        // 2. เล่นเสียง
        if (scareAudio != null) scareAudio.Play();

        // 3. รอเวลาตามที่ตั้งไว้ (แช่กล้องดูผี)
        yield return new WaitForSeconds(scareDuration);

        // 4. สลับกลับมาเป็นผู้เล่นปกติ
        if (scareCamera != null) scareCamera.SetActive(false);
        if (mainPlayer != null) mainPlayer.SetActive(true);

        // 5. ทำลายตัว Trigger ทิ้ง (กันเดินชนซ้ำ)
        Destroy(gameObject, 0.1f); 
    }
}