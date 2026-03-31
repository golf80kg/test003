using UnityEngine;
using System.Collections;

public class GhostAction : MonoBehaviour
{
    public GameObject ghostModel; 
    public Transform stopPoint;   
    public CanvasGroup blinkCanvasGroup; 
    public AudioSource scareSound;
    public float speed = 70f;
    public float fadeSpeed = 2f; 

    public void StartGhostDash()
    {
        // เริ่มทำงานจาก Manager (ซึ่งจะไม่หายไป)
        StartCoroutine(DashSequence());
    }

    IEnumerator DashSequence()
    {
        // 1. พุ่งเข้าหา
        float timer = 0;
        while (ghostModel != null && ghostModel.activeSelf && Vector3.Distance(ghostModel.transform.position, stopPoint.position) > 0.5f && timer < 1.0f)
        {
            ghostModel.transform.position = Vector3.MoveTowards(ghostModel.transform.position, stopPoint.position, speed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null; 
        }

        // 2. จังหวะหยุด: เล่นเสียง + จอดำ
        if (scareSound != null) scareSound.Play();
        if (blinkCanvasGroup != null) blinkCanvasGroup.alpha = 1;

        // 3. ปิดตัวผี (ตอนนี้ปิดได้เลย เพราะ Script รันอยู่ที่ Manager)
        if (ghostModel != null) ghostModel.SetActive(false);

        // 4. รอจังหวะหลับตาแป๊บนึง
        yield return new WaitForSeconds(0.2f); 

        // 5. ลืมตา (Fade Out) - บรรทัดนี้จะทำงานแน่นอนเพราะ Manager ยังอยู่
        if (blinkCanvasGroup != null)
        {
            while (blinkCanvasGroup.alpha > 0)
            {
                blinkCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }
            blinkCanvasGroup.alpha = 0; // มั่นใจว่าสว่างแน่นอน
        }
    }
}