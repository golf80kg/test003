using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables; // เพิ่มเข้ามาเพื่อให้โค้ดรู้จักคำสั่งเล่น Timeline

public class AiLook : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;      
    public Camera playerCam;
    public float aiSpeed = 5f;    
    
    [Header("Death Settings")]
    public float killDistance = 1.8f; 
    public AudioClip deathScream;    
    public GameObject jumpScareUI;   

    [Header("Sequencer Settings")]
    public PlayableDirector deathSequence; // ช่องสำหรับใส่ DeathCamera ที่มี Timeline
    public GameObject mainPlayer;          // ตัวละคร Capsule ของเรา
    public GameObject deathCameraObj;      // กล้องในห้องตาย

    private Renderer aiRenderer;
    private bool isDead = false;

    void Start()
    {
        if (playerCam == null) playerCam = Camera.main;

        // ปรับให้หา Renderer จากตัวลูกได้แม่นยำขึ้น
        aiRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (aiRenderer == null) aiRenderer = GetComponentInChildren<Renderer>();
        
        // ปิดกล้องห้องตายไว้ก่อนตอนเริ่มเกม
        if (deathCameraObj != null) deathCameraObj.SetActive(false);
    }

    void Update()
    {
        if (isDead || aiRenderer == null) return; 

        float distance = Vector3.Distance(transform.position, player.position);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam); 

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
        ai.isStopped = true;
        ai.speed = 0;

        // 1. ปิดตัวผู้เล่น เพื่อไม่ให้เห็น Capsule ยืนค้างในด่าน
        if (mainPlayer != null) mainPlayer.SetActive(false);

        // 2. เปิดกล้องห้องตายที่จัดฉากไว้
        if (deathCameraObj != null) deathCameraObj.SetActive(true);

        // 3. สั่งให้ Timeline เริ่มเล่น (ทั้งภาพพุ่งและเสียง)
        if (deathSequence != null) deathSequence.Play();

        // เล่นเสียงเสริม (ถ้ามีใส่ไว้)
        if (deathScream != null)
        {
            AudioSource.PlayClipAtPoint(deathScream, transform.position);
        }

        if (jumpScareUI != null) jumpScareUI.SetActive(true);

        // รอ 3 วินาทีเพื่อให้ Timeline เล่นจนจบ แล้วค่อยกลับหน้าเมนู
        Invoke("GameOver", 3.0f);
    }

    void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}