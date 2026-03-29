using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class JumpscareHide001 : MonoBehaviour
{
    public GameObject ghostModel; 
    public AudioSource scareSound; 
    public Image blackOverlay; 
    
    [Header("Time Settings")]
    public float ghostShowTime = 1f;   // << ช่องใหม่! ปรับเวลาให้ผีโชว์ตัวก่อนหาย (วินาที)
    public float blindDuration = 0.5f; // เวลาที่หน้าจอมืดสนิท
    public float fadeSpeed = 1.5f;     // ความเร็วในการกลับมามองเห็น

    private bool hasTriggered = false;

    private void Start()
    {
        if (blackOverlay != null)
        {
            Color c = blackOverlay.color;
            c.a = 0f;
            blackOverlay.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; 
            StartCoroutine(ShowAndHideGhost());
        }
    }

    IEnumerator ShowAndHideGhost()
    {
        // 1. ผีโผล่ + เล่นเสียง
        ghostModel.SetActive(true); 
        if (scareSound != null) scareSound.Play();

        // 2. รอตามเวลาที่ยูโรตั้งไว้ในช่อง Ghost Show Time
        yield return new WaitForSeconds(ghostShowTime);

        // 3. จังหวะผีหาย -> หน้าจอมืดสนิททันที!
        if (blackOverlay != null)
        {
            Color c = blackOverlay.color;
            c.a = 1f; 
            blackOverlay.color = c;
        }
        
        ghostModel.SetActive(false); 

        // 4. หน้าจอมืดค้างไว้แป๊บนึง
        yield return new WaitForSeconds(blindDuration);

        // 5. ค่อยๆ จางสีดำออกจนกลับมามองเห็นปกติ
        if (blackOverlay != null)
        {
            while (blackOverlay.color.a > 0)
            {
                Color c = blackOverlay.color;
                c.a -= Time.deltaTime * fadeSpeed;
                blackOverlay.color = c;
                yield return null;
            }
        }

        // 6. รอจนเสียงจบแล้วค่อยทำลายตัวเอง
        if (scareSound != null)
        {
            while (scareSound.isPlaying) yield return null;
        }

        Destroy(gameObject); 
    }
}