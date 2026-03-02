using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;

    [Header("Movement")]
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float crouchSpeed = 10f;
    public float jumpPower = 10f;
    public float gravity = 30f;

    [Header("Look")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Crouch")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float cameraCrouchOffset = 0.5f;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaDrainRate = 1f;
    public float staminaRegenRate = 0.8f;
    public float staminaRegenDelay = 1.5f;
    [HideInInspector] public float currentStamina;
    private float staminaRegenTimer;

    [Header("UI")]
    public Slider staminaSlider; // assign per scene

    [Header("Sounds")]
    public AudioClip footstepClip;
    public AudioSource walkingAudioSource;
    public AudioClip sprintStepClip;
    public AudioSource sprintingAudioSource;
    public float walkingStepInterval = 0.5f;
    public float sprintingStepInterval = 0.3f;

    private Vector3 moveDirection;
    private float rotationX = 0;
    private CharacterController controller;
    private float defaultCameraY;
    private float footstepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        defaultCameraY = playerCamera.transform.localPosition.y;
        currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();

        // Update UI safely
        if (staminaSlider != null && staminaSlider.gameObject != null)
            staminaSlider.value = currentStamina / maxStamina;
    }

    void HandleMovement()
    {
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        bool isMovingInput =
            Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f ||
            Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;

        bool isRunning = wantsToRun && currentStamina > 0f && !isCrouching;

        float speed = walkSpeed;
        if (isCrouching)
            speed = crouchSpeed;
        else if (isRunning)
            speed = runSpeed;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float moveX = Input.GetAxis("Vertical") * speed;
        float moveZ = Input.GetAxis("Horizontal") * speed;

        float yVelocity = moveDirection.y;
        moveDirection = (forward * moveX) + (right * moveZ);

        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump") && !isCrouching)
                yVelocity = jumpPower;
            else
                yVelocity = -1f;
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = yVelocity;

        controller.Move(moveDirection * Time.deltaTime);

        // ===== STAMINA SYSTEM =====
        if (isRunning && isMovingInput && controller.isGrounded)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            staminaRegenTimer = 0f;
        }
        else
        {
            if (staminaRegenTimer < staminaRegenDelay)
                staminaRegenTimer += Time.deltaTime;
            else if (currentStamina < maxStamina)
                currentStamina += staminaRegenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // ===== FOOTSTEP SOUNDS =====
        bool isMoving = controller.velocity.magnitude > 0.1f;
        footstepTimer -= Time.deltaTime;

        if (controller.isGrounded && isMoving)
        {
            if (footstepTimer <= 0f)
            {
                if (isRunning)
                {
                    sprintingAudioSource.PlayOneShot(sprintStepClip);
                    footstepTimer = sprintingStepInterval;
                }
                else
                {
                    walkingAudioSource.PlayOneShot(footstepClip);
                    footstepTimer = walkingStepInterval;
                }
            }
        }

        // ===== CROUCH =====
        float targetHeight = isCrouching ? crouchHeight : defaultHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * 10f);

        float targetCamY = isCrouching
            ? defaultCameraY - cameraCrouchOffset
            : defaultCameraY;

        Vector3 camPos = playerCamera.transform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCamY, Time.deltaTime * 10f);
        playerCamera.transform.localPosition = camPos;
    }

    void HandleMouseLook()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}