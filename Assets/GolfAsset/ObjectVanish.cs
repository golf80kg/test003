using UnityEngine;
using System.Collections;

public class ObjectVanish : MonoBehaviour
{
    public GameObject targetModel; // ลากโมเดลที่ต้องการให้หายมาใส่ช่องนี้
    public float minWaitTime = 0.05f;  // เวลารอน้อยสุด
    public float maxWaitTime = 0.2f;  // เวลารอนานสุด

    void Start()
    {
        StartCoroutine(VanishRoutine());
    }

    IEnumerator VanishRoutine()
    {
        while (true)
        {
            // สุ่มเวลาที่จะ "ปรากฏตัว" หรือ "หายไป"
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // สลับสถานะ (ถ้าเปิดอยู่ให้ปิด ถ้าปิดอยู่ให้เปิด)
            if (targetModel != null)
            {
                targetModel.SetActive(!targetModel.activeSelf);
                
                // ถ้าอยากให้หลอนขึ้น: เมื่อมันโผล่มา ให้สุ่มตำแหน่งใหม่นิดหน่อยก็ได้ครับ
                /*
                if(targetModel.activeSelf) {
                    targetModel.transform.localPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                }
                */
            }
        }
    }
}