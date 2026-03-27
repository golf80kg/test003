using UnityEngine;
using System.Collections;

public class AmbientFadeZone : MonoBehaviour
{
    public AudioSource targetAudio;    // ลาก Audio Source ตัวในรูปมาใส่
    public float maxVolume = 1f;       // ความดังสูงสุดที่ต้องการ
    public float fadeSpeed = 0.5f;     // ความเร็วในการค่อยๆ ดัง/เบา (ยิ่งน้อยยิ่งช้า)

    private Coroutine fadeCoroutine;

    private void Start()
    {
        if (targetAudio != null)
        {
            targetAudio.volume = 0;    // เริ่มเกมให้เสียงเงียบไว้ก่อน
            targetAudio.Play();        // ให้มัน Play วน Loop ไว้รอเลยแต่ Volume เป็น 0
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudio(maxVolume)); // ค่อยๆ ดังขึ้น
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudio(0)); // ค่อยๆ เงียบลง
        }
    }

    IEnumerator FadeAudio(float targetVolume)
    {
        while (!Mathf.Approximately(targetAudio.volume, targetVolume))
        {
            targetAudio.volume = Mathf.MoveTowards(targetAudio.volume, targetVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}