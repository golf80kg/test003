using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AiLook : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;      
    public Camera playerCam;
    public float aiSpeed = 5f;    
    
    [Header("Death Settings")]
    public float killDistance = 1.8f; // เพิ่มระยะให้กว้างขึ้นเล็กน้อยเพื่อความชัวร์
    public AudioClip deathScream;    
    public GameObject jumpScareUI;   

    private Renderer aiRenderer;
    private bool isDead = false;

    void Start()
    {
        // หากล้องหลักอัตโนมัติถ้าลืมลากใส่
        if (playerCam == null) playerCam = Camera.main;

        // หา Skinned Mesh Renderer จากตัวลูก
        aiRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        
        if (aiRenderer == null) {
            Debug.LogError("หาโมเดลผีไม่เจอ! กรุณาเช็กว่าโมเดลมี Skinned Mesh Renderer หรือไม่");
        }
    }

    void Update()
    {
        if (isDead || aiRenderer == null) return; 

        // คำนวณระยะห่างระหว่าง "เท้า" ของทั้งคู่ (แม่นยำกว่า)
        float distance = Vector3.Distance(transform.position, player.position);
        
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam); 

        // ตรวจสอบการมองเห็น
        if (GeometryUtility.TestPlanesAABB(planes, aiRenderer.bounds))
        {
            ai.speed = 0;
            ai.isStopped = true; 
        }
        else
        {
            ai.isStopped = false;
            ai.speed = aiSpeed;
            ai.SetDestination(player.position);
            LookAtPlayer();

            // ถ้าเข้าใกล้มากพอในขณะที่ผู้เล่นไม่มอง
            if (distance <= killDistance)
            {
                CaughtPlayer();
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; 
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void CaughtPlayer()
    {
        isDead = true;
        
        // หยุดการเคลื่อนที่ทันที
        ai.isStopped = true;
        ai.speed = 0;

        // 1. กระชากกล้องไปที่หน้าผี (บวกความสูงขึ้นไป 1 เมตรเพื่อให้จ้องหน้าพอดี)
        playerCam.transform.LookAt(transform.position + Vector3.up * 1.5f);
        
        // 2. เล่นเสียง (ใช้ PlayClipAtPoint เพื่อให้เสียงดังที่ตัวผู้เล่น)
        if (deathScream != null)
        {
            AudioSource.PlayClipAtPoint(deathScream, playerCam.transform.position);
        }

        if (jumpScareUI != null) jumpScareUI.SetActive(true);

        Invoke("GameOver", 2.0f);
    }

    void GameOver()
    {
        SceneManager.LoadScene(0); 
    }
}