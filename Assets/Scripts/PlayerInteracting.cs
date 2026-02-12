using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactDistance = 50f;
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
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        // Visualisera raycast i Scene View
        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                interactPrompt.text = interactable.GetInteractText();
                interactPrompt.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                }
                return; // Vi har hittat något → stoppa här
            }
        }

        // Ingen interactable träffad → göm prompt
        currentInteractable = null;
        interactPrompt.gameObject.SetActive(false);
    }
}
