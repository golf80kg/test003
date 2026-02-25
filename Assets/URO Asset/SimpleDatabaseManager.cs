using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement; // เพิ่มเพื่อใช้ในการเปลี่ยนฉาก

[System.Serializable]
public class PlayerData 
{
    public string playerName;
    public float totalPlayTime; // เปลี่ยนมาเก็บเวลาเล่นแทนคะแนน
}

[System.Serializable]
public class PlayerDatabase 
{
    public List<PlayerData> players = new List<PlayerData>();
}

public class SimpleDatabaseManager : MonoBehaviour 
{
    public static SimpleDatabaseManager Instance; // ทำให้เรียกใช้ได้จากทุกที่

    [Header("UI References")]
    public TMP_InputField nameInput; 
    
    private string savePath;
    private PlayerDatabase db;

    // ข้อมูลชั่วคราวสำหรับเก็บค่าระหว่างเล่น
    private string tempPlayerName;
    private float totalPlayTimeCounter;
    private bool isTimerRunning = false;

    void Awake() 
    {
        // ระบบ Singleton: ทำให้ Object นี้อยู่ยาวตั้งแต่หน้าเมนูจนจบเกม
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        } else {
            Destroy(gameObject);
        }
    }

    void Start() 
    {
        savePath = Application.persistentDataPath + "/ArcadeDatabase.json";
        LoadDatabase();
    }

    void Update()
    {
        // ถ้าตัวจับเวลาทำงานอยู่ ให้บวกเวลาไปเรื่อยๆ
        if (isTimerRunning)
        {
            totalPlayTimeCounter += Time.deltaTime;
        }
    }

    // --- ฟังก์ชันสำหรับหน้า NamingScene ---
    
    public void StartGameRecord() 
    {
        // 1. เก็บชื่อจาก InputField (ถ้าว่างจะใส่ว่า Player)
        string newName = nameInput.text;
        tempPlayerName = string.IsNullOrEmpty(newName) ? "Player" : newName;

        // 2. เริ่มนับเวลาใหม่
        totalPlayTimeCounter = 0;
        isTimerRunning = true;

        // 3. เปลี่ยนไปฉากเริ่มเกม (Scene1) ตามลำดับที่คุณตั้งไว้
        SceneManager.LoadScene("Scene1"); 
    }

    // --- ฟังก์ชันสำหรับหน้าจบเกม (Scene 3) ---

    public void FinishAndSaveGame() 
    {
        isTimerRunning = false; // หยุดเวลาทันทีเมื่อจบเกม

        // เพิ่มข้อมูลลงใน Database
        db.players.Add(new PlayerData { 
            playerName = tempPlayerName, 
            totalPlayTime = totalPlayTimeCounter 
        });

        // บันทึกเป็น JSON ลงเครื่อง
        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(savePath, json);
        
        Debug.Log("บันทึกสถิติสำเร็จ! ชื่อ: " + tempPlayerName + " เวลา: " + totalPlayTimeCounter + " วินาที");
    }

    void LoadDatabase() 
    {
        if (File.Exists(savePath)) {
            string json = File.ReadAllText(savePath);
            db = JsonUtility.FromJson<PlayerDatabase>(json);
        } else {
            db = new PlayerDatabase();
        }
    }
}