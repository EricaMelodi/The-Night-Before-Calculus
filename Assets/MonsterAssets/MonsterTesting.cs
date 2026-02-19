using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;  


public class MonsterAI : MonoBehaviour
{

    [Header("Jumpscare")]
    public string jumpscareSceneName = "JumpScareScene";


    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Speed Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    [Header("Roaming Settings")]
    public float roamRadius = 20f;
    public float roamWaitTime = 3f;

    [Header("Chase Settings")]
    public float chaseRadius = 10f;

    [Header("Hunt Settings")]
    public float huntDuration = 30f;

    private bool isHunting = false;
    private float huntTimer = 0f;

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

        agent.speed = walkSpeed;
        ChooseRoamTarget();
        musicPlaying = false;
    }


    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, chaseRadius);
        //foreach (var hitCollider in hitColliders)
        //{
        //    if (hitCollider.CompareTag("Player") && isHunting)
        //    {
        //        PlayerCaught();
        //        break; // Exit after catching the player
        //    }
        //}

        if (distanceToPlayer < 20f && isHunting)
        {
            PlayerCaught();
        }

        // Handle Hunt Timer
        if (isHunting)
        {
            huntTimer -= Time.deltaTime;
            if (huntTimer <= 0f)
            {
                isHunting = false;
            }
        }

        // Chase if hunting OR player in radius
        if (isHunting || distanceToPlayer <= chaseRadius)
        {
            agent.speed = runSpeed;
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);

            PlayChaseMusic();
        }
        else
        {
            // Roaming
            agent.speed = walkSpeed;
            animator.SetBool("isRunning", false);

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

        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    public void TriggerHunt()
    {
        isHunting = true;
        huntTimer = huntDuration;   // Resets to 30 seconds every time
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

    // On your Monster script
    //private void OnCollisionEnter(Collision collision)

    //{
    //    Debug.Log($"Monster collided22   with {collision}");

    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        PlayerCaught();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Monster collided with {other.name} (tag: {other.tag})");

        if (other.CompareTag("Player"))
        {
            PlayerCaught();
        }
    }


    private void PlayerCaught()
    {
        Debug.Log("Player caught! Loading jumpscare...");
        SceneManager.LoadScene(jumpscareSceneName);
    }

}