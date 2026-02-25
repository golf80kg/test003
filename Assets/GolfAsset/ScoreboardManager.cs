using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreboardManager : MonoBehaviour 
{
    [Header("UI Reference")]
    public TextMeshProUGUI scoreText; 

    void Start()
    {
        // 1. ปรับการตั้งค่าเมาส์ให้โผล่มาคลิกปุ่มได้
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;                  

        // 2. ถ้ามี Database ให้ดึงคะแนนมาโชว์ทันที
        if (SimpleDatabaseManager.Instance != null)
        {
            DisplayScores();
        }
    }

    public void DisplayScores()
    {
        List<PlayerData> playerList = SimpleDatabaseManager.Instance.GetPlayerList();
        
        if (playerList == null || playerList.Count == 0)
        {
            scoreText.text = "ยังไม่มีข้อมูลการเล่น";
            return;
        }

        string result = "--- TOP SCORES ---\n";
        foreach (PlayerData p in playerList)
        {
            int min = (int)p.totalPlayTime / 60;
            int sec = (int)p.totalPlayTime % 60;
            result += $"{p.playerName} : {min:00}:{sec:00}\n";
        }
        scoreText.text = result;
    }

    // ฟังก์ชันสำหรับปุ่ม Back
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }
}