using UnityEngine;
using TMPro;

public class NamingSceneBridge : MonoBehaviour
{
    // ลากช่อง InputField ในฉาก NamingScene มาใส่ตรงนี้
    public TMP_InputField localNameInput;

    public void OnClickSaveAndStart()
{
    // 1. ตรวจสอบก่อนว่ากรอกชื่อหรือยัง (ตัดช่องว่างออกด้วย Trim)
    if (localNameInput != null && string.IsNullOrWhiteSpace(localNameInput.text))
    {
        Debug.LogWarning("กรุณากรอกชื่อก่อนเริ่มเกม!");
        // คุณอาจจะเพิ่มโค้ดแสดง UI เตือนผู้เล่นตรงนี้ได้ เช่น textAlert.text = "กรุณาใส่ชื่อ";
        return; // สั่งให้หยุดการทำงานตรงนี้เลย ไม่ไปบรรทัดถัดไป
    }

    // 2. ถ้ากรอกชื่อแล้ว ถึงจะสั่งให้ตัวแม่ทำงาน
    if (SimpleDatabaseManager.Instance != null)
    {
        SimpleDatabaseManager.Instance.nameInput = localNameInput;
        SimpleDatabaseManager.Instance.StartGameRecord();
    }
    else
    {
        Debug.LogError("หา SimpleDatabaseManager ไม่เจอ!");
    }
}
}