using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable] 
public class RiddleData 
{ 
    public int id; 
    public string type; // <--- เพิ่มตัวแปร type เพื่อรับค่าหมวดหมู่จาก JSON
    public string text; 
    public string hint;     
    public string[] choices;        
    public int correctAnswerIndex;  
}

[System.Serializable] 
public class RiddleList 
{ 
    public List<RiddleData> riddles; 
}

public class DatabaseManager : MonoBehaviour 
{
    private List<RiddleData> mainDatabase = new List<RiddleData>();

    [Header("ตั้งค่าหมวดหมู่ของด่านนี้ (เช่น IFElse, Loop, Array)")]
    public string levelType = "IFElse"; // พิมพ์ชื่อด่านที่ต้องการสุ่มได้เลยจาก Inspector
    public int puzzlesToSpawn = 2;      // จำนวนข้อที่ต้องการให้สุ่มโผล่มา

    [Header("กระดานคำถาม (ประตู)")]
    public MapItem[] questionBoards;

    [Header("กระดานคำใบ้ (ตั้งเรียงให้ตรงกับประตู)")]
    public MapItem[] hintBoards;

    void Start() 
    {
        LoadData(); 
        
        // สั่งให้สุ่มโจทย์ ตามจำนวนและหมวดหมู่ที่ตั้งไว้ใน Inspector
        AssignRandomPuzzles(puzzlesToSpawn, levelType); 
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

    // แก้ไขฟังก์ชันให้รับค่าหมวดหมู่ (string) แทน ID
    void AssignRandomPuzzles(int amount, string targetType) 
    {
        // 1. กรอง (Filter): เอาเฉพาะโจทย์ที่มี type ตรงกับ targetType (เช่น "IFElse")
        List<RiddleData> filteredPuzzles = mainDatabase.Where(r => r.type == targetType).ToList();

        // 2. เช็คว่าหลังจากกรองแล้ว มีโจทย์เหลือพอให้สุ่มไหม
        if (filteredPuzzles.Count < amount) 
        {
            Debug.LogWarning($"มีโจทย์หมวด '{targetType}' ไม่พอ! ต้องการ {amount} ข้อ แต่มีแค่ {filteredPuzzles.Count} ข้อ (ระบบจะแสดงเท่าที่มี)");
            amount = filteredPuzzles.Count; 
        }

        // 3. สับไพ่และหยิบโจทย์ เฉพาะกลุ่มที่กรองมาแล้ว
        List<RiddleData> selectedPuzzles = filteredPuzzles.OrderBy(x => Random.value).Take(amount).ToList();

        for (int i = 0; i < amount; i++) 
        {
            Debug.Log($"ด่าน {targetType} | ประตูที่ {i+1} สุ่มได้โจทย์ ID: {selectedPuzzles[i].id}");

            // แจกคำถาม
            if (i < questionBoards.Length && questionBoards[i] != null) 
            {
                questionBoards[i].SetupQuestion(selectedPuzzles[i]); 
            }
            
            // แจกคำใบ้
            if (i < hintBoards.Length && hintBoards[i] != null) 
            {
                hintBoards[i].SetupHint(selectedPuzzles[i].hint); 
            }
        }
    }
}