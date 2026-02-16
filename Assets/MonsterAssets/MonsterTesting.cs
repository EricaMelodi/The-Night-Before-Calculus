using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // Player Transform
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Speed Settings")]
    public float walkSpeed = 2f;       // Speed when roaming
    public float runSpeed = 5f;        // Speed when chasing player

    [Header("Roaming Settings")]
    public float roamRadius = 20f;
    public float roamWaitTime = 3f;

    [Header("Chase Settings")]
    public float chaseRadius = 10f;

    private Vector3 roamTarget;
    private float waitTimer;

    [Header("Sounds")]
    public AudioSource chaseMusicSource;
    public AudioClip chaseMusicClip;
    public float musicVolume;
    private bool musicPlaying;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = walkSpeed; // Default to walking
        ChooseRoamTarget();
        musicPlaying = false;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRadius)
        {
            // Chase the player
            agent.speed = runSpeed; // Set agent speed to running
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);

            PlayChaseMusic();
        }
        else
        {
            // Roaming
            agent.speed = walkSpeed; // Set agent speed to walking
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

            StopChaseMusic();
        }

        // Optional: update animator speed for blend tree
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

    private void PlayChaseMusic()
    {
        if (!musicPlaying)
        {
            chaseMusicSource.clip = chaseMusicClip;
            chaseMusicSource.loop = true;
            chaseMusicSource.volume = musicVolume;
            chaseMusicSource.Play();
            musicPlaying = true;
        }
    }


    private void StopChaseMusic()
    {
        if (musicPlaying)
        {
            chaseMusicSource.Stop();
            musicPlaying = false;
        }
    }
}
