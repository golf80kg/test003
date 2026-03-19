using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // <--- จุดที่ 1: เพิ่มบรรทัดนี้เพื่อให้เรียกใช้ระบบเปลี่ยน Scene ได้

public class NamingSceneBridge : MonoBehaviour
{
    // ลากช่อง InputField ในฉาก NamingScene มาใส่ตรงนี้
    public TMP_InputField localNameInput;

    // <--- จุดที่ 2: เพิ่มช่องสำหรับใส่ชื่อ Scene คัทซีนของคุณ
    public string cutsceneSceneName = "CutScene"; 

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
            SimpleDatabaseManager.Instance.StartGameRecord(); // บันทึกข้อมูลลง Database

            // <--- จุดที่ 3: พอระบบเพื่อนบันทึกเสร็จปุ๊บ สั่งโหลดหน้าคัทซีนทันที! --->
            SceneManager.LoadScene(cutsceneSceneName);
        }
        else
        {
            Debug.LogError("หา SimpleDatabaseManager ไม่เจอ!");
        }
    }
}