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

    void Awake() 
    {
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
        if (isTimerRunning)
        {
            totalPlayTimeCounter += Time.deltaTime;
        }
    }

    // --- เพิ่มฟังก์ชัน BackToMenu ไว้ที่นี่ครับ ---
    public void BackToMenu()
    {
        // โหลดฉากลำดับที่ 0 (MainMenu)
        SceneManager.LoadScene(0); 
    }

    public void StartGameRecord() 
    {
        if (nameInput != null)
        {
            string newName = nameInput.text;
            tempPlayerName = string.IsNullOrEmpty(newName) ? "Player" : newName;
        }
        else
        {
            tempPlayerName = "Guest";
        }

        totalPlayTimeCounter = 0;
        isTimerRunning = true;

        SceneManager.LoadScene("Scene1"); 
    }

    public void FinishAndSaveGame() 
    {
        isTimerRunning = false;

        db.players.Add(new PlayerData { 
            playerName = tempPlayerName, 
            totalPlayTime = totalPlayTimeCounter 
        });

        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(savePath, json);
        
        Debug.Log("บันทึกสำเร็จที่: " + savePath);
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

    public List<PlayerData> GetPlayerList()
    {
        if (db == null) LoadDatabase();
        return db.players.OrderBy(p => p.totalPlayTime).ToList();
    }
}