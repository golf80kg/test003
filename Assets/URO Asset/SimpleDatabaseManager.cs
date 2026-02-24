using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

// 1. กำหนดโครงสร้างข้อมูล (Schema)
[System.Serializable]
public class PlayerData 
{
    public string playerName;
    public int score;
}

// 2. จำลอง Table สำหรับเก็บรายชื่อผู้เล่นทั้งหมด
[System.Serializable]
public class PlayerDatabase 
{
    public List<PlayerData> players = new List<PlayerData>();
}

public class SimpleDatabaseManager : MonoBehaviour 
{
    [Header("UI References")]
    public TMP_InputField nameInput; // ช่องกรอกชื่อ
    
    private string savePath;
    private PlayerDatabase db;

    void Start() 
    {
        // กำหนดที่อยู่ไฟล์ Database ให้อยู่ในโฟลเดอร์เซฟเกมของเครื่อง
        savePath = Application.persistentDataPath + "/ArcadeDatabase.json";
        LoadDatabase();
    }

    // ฟังก์ชันนี้เอาไปผูกกับปุ่ม Save
    public void SavePlayer() 
    {
        string newName = nameInput.text;
        
        // กันคนกดส่งข้อมูลว่างๆ
        if (string.IsNullOrEmpty(newName)) 
        {
            Debug.LogWarning("ยังไม่ได้กรอกชื่อเลย!");
            return;
        }

        // 3. เพิ่มข้อมูลคนใหม่เข้าไป
        db.players.Add(new PlayerData { playerName = newName, score = 0 });
        
        // 4. แปลงข้อมูลทั้งหมดเป็น JSON แล้วเขียนลงไฟล์
        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(savePath, json);
        
        Debug.Log("บันทึกชื่อ: " + newName + " ลง Database สำเร็จ!");
        Debug.Log("ไฟล์เก็บไว้ที่: " + savePath);
        
        nameInput.text = ""; // เคลียร์ช่องให้ว่าง
    }

    // ฟังก์ชันโหลดข้อมูลเก่าขึ้นมาตอนเปิดเกม
    void LoadDatabase() 
    {
        if (File.Exists(savePath)) 
        {
            string json = File.ReadAllText(savePath);
            db = JsonUtility.FromJson<PlayerDatabase>(json);
        } 
        else 
        {
            db = new PlayerDatabase(); // ถ้ายังไม่มีไฟล์ ให้สร้างฐานข้อมูลใหม่ว่างๆ
        }
    }
}