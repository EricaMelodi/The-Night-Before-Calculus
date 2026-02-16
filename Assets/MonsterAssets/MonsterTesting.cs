using UnityEngine;

public class MonsterTesting : MonoBehaviour
{
    public Transform target;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float rotationSpeed = 5f;
    public float range = 10f;
    public float stopDistance = 1f;

    [Header("Roaming")]
    public float roamRadius = 30f;
    private Vector3 roamTarget;

    [Header("Forced Chase")]
    public float forcedChaseDuration = 30f;
    public float forcedChaseCooldown = 20f;

    private bool isForcedChasing = false;
    private bool forcedChaseOnCooldown = false;
    private float forcedChaseTimer = 0f;

    private Rigidbody rb;
    private Animator anim;
    private bool playerLost = false;

    void Awake()
    {
        if (target == null)
            target = GameObject.FindWithTag("Player").transform;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze X/Z rotation to prevent tipping

        anim = GetComponent<Animator>();

        SetNewRoamTarget();
        anim.SetBool("isRunning", false); // Start in walking state
    }

    void Update()
    {
        if (playerLost)
        {
            anim.SetBool("isRunning", false);
            return;
        }

        HandleForcedChaseTimer();

        float distance = Vector3.Distance(transform.position, target.position);

        if (isForcedChasing || distance <= range)
            ChasePlayer(distance);
        else
            Roam();

        if (distance <= 1f)
            LoseGame();
    }

    void FixedUpdate()
    {
        // All Rigidbody movement happens here (handled in ChasePlayer/Roam)
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
        if (!isForcedChasing) return;

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
        Vector3 direction = target.position - transform.position;

        // Keep only horizontal direction
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            // Smooth rotation
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        // Only move if further than stop distance
        if (distance > stopDistance)
        {
            Vector3 move = transform.forward * runSpeed * Time.fixedDeltaTime;
            Vector3 newPos = rb.position + move;

            // Stick to ground using raycast
            RaycastHit hit;
            if (Physics.Raycast(newPos + Vector3.up * 2f, Vector3.down, out hit, 10f))
            {
                newPos.y = hit.point.y;
            }

            rb.MovePosition(newPos);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void Roam()
    {
        Vector3 direction = roamTarget - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.5f)
        {
            // Smooth rotation
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime));

            // Move forward
            Vector3 move = transform.forward * walkSpeed * Time.fixedDeltaTime;
            Vector3 newPos = rb.position + move;

            // Stick to ground using raycast
            RaycastHit hit;
            if (Physics.Raycast(newPos + Vector3.up * 2f, Vector3.down, out hit, 10f))
            {
                newPos.y = hit.point.y;
            }

            rb.MovePosition(newPos);
            anim.SetBool("isRunning", false); // Walk animation handled by Animator
        }
        else
        {
            SetNewRoamTarget();
        }
    }


    void SetNewRoamTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection.y = 0;
        roamTarget = transform.position + randomDirection;
    }

    void LoseGame()
    {
        Debug.Log("You lose!");
        playerLost = true;
        anim.SetBool("isRunning", false);

        if (target != null)
        {
            var controller = target.GetComponent<PlayerMovement>();
            if (controller != null)
                controller.enabled = false;
        }
    }
}
