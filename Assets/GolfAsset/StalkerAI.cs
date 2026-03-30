using UnityEngine;
using UnityEngine.AI;

public class StalkerAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;
    public Animator anim; 
    
    [Header("AI Settings")]
    public float viewDistance = 15f; 
    public float wanderRadius = 10f; 
    public float aiSpeedWander = 2f; 
    public float aiSpeedChase = 5f;  

    private Vector3 lastKnownPosition;
    private bool isChasing = false;
    private bool investigatingLastPos = false;

    void Update()
    {
        // --- ส่วนที่ตัดออก: ไม่ต้องสั่ง anim.SetFloat("Speed") แล้ว ---
        // เพื่อให้มันเล่นท่าทางที่ตั้งไว้ใน Animator (Entry State) ตลอดเวลา

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < viewDistance && CanSeePlayer())
        {
            StartChasing();
        }
        else if (isChasing)
        {
            LostPlayer();
        }

        if (!isChasing && !investigatingLastPos && !ai.pathPending && ai.remainingDistance < 0.5f)
        {
            Wander();
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = player.position - transform.position;
        
        // ยิง Ray จากระดับอก (สูงขึ้น 1 เมตร) เพื่อเช็กว่าเห็นผู้เล่นไหม
        if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, viewDistance))
        {
            if (hit.transform.CompareTag("Player")) return true;
        }
        return false;
    }

    void StartChasing()
    {
        isChasing = true;
        investigatingLastPos = false;
        ai.speed = aiSpeedChase;
        ai.SetDestination(player.position);
        lastKnownPosition = player.position;
    }

    void LostPlayer()
    {
        isChasing = false;
        investigatingLastPos = true;
        ai.SetDestination(lastKnownPosition);
        
        // หยุดเดินหาหลังจากผ่านไป 3 วินาที
        CancelInvoke("FinishInvestigating"); 
        Invoke("FinishInvestigating", 3f); 
    }

    void FinishInvestigating()
    {
        investigatingLastPos = false;
    }

    void Wander()
    {
        ai.speed = aiSpeedWander;
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        
        // สุ่มหาพิกัดบนพื้นสีฟ้า (NavMesh)
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            ai.SetDestination(hit.position);
        }
    }
}