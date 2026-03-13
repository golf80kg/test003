using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using System.Collections; 

public class QuestionUIManager : MonoBehaviour
{
    [Header("ส่วนแสดงผล UI")]
    public GameObject questionPanel; 
    public TextMeshProUGUI questionText; 

    [Header("ปุ่มตัวเลือกทั้ง 4")]
    public Button[] choiceButtons; 
    public TextMeshProUGUI[] choiceTexts; 

    [Header("ตั้งค่าระบบคูลดาวน์")] 
    public float cooldownTime = 3.0f; 

    [Header("ตั้งค่ากำแพงและโน้ตที่จะให้หายไป")] 
    public GameObject doorToOpen; 
    public MapItem noteToDestroy; // <--- เพิ่มช่องสำหรับลากสมุดโน้ตมาใส่

    private int correctIndex; 
    private bool isCooldown = false; 
    private string originalQuestion; 

    void Start()
    {
        questionPanel.SetActive(false);
    }

    public void OpenQuestion(RiddleData riddle)
    {
        questionPanel.SetActive(true); 
        
        questionText.text = riddle.text; 
        originalQuestion = riddle.text; 
        correctIndex = riddle.correctAnswerIndex; 
        isCooldown = false; 

        for (int i = 0; i < 4; i++)
        {
            if (i < riddle.choices.Length)
            {
                choiceTexts[i].text = riddle.choices[i];
            }
        }
    }

    public void CheckAnswer(int selectedIndex)
    {
        if (isCooldown) return;

        if (selectedIndex == correctIndex)
        {
            Debug.Log("ตอบถูก! ผ่านได้");
            
            if (doorToOpen != null)
            {
                doorToOpen.SetActive(false); 
            }

            // <--- สั่งประหารโน้ตคำใบ้ตรงนี้ --->
            if (noteToDestroy != null)
            {
                noteToDestroy.ForceDestroyNote();
            }

            CloseQuestion(); 
        }
        else
        {
            Debug.Log("ตอบผิด! ลองใหม่นะ");
            StartCoroutine(WrongAnswerCooldown());
        }
    }

    public void CloseQuestion()
    {
        questionPanel.SetActive(false); 

        // ส่งสัญญาณบอก PlayerController ให้ล็อกเมาส์กลับ (ถ้าคุณใช้ระบบ PauseManager ให้คงบรรทัดนี้ไว้ครับ)
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if(player != null) player.ReturnToGame();
    }

    private IEnumerator WrongAnswerCooldown()
    {
        isCooldown = true; 
        float remainingTime = cooldownTime; 

        while (remainingTime > 0)
        {
            questionText.text = "<color=red>ตอบผิด!</color>\n กรุณารอ " + Mathf.CeilToInt(remainingTime) + " วินาที...";
            remainingTime -= Time.deltaTime;
            yield return null; 
        }

        questionText.text = originalQuestion;
        isCooldown = false;
    }
}