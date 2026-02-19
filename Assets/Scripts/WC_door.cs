using UnityEngine;

public class WC_door : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        // Se till att Box Collider inte är trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = false;
    }

    void Update()
    {
        // Roterar dörren mot målet
        Quaternion target = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * openSpeed);
    }

    // Kallas av PlayerInteraction när E trycks
    public void Interact()
    {
        isOpen = !isOpen; // Öppna eller stäng dörren
    }

    // Text som visas i prompten
    public string GetInteractText()
    {
        return isOpen ? "Press E to close door" : "Press E to open door";
    }
}
