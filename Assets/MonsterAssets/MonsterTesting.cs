using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // Player Transform
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Roaming Settings")]
    public float roamRadius = 20f;
    public float roamWaitTime = 3f;

    [Header("Chase Settings")]
    public float chaseRadius = 10f;

    private Vector3 roamTarget;
    private float waitTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ChooseRoamTarget();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRadius)
        {
            // Chase the player
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);  // Switch to run animation
        }
        else
        {
            // Roaming
            animator.SetBool("isRunning", false); // Switch to walk animation

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= roamWaitTime)
                {
                    ChooseRoamTarget();
                    waitTimer = 0f;
                }
            }
        }

        // Optional: update animator speed if you have a blend tree
        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    private void ChooseRoamTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            roamTarget = hit.position;
            agent.SetDestination(roamTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}
