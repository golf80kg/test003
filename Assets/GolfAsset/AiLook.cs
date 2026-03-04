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
    [HideInInspector] public PlayableDirector deathSequence; 
    [HideInInspector] public GameObject deathCameraObj;
    public GameObject mainPlayer;          

    private Renderer aiRenderer;
    private bool isDead = false;

    void Start()
    {
        if (playerCam == null) playerCam = Camera.main;

        deathCameraObj = GameObject.Find("DeathCamera");
        if (deathCameraObj != null)
        {
            deathSequence = deathCameraObj.GetComponent<PlayableDirector>();
            deathCameraObj.SetActive(false);
            Debug.Log("<color=green>AI:</color> พบระบบ DeathCamera อัตโนมัติแล้ว!");
        }
        else
        {
            Debug.LogError("<color=red>AI Error:</color> ไม่พบวัตถุชื่อ 'DeathCamera' ในฉากนี้!");
        }

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
        if (direction != Vector3.zero) transform.rotation = Quaternion.LookRotation(direction);
    }

    void CaughtPlayer()
    {
        isDead = true;
        ai.enabled = false; 

        // 1. สั่งหยุดเวลาใน Database ทันทีที่โดนจับ
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
            float waitTime = 3.0f;
            if (deathSequence != null) 
            {
                deathSequence.Play();
                waitTime = (float)deathSequence.duration;
            }

            if (deathScream != null) AudioSource.PlayClipAtPoint(deathScream, deathCameraObj.transform.position);

            Invoke("GoToGameOverScene", waitTime);
        }
        else
        {
            Invoke("GoToGameOverScene", 1.0f);
        }
    }

    // ฟังก์ชันใหม่: โหลดเข้า Scene GameOver
    void GoToGameOverScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // เปลี่ยนเป็นชื่อ Scene ตายของคุณ (ต้องมีใน Build Settings)
        SceneManager.LoadScene("GameOverScene"); 
    }
}