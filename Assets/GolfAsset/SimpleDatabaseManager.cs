using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class PlayerData 
{
    public string playerName;
    public float totalPlayTime;
}

[System.Serializable]
public class PlayerDatabase 
{
    public List<PlayerData> players = new List<PlayerData>();
}

public class SimpleDatabaseManager : MonoBehaviour 
{
    public static SimpleDatabaseManager Instance;

    [Header("UI References")]
    public TMP_InputField nameInput; 
    
    private string savePath;
    private PlayerDatabase db;

    private string tempPlayerName;
    private float totalPlayTimeCounter;
    private bool isTimerRunning = false;

    // --- เพิ่มตัวแปรสำหรับระบบ Checkpoint ---
    private string lastSceneName; 

    void Awake() 
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            if (nameInput != null) Instance.nameInput = this.nameInput;
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        nameInput = FindObjectOfType<TMP_InputField>();

        // --- ระบบ Checkpoint: บันทึกชื่อฉากล่าสุดที่เข้าไปเล่น ---
        // เราจะไม่บันทึกหน้าเมนู หน้าตาย หรือหน้ากรอกชื่อ
        if (scene.name != "MainMenu" && scene.name != "NamingScene" && scene.name != "GameOverScene")
        {
            lastSceneName = scene.name;
            Debug.Log("<color=cyan>Checkpoint:</color> บันทึกฉากล่าสุดแล้ว -> " + lastSceneName);
        }
    }

    void Start() 
    {
        savePath = Application.persistentDataPath + "/ArcadeDatabase.json";
        LoadDatabase();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            totalPlayTimeCounter += Time.deltaTime;
        }
    }

    // --- ฟังก์ชันควบคุมเวลา (ใช้เรียกจากสคริปต์ตาย/ปุ่ม Retry) ---
    public void SetTimerRunning(bool running)
    {
        isTimerRunning = running;
    }

    // --- ฟังก์ชันดึงชื่อฉากล่าสุด (ใช้เรียกจากปุ่ม Retry) ---
    public string GetLastSceneName()
    {
        // ถ้าไม่มีค่าบันทึกไว้ (เผื่อกรณีฉุกเฉิน) ให้ส่ง Scene1 กลับไป
        return string.IsNullOrEmpty(lastSceneName) ? "Scene1" : lastSceneName;
    }

    public void GoToNamingScene()
    {
        SceneManager.LoadScene("NamingScene"); 
    }

    public void StartGameRecord() 
{
    if (nameInput != null)
    {
        string newName = nameInput.text.Trim(); // ใช้ Trim เพื่อตัดช่องว่าง
        
        // ถ้าชื่อว่างเปล่าจริงๆ ไม่ควรให้โหลดฉากต่อ
        if (string.IsNullOrEmpty(newName)) 
        {
            Debug.LogError("StartGameRecord ถูกเรียกโดยไม่มีชื่อผู้เล่น!");
            return; 
        }
        
        tempPlayerName = newName;
    }
    // ... ส่วนที่เหลือของโค้ด ...
    totalPlayTimeCounter = 0; 
    isTimerRunning = true;
    SceneManager.LoadScene("Scene1"); 
}

    public void FinishAndSaveGame() 
    {
        isTimerRunning = false;
        LoadDatabase();
        db.players.Add(new PlayerData { playerName = tempPlayerName, totalPlayTime = totalPlayTimeCounter });
        SaveToDisk();
    }

    private void SaveToDisk()
    {
        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(savePath, json);
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

    public List<PlayerData> GetPlayerList() { LoadDatabase(); return db.players.OrderBy(p => p.totalPlayTime).ToList(); }

    public void BackToMenu()
    {
        isTimerRunning = false;
        SceneManager.LoadScene(0); 
    }

    private void OnDestroy() { SceneManager.sceneLoaded -= OnSceneLoaded; }
}