using UnityEngine;

public class Door : MonoBehaviour, IInteractable
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

        // BoxCollider ska inte vara trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = false;
    }

    void Update()
    {
       
        Quaternion target = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * openSpeed);
    }

    // PlayerInteraction kallar detta när E trycks
    public void Interact()
    {
        // ÄNDRA TILL 0 SENARE FIX!!!! chekkk
        if (Paper.papersLeft == 10)
        {
            isOpen = !isOpen;
        }
    }

    // Text som ska visas i prompten
    public string GetInteractText()
    {
        // ÄNDRA TILL 0 SENARE FIX!!!! chekkkk
        if(Paper.papersLeft != 10)
        {
            return "Still missing pages";
        }
        return isOpen ? "Press E to close door" : "Press E to open door";
    }
}
