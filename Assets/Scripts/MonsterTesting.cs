using UnityEngine;

public class MonsterTesting : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;
    public float rotationSpeed = 3f;
    public float range = 20f;
    public float stop = 0f;

    [Header("Roaming")]
    public float roamRadius = 20f;
    public float roamInterval = 20f;
    private Vector3 roamTarget;
    private float roamTimer = 0f;

    [Header("Forced Chase")]
    public float forcedChaseDuration = 30f;
    public float forcedChaseCooldown = 20f;

    private bool isForcedChasing = false;
    private bool forcedChaseOnCooldown = false;
    private float forcedChaseTimer = 0f;

    private Transform myTransform;
    private bool playerLost = false;
    private Animator anim;

    void Awake()
    {
        if (target == null)
            target = GameObject.FindWithTag("Player").transform;

        myTransform = transform;
        anim = GetComponent<Animator>();

        SetNewRoamTarget();
        anim.SetBool("isWalking", false);
    }

    void Update()
    {
        if (playerLost)
        {
            anim.SetBool("isWalking", false);
            return;
        }

        HandleForcedChaseTimer();

        float distance = Vector3.Distance(myTransform.position, target.position);

        // Forced chase ignores vision range
        if (isForcedChasing)
        {
            ChasePlayer(distance);
        }
        else
        {
            if (distance <= range)
                ChasePlayer(distance);
            else
                Roam();
        }

        if (distance <= 2f)
            LoseGame();
    }

    public void ForceChase()
    {
        if (forcedChaseOnCooldown || playerLost)
            return;

        isForcedChasing = true;
        forcedChaseTimer = forcedChaseDuration;
    }

    void HandleForcedChaseTimer()
    {
        if (!isForcedChasing)
            return;

        forcedChaseTimer -= Time.deltaTime;

        if (forcedChaseTimer <= 0f)
        {
            isForcedChasing = false;
            forcedChaseOnCooldown = true;
            Invoke(nameof(ResetForcedChaseCooldown), forcedChaseCooldown);
        }
    }

    void ResetForcedChaseCooldown()
    {
        forcedChaseOnCooldown = false;
    }

    void ChasePlayer(float distance)
    {
        Vector3 direction = target.position - myTransform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            myTransform.rotation = Quaternion.Slerp(
                myTransform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (distance > stop)
        {
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void Roam()
    {
        roamTimer += Time.deltaTime;

        if (roamTimer >= roamInterval)
        {
            SetNewRoamTarget();
            roamTimer = 0f;
        }

        Vector3 direction = roamTarget - myTransform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            myTransform.rotation = Quaternion.Slerp(
                myTransform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );

            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void SetNewRoamTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection.y = 0;
        roamTarget = myTransform.position + randomDirection;
    }

    void LoseGame()
    {
        Debug.Log("You lose!");
        playerLost = true;
        anim.SetBool("isWalking", false);

        if (target != null)
        {
            var controller = target.GetComponent<PlayerMovement>();
            if (controller != null)
                controller.enabled = false;
        }
    }
}
