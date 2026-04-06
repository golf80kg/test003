using System.Collections;
using UnityEngine;

public class Jumpscare01 : MonoBehaviour
{
    public GameObject JumpScareImg; 
    public AudioSource audioSource; 

    private bool hasTriggered = false; // ไม่ให้ทำงานซ้ำ

    void Start()
    {
        if (JumpScareImg != null) JumpScareImg.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //เช็คว่าเคยทำงานไปรึยัง
        if (hasTriggered || !other.CompareTag("Player")) return;

        hasTriggered = true; // ล็อคไว้ว่าทำงานแล้ว
        
        if (JumpScareImg != null) JumpScareImg.SetActive(true); 
        if (audioSource != null) audioSource.Play(); 

        // ปิด Collider ทันที เพื่อไม่ให้เดินชนซ้ำแล้วนับเวลาใหม่
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        StartCoroutine(DisableJumpscare());
    }

    IEnumerator DisableJumpscare()
    {
        // โชว์รูปผีค้างไว้ 1 วินาที
        yield return new WaitForSeconds(1.0f); 
        
        if (JumpScareImg != null) JumpScareImg.SetActive(false);

        // รอจนกว่าเสียงจะเล่นจบ
        if (audioSource != null && audioSource.clip != null)
        {            
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        Destroy(gameObject); 
    }
}