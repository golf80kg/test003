using UnityEngine;
using UnityEngine.AI;

public class StalkerAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;
    public float viewDistance = 15f; // ระยะการมองเห็น
    public float wanderRadius = 10f; // ระยะสุ่มเดิน
    public float aiSpeedWander = 2f; // ความเร็วตอนเดินสุ่ม
    public float aiSpeedChase = 5f;  // ความเร็วตอนวิ่งไล่

    private Vector3 lastKnownPosition;
    private bool isChasing = false;
    private bool investigatingLastPos = false;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 1. เช็กว่าผู้เล่นอยู่ในระยะและไม่มีอะไรบังตาหรือไม่
        if (distanceToPlayer < viewDistance && CanSeePlayer())
        {
            StartChasing();
        }
        else if (isChasing)
        {
            // 2. ถ้าผู้เล่นหายไป ให้ไปที่จุดล่าสุดที่เห็น
            LostPlayer();
        }

        // 3. จัดการการเคลื่อนที่ตามสถานะ
        if (!isChasing && !investigatingLastPos && !ai.pathPending && ai.remainingDistance < 0.5f)
        {
            Wander();
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = player.position - transform.position;
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
        // เมื่อถึงจุดล่าสุดแล้วจะหยุดมอง (เช็กใน Update)
        Invoke("FinishInvestigating", 3f); // หยุดมอง 3 วินาที
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
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        ai.SetDestination(hit.position);
    }
}