using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

// --- เพิ่มส่วนนี้กลับเข้าไปครับ ---
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
// -----------------------------

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

    // --- ฟังก์ชันควบคุมเวลา ---
    public void SetTimerRunning(bool running)
    {
        isTimerRunning = running;
    }

    public void GoToNamingScene()
    {
        SceneManager.LoadScene("NamingScene"); 
    }

    // ฟังก์ชันเริ่มเกม (จากหน้ากรอกชื่อ)
    public void StartGameRecord() 
    {
        if (nameInput != null)
        {
            string newName = nameInput.text;
            tempPlayerName = string.IsNullOrEmpty(newName) ? "Player" : newName;
        }
        else
        {
            var foundInput = FindObjectOfType<TMP_InputField>();
            tempPlayerName = (foundInput != null && !string.IsNullOrEmpty(foundInput.text)) ? foundInput.text : "Guest";
        }

        totalPlayTimeCounter = 0; // รีเซ็ตเวลาเป็น 0 เมื่อเริ่มใหม่จากหน้ากรอกชื่อ
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