using UnityEngine;
using UnityEngine.Playables; 
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    private PlayableDirector timeline;
    public string gameSceneName = "Scene1"; // <--- เปลี่ยนตรงนี้ให้ตรงกับชื่อไฟล์ Scene ด่าน 1 ของคุณเป๊ะๆ นะครับ

    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
        if (timeline != null)
        {
            // สั่งว่าถ้า Timeline เล่นจบ ให้เรียกฟังก์ชัน GoToGame
            timeline.stopped += GoToGame;
        }
    }

    void Update()
    {
        // เผื่อให้ผู้เล่นกด ESC ข้ามคัทซีนได้
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Escape))
        {
            GoToGame(timeline);
        }
    }

    void GoToGame(PlayableDirector pd)
    {
        SceneManager.LoadScene(gameSceneName);
    }
}