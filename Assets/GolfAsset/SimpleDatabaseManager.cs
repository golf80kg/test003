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
    public static SimpleDatabaseManager Instance;//ถ้า GetComponent Ob3D.GetComponent<StalkerAI>(); ต้องอ้างอิงวัตถุ

    private TMP_InputField nameInput; // private เพราะมีแค่ใน NamingScene เท่านั้น
    
    private string savePath;
    private PlayerDatabase db;

    private string tempPlayerName;
    private float totalPlayTimeCounter;
    private bool isTimerRunning = false;
    private string lastSceneName; 

    void Awake() 
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            Destroy(gameObject); // ทำลายตัวซ้ำทิ้ง
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // หา InputField เฉพาะหน้า NamingScene เท่านั้น
        if (scene.name == "NamingScene")
        {
            nameInput = FindObjectOfType<TMP_InputField>();
            if (nameInput != null)
            {
                nameInput.characterLimit = 15;
                Debug.Log("<color=green>System:</color> จำกัดการพิมพ์ชื่อไว้ที่ 15 ตัวอักษรแล้ว");
            }
        }

        if (scene.name != "MainMenu" && scene.name != "NamingScene" && scene.name != "GameOverScene")
        {
            lastSceneName = scene.name;
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

    public void SetTimerRunning(bool running)
    {
        isTimerRunning = running;
    }

    public string GetLastSceneName()
    {
        return string.IsNullOrEmpty(lastSceneName) ? "Scene1" : lastSceneName;
    }

    public void StartGameRecord() 
    {
        if (nameInput != null)
        {
            string newName = nameInput.text.Trim();
            
            if (string.IsNullOrEmpty(newName)) 
            {
                Debug.LogError("กรุณาใส่ชื่อผู้เล่น!");
                return; 
            }

            if (newName.Length > 15)
            {
                newName = newName.Substring(0, 15);
            }
            
            tempPlayerName = newName;
        }
        
        totalPlayTimeCounter = 0; 
        isTimerRunning = true;
        SceneManager.LoadScene("Scene1"); 
    }

    public void FinishAndSaveGame() 
    {
        isTimerRunning = false;
        LoadDatabase();

        float roundedTime = (float)System.Math.Round(totalPlayTimeCounter, 2);

        db.players.Add(new PlayerData { 
            playerName = tempPlayerName, 
            totalPlayTime = roundedTime 
        });

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

    public List<PlayerData> GetPlayerList() 
    { 
        LoadDatabase(); 
        return db.players.OrderBy(p => p.totalPlayTime).ToList(); 
    }

    public void BackToMenu()
    {
        isTimerRunning = false;
        SceneManager.LoadScene(0); 
    }
}