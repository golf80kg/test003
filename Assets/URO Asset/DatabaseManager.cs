using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable] 
public class RiddleData 
{ 
    public int id; 
    public string type; 
    public string text; 
    public string hint;     
    public string[] choices;        
    public int correctAnswerIndex;  
}
//วางโครงสร้างจากไฟล์JSON
[System.Serializable] 
public class RiddleList 
{ 
    public List<RiddleData> riddles; 
}

public class DatabaseManager : MonoBehaviour 
{
    private List<RiddleData> mainDatabase = new List<RiddleData>();
//inspector ใน unity
    [Header("ตั้งค่าหมวดหมู่ของด่านนี้ (เช่น IFElse, Loop, Array)")]
    public string levelType = "IFElse"; 
    public int puzzlesToSpawn = 2;      

    [Header("กระดานคำถาม (ประตู)")]
    public MapItem[] questionBoards;

    [Header("กระดานคำใบ้ (ตั้งเรียงให้ตรงกับประตู)")]
    public MapItem[] hintBoards;

    void Start() 
    {
        LoadData(); 
        
        
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
    
    void AssignRandomPuzzles(int amount, string targetType) 
    {
        //  กรองเอาเฉพาะโจทย์ที่มี type ตรงกับ targetType 
        List<RiddleData> filteredPuzzles = mainDatabase.Where(r => r.type == targetType).ToList();

        //เช็คว่ามีโจทย์เหลือพอให้สุ่มไหม
        if (filteredPuzzles.Count < amount) 
        {
            Debug.LogWarning($"มีโจทย์หมวด '{targetType}' ไม่พอ! ต้องการ {amount} ข้อ แต่มีแค่ {filteredPuzzles.Count} ข้อ (ระบบจะแสดงเท่าที่มี)");
            amount = filteredPuzzles.Count; 
        }
       
        List<RiddleData> selectedPuzzles = filteredPuzzles.OrderBy(x => Random.value).Take(amount).ToList();

        for (int i = 0; i < amount; i++) 
        {
            Debug.Log($"ด่าน {targetType} | ประตูที่ {i+1} สุ่มได้โจทย์ ID: {selectedPuzzles[i].id}");
            
            if (i < questionBoards.Length && questionBoards[i] != null) 
            {
                questionBoards[i].SetupQuestion(selectedPuzzles[i]); 
            }
                       
            if (i < hintBoards.Length && hintBoards[i] != null) 
            {
                hintBoards[i].SetupHint(selectedPuzzles[i].hint); 
            }
        }
    }
}