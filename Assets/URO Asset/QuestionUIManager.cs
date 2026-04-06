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
    public MapItem noteToDestroy; 
    [Header("ตั้งค่าแมพให้หายไป")]
    public GameObject MapSystem; 

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
        
        questionText.text = riddle.text; //ข้อความจากRiddletextไปแสดงบนUI
        originalQuestion = riddle.text; 
        correctIndex = riddle.correctAnswerIndex; 
        isCooldown = false; 
        // loop วนซ้ำ  สำหรับ 4 ปุ่ม
        for (int i = 0; i < 4; i++)
        {
            if (i < riddle.choices.Length)//กันeror
            {
                choiceTexts[i].text = riddle.choices[i];
            }
        }
    }

    public void CheckAnswer(int selectedIndex)
    {   //ถ้าcooldownอยู่ให้หยุด ไม่ให้กดซ้ำ
        if (isCooldown) return;
        //เทียบปุ่มที่กดกับที่คำตอบที่ถูก
        if (selectedIndex == correctIndex)
        {
            Debug.Log("ตอบถูก! ผ่านได้");
            
            if (doorToOpen != null)
            {
                doorToOpen.SetActive(false); 
            }
            
            if (noteToDestroy != null)
            {
                noteToDestroy.ForceDestroyNote();
            }
            
            if (MapSystem != null)
            {
                MapSystem.SetActive(false);
                Debug.Log("ปิดแมพของเพื่อนเรียบร้อย!");
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

        //ล็อคเมาส์เวลากดResume หน้าPause
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if(player != null) player.ReturnToGame();
    }

    private IEnumerator WrongAnswerCooldown()
    {
        isCooldown = true; //ป้องกัน player กดซ้ำระหว่างรอ
        float remainingTime = cooldownTime; //เก็บเวลาไว้ในremainingtime

        while (remainingTime > 0)//วนลูปถ้้าเวลาเหลือ
        {
            questionText.text = "<color=red>ERROR</color> " + Mathf.CeilToInt(remainingTime) + "";
            remainingTime -= Time.deltaTime;
            yield return null; 
        }

        questionText.text = originalQuestion;
        isCooldown = false;
    }
}