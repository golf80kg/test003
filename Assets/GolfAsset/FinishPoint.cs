using UnityEngine;

public class FinishPoint : MonoBehaviour 
{
    private void OnTriggerEnter(Collider other) 
    {
        // เช็กว่าตัวละคร (Player) เดินมาชนเส้นชัย
        if (other.CompareTag("Player")) 
        {
            // สั่งให้ Database หยุดเวลาและเขียนไฟล์ JSON ทันที
            if (SimpleDatabaseManager.Instance != null)
            {
                SimpleDatabaseManager.Instance.FinishAndSaveGame();
                Debug.Log("บันทึกข้อมูลเรียบร้อย!");
            }
        }
    }
}