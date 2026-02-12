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

    public string GetInteractText()
    {
        return "Press E to collect paper";
    }
}
