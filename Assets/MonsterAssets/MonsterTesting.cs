
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;  
using TMPro;


public class MonsterAI : MonoBehaviour
{

    [Header("Jumpscare")]
    public string jump = "JumpScareScene";


    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("UI")]
    public TextMeshProUGUI runText;
    public float shakeAmount = 10f;

    private Vector2 originalTextPosition;

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

        if (runText != null)
        {
            originalTextPosition = runText.rectTransform.anchoredPosition;
            runText.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < 10f && isHunting)
        {
            PlayerCaught();
        }

        if (isHunting)
        {
            huntTimer -= Time.deltaTime;
            if (huntTimer <= 0f)
            {
                isHunting = false;
            }
        }

        if (isHunting || distanceToPlayer <= chaseRadius)
        {
            agent.speed = runSpeed;
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);

            PlayChaseMusic();

            if (runText != null)
            {
                runText.gameObject.SetActive(true);
                ShakeText();
            }
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

            if (runText != null)
            {
                runText.gameObject.SetActive(false);
                runText.rectTransform.anchoredPosition = originalTextPosition;
            }
        }

        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    public void TriggerHunt()
    {
        isHunting = true;
        huntTimer = huntDuration;
    }

    private void ShakeText()
    {
        Vector2 randomOffset = Random.insideUnitCircle * shakeAmount;
        runText.rectTransform.anchoredPosition = originalTextPosition + randomOffset;
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
        SceneManager.LoadScene(jump);
    }

}