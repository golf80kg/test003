using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

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
    // ไม่ต้องลากใส่ใน Inspector แล้ว โค้ดจะหาเองจากชื่อวัตถุ
    [HideInInspector] public PlayableDirector deathSequence; 
    [HideInInspector] public GameObject deathCameraObj;
    public GameObject mainPlayer;          

    private Renderer aiRenderer;
    private bool isDead = false;

    void Start()
    {
        if (playerCam == null) playerCam = Camera.main;

        // --- ระบบค้นหากล้องและ Timeline อัตโนมัติ ---
        // ค้นหาวัตถุในฉากที่ชื่อว่า "DeathCamera" (ต้องตั้งชื่อให้ตรงเป๊ะ)
        deathCameraObj = GameObject.Find("DeathCamera");

        if (deathCameraObj != null)
        {
            // ดึง PlayableDirector มาจากวัตถุที่เจอ
            deathSequence = deathCameraObj.GetComponent<PlayableDirector>();
            // ปิดกล้องไว้ก่อนเริ่มเกม
            deathCameraObj.SetActive(false);
            Debug.Log("<color=green>AI:</color> พบระบบ DeathCamera อัตโนมัติแล้ว!");
        }
        else
        {
            Debug.LogError("<color=red>AI Error:</color> ไม่พบวัตถุชื่อ 'DeathCamera' ในฉากนี้! กรุณาเช็คชื่อวัตถุด้วย");
        }
        // ------------------------------------------

        aiRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (aiRenderer == null) aiRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (isDead || aiRenderer == null) return; 

        float distance = Vector3.Distance(transform.position, player.position);
        
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam); 
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
        ai.isStopped = true;
        ai.speed = 0;
        ai.velocity = Vector3.zero;
    }

    void MoveToPlayer(float distance)
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
        ai.enabled = false; 

        if (playerCam != null) playerCam.gameObject.SetActive(false);
        if (mainPlayer != null) mainPlayer.SetActive(false);
        if (jumpScareUI != null) jumpScareUI.SetActive(true);

        if (deathCameraObj != null) 
        {
            deathCameraObj.SetActive(true);
            
            // เช็คว่ามี Timeline ให้เล่นไหม ถ้ามีให้เล่นจนจบ ถ้าไม่มีให้รอ 3 วิ
            float waitTime = 3.0f;
            if (deathSequence != null) 
            {
                deathSequence.Play();
                waitTime = (float)deathSequence.duration;
            }

            if (deathScream != null)
            {
                AudioSource.PlayClipAtPoint(deathScream, deathCameraObj.transform.position);
            }

            Invoke("GameOver", waitTime);
        }
        else
        {
            // ถ้าหากล้องไม่เจอจริงๆ ให้ตัดเข้าเมนูเลยใน 1 วิเพื่อไม่ให้เกมค้าง
            Invoke("GameOver", 1.0f);
        }
    }

    void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}