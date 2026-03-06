using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable] 
public class RiddleData 
{ 
    public int id; 
    public string text; // คำถาม
    public string hint; // คำใบ้    
    public string[] choices;        // เก็บตัวเลือก 4 ข้อ
    public int correctAnswerIndex;  // เก็บตำแหน่งข้อที่ถูก (0, 1, 2, หรือ 3)
}

[System.Serializable] 
public class RiddleList 
{ 
    public List<RiddleData> riddles; 
}

public class DatabaseManager : MonoBehaviour 
{
    private List<RiddleData> mainDatabase = new List<RiddleData>();

    [Header("กระดานคำถาม (ประตู)")]
    public MapItem[] questionBoards;

    [Header("กระดานคำใบ้ (ตั้งเรียงให้ตรงกับประตู)")]
    public MapItem[] hintBoards;

    void Start() 
    {
        LoadData(); 
        
        // สั่งให้สุ่ม 2 ข้อ โดยเลือกเฉพาะช่วง ID ที่ 1 ถึง 3
        AssignRandomPuzzles(2, 1, 2); 
    }

    void LoadData() 
    {
        string dataPath = Path.Combine(Application.streamingAssetsPath, "Riddles.json");
        
        if (File.Exists(dataPath))
        {
            string jsonContent = File.ReadAllText(dataPath);
            RiddleList jsonData = JsonUtility.FromJson<RiddleList>(jsonContent);

            if (jsonData != null && jsonData.riddles != null)
            {
                mainDatabase = jsonData.riddles;
                Debug.Log($"โหลดไฟล์สำเร็จ! มีโจทย์ทั้งหมด {mainDatabase.Count} ข้อ");
            }
        }
        else
        {
            Debug.LogError("หาไฟล์ Riddles.json ไม่เจอครับ ลองเช็คชื่อไฟล์อีกรอบนะ!");
        }
    }

    // เพิ่มการรับค่า minId และ maxId เพื่อกำหนดขอบเขต
    void AssignRandomPuzzles(int amount, int minId, int maxId) 
    {
        // 1. กรอง (Filter) ก่อนเลย: เอาเฉพาะโจทย์ที่มี ID ตั้งแต่ minId ถึง maxId
        List<RiddleData> filteredPuzzles = mainDatabase.Where(r => r.id >= minId && r.id <= maxId).ToList();

        // 2. เช็คว่าหลังจากกรองแล้ว มีโจทย์เหลือพอให้สุ่มไหม
        if (filteredPuzzles.Count < amount) 
        {
            Debug.LogError($"มีโจทย์ไม่พอในช่วงที่กำหนด! ต้องการ {amount} ข้อ แต่หาเจอแค่ {filteredPuzzles.Count} ข้อ");
            // ถ้าไม่พอ ให้ปรับ amount ให้เท่ากับที่หาเจอเพื่อไม่ให้มี Error
            amount = filteredPuzzles.Count; 
        }

        // 3. สับไพ่และหยิบโจทย์ เฉพาะกลุ่มที่กรองมาแล้ว
        List<RiddleData> selectedPuzzles = filteredPuzzles.OrderBy(x => Random.value).Take(amount).ToList();

      for (int i = 0; i < amount; i++) 
        {
            Debug.Log($"คู่ที่ {i+1} ได้โจทย์ ID: {selectedPuzzles[i].id}");

            // แจกคำถาม (ส่งข้อมูลไปทั้งก้อน selectedPuzzles[i] เลย)
            if (i < questionBoards.Length && questionBoards[i] != null) 
            {
                questionBoards[i].SetupQuestion(selectedPuzzles[i]); 
            }
            
            // แจกคำใบ้ (ส่งไปแค่ text ของคำใบ้)
            if (i < hintBoards.Length && hintBoards[i] != null) 
            {
                hintBoards[i].SetupHint(selectedPuzzles[i].hint); 
            }
        }
    }
}