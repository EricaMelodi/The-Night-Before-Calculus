    
using UnityEngine;

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

    private Vector3 moveDirection;
    private float rotationX = 0;
    private CharacterController controller;
    private float defaultCameraY;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        defaultCameraY = playerCamera.transform.localPosition.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

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

        // Crouch height + camera
        float targetHeight = isCrouching ? crouchHeight : defaultHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * 10f);

        float targetCamY = isCrouching
            ? defaultCameraY - cameraCrouchOffset
            : defaultCameraY;

        Vector3 camPos = playerCamera.transform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCamY, Time.deltaTime * 10f);
        playerCamera.transform.localPosition = camPos;

        controller.Move(moveDirection * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
