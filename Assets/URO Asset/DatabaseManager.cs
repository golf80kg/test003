using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

// --- โครงสร้างข้อมูลสำหรับ JSON ---
[System.Serializable] public class QData { public int id; public string text; }
[System.Serializable] public class QList { public List<QData> questions; }

[System.Serializable] public class HData { public int questionId; public string hintText; }
[System.Serializable] public class HList { public List<HData> hints; }

public class FullQuestion {
    public int id;
    public string questionText;
    public string hintText;
}

public class DatabaseManager : MonoBehaviour {
    private List<FullQuestion> combinedDatabase = new List<FullQuestion>();

    [Header("กระดานคำถาม (ประตู)")]
    public MapItem[] questionBoards; // เปลี่ยนเป็น Array เพื่อใส่ได้หลายบาน

    [Header("กระดานคำใบ้ (ตั้งเรียงให้ตรงกับประตู)")]
    public MapItem[] hintBoards;     // เปลี่ยนเป็น Array เพื่อใส่ได้หลายบาน

    void Start() {
        LoadAndCombineData();
        
        // สั่งให้สุ่ม 2 คู่ (ถ้าอนาคตมี 3 ประตู ก็เปลี่ยนเลขนี้เป็น 3 ได้เลย)
        AssignRandomPuzzles(2); 
    }

    void LoadAndCombineData() {
        string qPath = Path.Combine(Application.streamingAssetsPath, "Questions.json");
        QList qList = JsonUtility.FromJson<QList>(File.ReadAllText(qPath));

        string hPath = Path.Combine(Application.streamingAssetsPath, "Hints.json");
        HList hList = JsonUtility.FromJson<HList>(File.ReadAllText(hPath));

        foreach (var q in qList.questions) {
            var matchHint = hList.hints.Find(h => h.questionId == q.id);
            combinedDatabase.Add(new FullQuestion {
                id = q.id,
                questionText = q.text,
                hintText = (matchHint != null) ? matchHint.hintText : "ไม่มีคำใบ้"
            });
        }
    }

    void AssignRandomPuzzles(int amount) {
        // เช็คก่อนว่าใน JSON มีโจทย์พอให้สุ่มไหม
        if (combinedDatabase.Count < amount) {
            Debug.LogError("มีโจทย์ไม่พอ! ต้องการ " + amount + " ข้อ แต่ใน JSON มีแค่ " + combinedDatabase.Count + " ข้อ");
            return;
        }

        // 1. สับไพ่ แล้วหยิบโจทย์ออกมาแบบ "ไม่ซ้ำกัน" ตามจำนวน amount (ในที่นี้คือ 2)
        List<FullQuestion> selectedPuzzles = combinedDatabase.OrderBy(x => Random.value).Take(amount).ToList();

        // 2. เดินแจกโจทย์ให้กระดานทีละคู่
        for (int i = 0; i < amount; i++) {
            Debug.Log($"คู่ที่ {i+1} ได้โจทย์ ID: {selectedPuzzles[i].id}");

            // แจกให้กระดานคำถาม (ถ้าใส่ไว้ใน Inspector)
            if (i < questionBoards.Length && questionBoards[i] != null) {
                questionBoards[i].Setup(selectedPuzzles[i].questionText);
            }
            
            // แจกให้กระดานคำใบ้ (ถ้าใส่ไว้ใน Inspector)
            if (i < hintBoards.Length && hintBoards[i] != null) {
                hintBoards[i].Setup(selectedPuzzles[i].hintText);
            }
        }
    }
}