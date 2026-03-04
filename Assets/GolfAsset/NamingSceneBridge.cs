using UnityEngine;
using TMPro;

public class NamingSceneBridge : MonoBehaviour
{
    // ลากช่อง InputField ในฉาก NamingScene มาใส่ตรงนี้
    public TMP_InputField localNameInput;

    public void OnClickSaveAndStart()
    {
        // 1. เช็คว่ามีตัว Manager (ตัวแม่) อยู่ในระบบไหม
        if (SimpleDatabaseManager.Instance != null)
        {
            // 2. ส่งปลั๊ก InputField ของฉากนี้ไปให้ตัวแม่
            SimpleDatabaseManager.Instance.nameInput = localNameInput;
            
            // 3. สั่งให้ตัวแม่ทำงาน (เก็บชื่อ, เริ่มนับเวลา, โหลด Scene1)
            SimpleDatabaseManager.Instance.StartGameRecord();
        }
        else
        {
            Debug.LogError("หา SimpleDatabaseManager (Instance) ไม่เจอ! คุณได้สร้างมันไว้ในฉาก MainMenu หรือยัง?");
        }
    }
}