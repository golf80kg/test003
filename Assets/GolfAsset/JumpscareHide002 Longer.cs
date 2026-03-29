using UnityEngine;
using System.Collections;

public class JumpscareHide002Longer : MonoBehaviour
{
    public GameObject ghostModel; 
    public AudioSource scareSound; 

    private bool hasTriggered = false; // กันบั๊กชนซ้ำ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // ล็อคไว้ว่าทำงานแล้ว
            StartCoroutine(ShowAndHideGhost());
        }
    }

    IEnumerator ShowAndHideGhost()
    {
        // 1. ผีโผล่ + เล่นเสียง
        ghostModel.SetActive(true); 
        if (scareSound != null) scareSound.Play();

        // 2. รอ 2 วินาทีตามที่ยูโรตั้งไว้
        yield return new WaitForSeconds(4f);

        // 3. ปิดโมเดลผี (แต่เสียงใน Audio Source จะยังดังอยู่ถ้าไม่ได้ติดไว้ที่ตัวผีที่ถูกปิด)
        ghostModel.SetActive(false);

        // 4. รอจนกว่าเสียงจะเล่นจบจริงๆ ก่อนค่อยทำลายตัวเอง (Trigger)
        if (scareSound != null)
        {
            // รอจนกว่า AudioSource จะหยุดเล่น
            while (scareSound.isPlaying)
            {
                yield return null; 
            }
        }

        // 5. ทำลายตัว Trigger ทิ้งเมื่อเสียงจบแล้ว
        Destroy(gameObject); 
    }
}