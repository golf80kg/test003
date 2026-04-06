using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class AiLook : MonoBehaviour
{
    public NavMeshAgent ai;         // ทำให้ผีเดินหาเส้นทางอัตโนมัติบน NavMesh
    public Transform player;      
    public Camera playerCam;
    public float aiSpeed = 5f;    
    
    [Header("Death Settings")]
    public float killDistance = 1.8f; 
    public AudioClip deathScream;    
    public GameObject jumpScareUI;   

    [Header("Sequencer Settings")]
    public GameObject deathCameraObj;
    [HideInInspector] public PlayableDirector deathSequence; 
    public GameObject mainPlayer;          

    // Renderer ของผี — เก็บไว้เพื่อใช้เช็คทุก Frame ว่าผีอยู่ในมุมมองกล้องไหม
    // เป็น private เพราะจัดการภายในโค้ดเอง ไม่ต้อง assign จาก Inspector
    private Renderer aiRenderer;
    private bool isDead = false;

    void Start()
    {
        if (playerCam == null) playerCam = Camera.main;

        if (deathCameraObj == null)
        {
            deathCameraObj = GameObject.Find("DeathCamera");
        }

        if (deathCameraObj != null)
        {
            deathSequence = deathCameraObj.GetComponent<PlayableDirector>();
            deathCameraObj.SetActive(false); // ซ่อนกล้องฉากตายไว้ก่อน รอจนผู้เล่นตายค่อยเปิด
            Debug.Log("<color=green>AI (" + gameObject.name + "):</color> เชื่อมต่อระบบ DeathCamera เรียบร้อย!");
        }
        else
        {
            Debug.LogError("<color=red>AI Error (" + gameObject.name + "):</color> ไม่พบวัตถุที่จะใช้เป็นกล้องตอนตาย! โปรดลากใส่ใน Inspector");
        }

        // ลองหา SkinnedMeshRenderer ก่อน (ใช้กับโมเดลที่มี Animation/กระดูก)
        // ถ้าไม่มีค่อยใช้ Renderer ทั่วไป (วัตถุธรรมดาไม่มี Animation)
        aiRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (aiRenderer == null) aiRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (isDead || aiRenderer == null) return; 

        float distance = Vector3.Distance(transform.position, player.position);

        // CalculateFrustumPlanes = คำนวณ 6 ด้านของรูปทรงปิรามิด (Frustum) ที่แทนมุมมองกล้อง
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);

        // TestPlanesAABB = เช็คว่า Renderer ของผีอยู่ในกรอบปิรามิดนั้นไหม
        
        if (GeometryUtility.TestPlanesAABB(planes, aiRenderer.bounds))
        {
            StopEnemy();
        }
        else
        {
            MoveToPlayer(distance);
        }
    }

    void StopEnemy()
    {
        if (ai.isOnNavMesh)
        {
            ai.isStopped = true;
            ai.speed = 0;
            ai.velocity = Vector3.zero; // หยุดทันที ป้องกันการลื่นไถลต่อ
        }
    }

    void MoveToPlayer(float distance)
    {
        if (ai.isOnNavMesh)
        {
            ai.isStopped = false;
            ai.speed = aiSpeed;
            ai.SetDestination(player.position); // NavMesh หาเส้นทางให้อัตโนมัติ
            LookAtPlayer();
        }

        if (distance <= killDistance)
        {
            CaughtPlayer();
        }
    }

    void LookAtPlayer()
    {
        // ลบเวกเตอร์สองจุด = ได้ทิศทางจากผีไปหาผู้เล่น
        Vector3 direction = player.position - transform.position;

        // ตัดแกน Y ออก ป้องกันผีก้มหรือแหงนหน้าเวลาผู้เล่นอยู่บันได
        direction.y = 0;

        // เช็ค != zero ก่อน เพราะถ้าผีกับผู้เล่นอยู่จุดเดียวกัน direction = zero แล้ว LookRotation จะ Error
        // Quaternion.LookRotation = แปลงทิศทางเป็นค่าการหมุน แล้วหันหน้าผีไปทางนั้น
        if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);
    }

    void CaughtPlayer()
    {
        if (isDead) return;
        isDead = true;
        ai.enabled = false; 

        if (SimpleDatabaseManager.Instance != null)
        {
            SimpleDatabaseManager.Instance.SetTimerRunning(false);
        }

        if (playerCam != null) playerCam.gameObject.SetActive(false);
        if (mainPlayer != null) mainPlayer.SetActive(false);
        if (jumpScareUI != null) jumpScareUI.SetActive(true); 

        if (deathCameraObj != null) 
        {
            deathCameraObj.SetActive(true);
            float waitTime = 3.0f; // ถ้าไม่มี Timeline
            if (deathSequence != null) 
            {
                deathSequence.Play();
                waitTime = (float)deathSequence.duration; // ใช้ความยาว Timeline จริงๆ
            }

            // PlayClipAtPoint = เล่นเสียง ณ ตำแหน่งใน 3D Space
            // สร้าง AudioSource ชั่วคราวแล้วทำลายทิ้งเองอัตโนมัติหลังเสียงจบ
            if (deathScream != null) AudioSource.PlayClipAtPoint(deathScream, deathCameraObj.transform.position);

            // Invoke = เรียกฟังก์ชันแบบหน่วงเวลา รอให้ Timeline เล่นจบก่อนค่อยโหลด Scene ใหม่
            Invoke("GoToGameOverScene", waitTime);
        }
        else
        {
            Invoke("GoToGameOverScene", 1.0f); // ถ้าไม่มี DeathCamera รอแค่ 1 วิแล้วไปเลย
        }
    }

    void GoToGameOverScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOverScene"); 
    }
}