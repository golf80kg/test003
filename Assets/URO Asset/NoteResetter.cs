using UnityEngine;

public class NoteResetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. เช็คว่าคนที่เดินมาชนคือตัวละครผู้เล่น (Player)
        if (other.CompareTag("Player"))
        {
            // 2. ค้นหาสมุดโน้ตทั้งหมดที่อยู่ในฉาก (ทั้งที่พื้นและที่เก็บมาแล้ว)
            MapItem[] allNotes = FindObjectsOfType<MapItem>();
            
            // 3. สั่งให้สมุดโน้ตทุกเล่ม เช็คตัวเองว่า "ถ้าเก็บมาแล้ว ให้ทำลายทิ้งซะ"
            foreach (MapItem note in allNotes)
            {
                note.RemoveFromPlayer();
            }
            
            Debug.Log("เดินผ่านประตู: ล้างของในกระเป๋าเรียบร้อย พร้อมลุยด่านต่อไป!");
        }
    }
}