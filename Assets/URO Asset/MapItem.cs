using UnityEngine;
using TMPro; 

public class MapItem : MonoBehaviour 
{
    [Header("--- ระบบประตูคำถาม ---")]
    public QuestionUIManager uiManager; 
    public RiddleData myRiddle; 
    private bool isQuestionBoard = false; 

    [Header("--- ระบบกระดานคำใบ้ (Note) ---")]
    public string myHint; 
    public GameObject noteUI;           
    public TextMeshProUGUI contentText; 
    
    private bool isPlayerInRange = false;
    public bool isCollected = false; 
    private bool isReading = false;   

    public void SetupQuestion(RiddleData riddleData) 
    {
        myRiddle = riddleData;
        isQuestionBoard = true;
    }

    public void SetupHint(string hintText) 
    {
        myHint = hintText;
        isQuestionBoard = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isQuestionBoard)
            {
                isPlayerInRange = true;
                Debug.Log("กด E เพื่อโต้ตอบกับประตู");
            }
            else if (!isQuestionBoard && !isCollected)
            {
                isPlayerInRange = true;
                Debug.Log("กด Q เพื่อเก็บคำใบ้");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isQuestionBoard && Input.GetKeyDown(KeyCode.E))
        {
            if (isPlayerInRange && myRiddle != null)
            {
                if (uiManager != null) 
                {
                    uiManager.OpenQuestion(myRiddle);
                }
            }
        }

        if (!isQuestionBoard && Input.GetKeyDown(KeyCode.Q))
        {
            if (isPlayerInRange && !isCollected)
            {
                CollectNote();
            }
            else if (isCollected && !isReading)
            {
                OpenNote();
            }
            else if (isCollected && isReading)
            {
                CloseNote();
            }
        }
    }

   void CollectNote()
    {
        isCollected = true;
        isPlayerInRange = false; 

        //ค้นหา MeshRenderer แล้วสั่งปิดทีละตัว
        MeshRenderer[] allMeshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in allMeshes)
        {
            mesh.enabled = false;
        }

        //ปิดกรอบชน เพื่อไม่ให้เดินชนซ้ำ
        if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = false; 

        Debug.Log("เก็บโน้ตคำใบ้แล้ว! (กด Q เพื่อเปิดอ่าน)");
    }

    void OpenNote()
    {
        isReading = true;
        if (contentText != null) contentText.text = myHint;
        if (noteUI != null) noteUI.SetActive(true); 
    }

    void CloseNote()
    {
        isReading = false;
        if (noteUI != null) noteUI.SetActive(false); 
    }

    public void RemoveFromPlayer()
    {
        if (!isQuestionBoard && isCollected)
        {
            if (isReading) CloseNote(); 
            Destroy(gameObject); 
        }
    }
    
    public void ForceDestroyNote()
    {
        if (!isQuestionBoard)
        {
            if (isReading) CloseNote(); 
            Debug.Log("ทำลายคำใบ้พร้อมกำแพงแล้ว!");
            Destroy(gameObject); 
        }
    }
}