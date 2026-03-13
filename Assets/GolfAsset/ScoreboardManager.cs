using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

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

    string result = "<size=120%>TOP 8 SCORES</size>\n\n";
    
    int rank = 1;

    // เติม .Take(8) ตรงนี้ เพื่อบังคับให้เอามาแค่ 8 คนจริงๆ
    foreach (PlayerData p in playerList.Take(8)) 
    {
        int min = (int)p.totalPlayTime / 60;
        int sec = (int)p.totalPlayTime % 60;

        // ลบ </align> ออกเพื่อให้โค้ดสะอาด และเปลี่ยนเป็น \n เพื่อขึ้นบรรทัดใหม่
        result += $"{rank}. {p.playerName}<pos=80%>{min:00}:{sec:00}\n";
        
        rank++; 
    }

    scoreText.text = result;
}

    // ฟังก์ชันสำหรับปุ่ม Back
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }
}