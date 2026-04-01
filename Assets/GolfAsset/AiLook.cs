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
    // เอา [HideInInspector] ออกเพื่อให้ลากใส่เองได้จากหน้า Inspector
    public GameObject deathCameraObj;
    [HideInInspector] public PlayableDirector deathSequence; 
    public GameObject mainPlayer;          

    private Renderer aiRenderer;
    private bool isDead = false;

    void Start()
    {
        if (playerCam == null) playerCam = Camera.main;

        // ถ้าเราไม่ได้ลากใส่เองใน Inspector ให้พยายามหาชื่อ "DeathCamera" อัตโนมัติ
        if (deathCameraObj == null)
        {
            deathCameraObj = GameObject.Find("DeathCamera");
        }

        // ตรวจสอบผลลัพธ์
        if (deathCameraObj != null)
        {
            deathSequence = deathCameraObj.GetComponent<PlayableDirector>();
            deathCameraObj.SetActive(false);
            Debug.Log("<color=green>AI (" + gameObject.name + "):</color> เชื่อมต่อระบบ DeathCamera เรียบร้อย!");
        }
        else
        {
            Debug.LogError("<color=red>AI Error (" + gameObject.name + "):</color> ไม่พบวัตถุที่จะใช้เป็นกล้องตอนตาย! โปรดลากใส่ใน Inspector");
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
        if (ai.isOnNavMesh)
        {
            ai.isStopped = true;
            ai.speed = 0;
            ai.velocity = Vector3.zero;
        }
    }

    void MoveToPlayer(float distance)
    {
        if (ai.isOnNavMesh)
        {
            ai.isStopped = false;
            ai.speed = aiSpeed;
            ai.SetDestination(player.position);
            LookAtPlayer();
        }

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
        if (isDead) return;
        isDead = true;
        ai.enabled = false; 

        // 1. สั่งหยุดเวลาใน Database
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

    void GoToGameOverScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOverScene"); 
    }
}