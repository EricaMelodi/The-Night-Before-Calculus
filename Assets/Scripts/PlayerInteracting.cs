using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactDistance = 3f;
    public TextMeshProUGUI interactPrompt;

    private Camera cam;
    private IInteractable currentInteractable;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("PlayerInteraction: No MainCamera found in scene!");
        }

        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);
        else
            Debug.LogWarning("PlayerInteraction: InteractPrompt is not assigned!");
    }

    void Update()
    {
        if (cam == null || interactPrompt == null) return;

        CheckForInteractable();
    }
    void CheckForInteractable()
    {
        float radius = 3.5f; // Hur stor “tålighet” raycasten ska ha
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactDistance, Color.red);

        if (Physics.SphereCast(ray, radius, out hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                interactPrompt.text = interactable.GetInteractText();
                interactPrompt.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                    currentInteractable.Interact();

                return;
            }
        }

        currentInteractable = null;
        interactPrompt.gameObject.SetActive(false);
    }
}
