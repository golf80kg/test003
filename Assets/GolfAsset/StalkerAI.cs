using UnityEngine;
using UnityEngine.AI;

public class StalkerAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;
    public Animator anim; // <--- ช่องนี้จะโผล่มาให้ลากแล้วครับ!
    
    public float viewDistance = 15f; 
    public float wanderRadius = 10f; 
    public float aiSpeedWander = 2f; 
    public float aiSpeedChase = 5f;  

    private Vector3 lastKnownPosition;
    private bool isChasing = false;
    private bool investigatingLastPos = false;

    void Update()
    {
        // สั่งให้อนิเมชั่นทำงานตามความเร็วที่ AI เดินจริง
        if (anim != null)
        {
            anim.SetFloat("Speed", ai.velocity.magnitude);
        }

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
        // ปรับจุดยิง Ray ให้สูงขึ้นนิดหน่อยจะได้ไม่ติดพื้น
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
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        ai.SetDestination(hit.position);
    }
}