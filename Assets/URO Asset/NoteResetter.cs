using UnityEngine;

public class NoteResetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. เช็คว่าคนที่เดินมาชนคือตัวละครผู้เล่น (Player)
        if (other.CompareTag("Player"))
        {
            // 2. ค้นหาสมุดโน้ตทั้งหมด (ใช้คำสั่งใหม่ FindObjectsByType เพื่อแก้แจ้งเตือนสีเหลืองใน Unity 6)
            MapItem[] allNotes = FindObjectsByType<MapItem>(FindObjectsSortMode.None);
            
            // 3. สั่งให้สมุดโน้ตทุกเล่ม เช็คตัวเองว่า "ถ้าเก็บมาแล้ว ให้ทำลายทิ้งซะ"
            foreach (MapItem note in allNotes)
            {
                note.RemoveFromPlayer();
            }
            
            Debug.Log("เดินผ่านประตู: ล้างของในกระเป๋าเรียบร้อย พร้อมลุยด่านต่อไป!");

            // 4. ทำลายกล่องล้างกระเป๋า (Reset Note) ทิ้งไปเลย เพื่อให้ทำงานแค่ครั้งเดียว!
            Destroy(gameObject);
        }
    }
}