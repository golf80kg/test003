using UnityEngine;

public class MapControllerHub : MonoBehaviour
{
    public GameObject mapCanvasPanel; // ลาก UI Image แผนที่มาใส่ที่นี่
    private bool hasMap = false;      // เช็คว่าเก็บแผนที่หรือยัง
    private bool isMapOpen = false;

    void Start()
    {
        // เริ่มเกมให้ปิดแผนที่ไว้ก่อน
        if (mapCanvasPanel != null)
            mapCanvasPanel.SetActive(false);
    }

    void Update()
    {
        // ถ้ามีแผนที่แล้ว และกดปุ่ม M
        if (hasMap && Input.GetKeyDown(KeyCode.M))
        {
            isMapOpen = !isMapOpen;
            mapCanvasPanel.SetActive(isMapOpen);
        }
    }

    // ฟังก์ชันรับแผนที่ (ตัว PaperMapCollectible จะมาเรียกใช้ตัวนี้)
    public void ReceiveMap()
    {
        hasMap = true;
        Debug.Log("เก็บแผนที่แล้ว! กด M เพื่อเปิดดู");
    }
}