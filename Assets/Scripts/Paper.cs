using UnityEngine;

public class Paper : MonoBehaviour, IInteractable
{
    public static int papersLeft = 10;

    public void Interact()
    {
        papersLeft = Mathf.Max(papersLeft - 1, 0);
        Debug.Log($"Paper collected! {papersLeft} left");
        Destroy(gameObject);
    }

    public string GetInteractText() => "Press E to collect paper";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Visa UI, t.ex.
            Debug.Log("Press E to collect");
        }
    }
}
