using UnityEngine;
using System.Collections; // ต้องมีบรรทัดนี้เพื่อใช้ระบบหน่วงเวลา (Coroutine)

public class JumpscareHide001 : MonoBehaviour
{
    public GameObject ghostModel; // ลากโมเดลผีมาใส่ช่องนี้
    public AudioSource scareSound; // (ตัวเลือก) ลากเสียงตกใจมาใส่

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShowAndHideGhost()); // สั่งให้เริ่มทำงานแบบหน่วงเวลา
        }
    }

    IEnumerator ShowAndHideGhost()
    {
        // 1. ทำให้ผีปรากฏตัว
        ghostModel.SetActive(true); 
        if (scareSound != null) scareSound.Play();

        // 2. รอเวลา 3 วินาที (ยูโรปรับเลขได้ตรงนี้)
        yield return new WaitForSeconds(2f);

        // 3. ทำให้ผีหายไป
        ghostModel.SetActive(false);

        // 4. ทำลายตัว Trigger ทิ้งไปเลยเพื่อไม่ให้หลอกซ้ำที่เดิม
        Destroy(gameObject); 
    }
}