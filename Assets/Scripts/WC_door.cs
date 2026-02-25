using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WC_door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("NavMesh Obstacles")]
    public NavMeshObstacle[] obstacles; // assign door + frame

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Collider doorCollider;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        // Get the door's collider
        doorCollider = GetComponent<Collider>();
        if (doorCollider != null)
            doorCollider.isTrigger = false; // solid by default

        // Auto-detect obstacles if none assigned
        if (obstacles == null || obstacles.Length == 0)
            obstacles = GetComponentsInChildren<NavMeshObstacle>();

        // Initial carving
        foreach (var obs in obstacles)
            if (obs != null)
                obs.carving = !isOpen; // carve only if closed
    }

    void Update()
    {
        // Smoothly rotate the door every frame
        Quaternion target = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * openSpeed);
    }

    public void Interact()
    {
        StopAllCoroutines();
        StartCoroutine(ToggleDoor(!isOpen));
    }

    private IEnumerator ToggleDoor(bool open)
    {
        isOpen = open;

        // Update obstacles carving
        foreach (var obs in obstacles)
            if (obs != null)
                obs.carving = !isOpen;

        // Smoothly rotate until fully open/closed
        float t = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = isOpen ? openRotation : closedRotation;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        // Disable collider if open, enable if closed
        if (doorCollider != null)
            doorCollider.enabled = !isOpen;
    }

    public string GetInteractText()
    {
        return isOpen ? "Press E to close door" : "Press E to open door";
    }
}