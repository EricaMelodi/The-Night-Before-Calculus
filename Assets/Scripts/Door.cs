using UnityEngine;

public class Door : MonoBehaviour
{
    public static bool hasPaper = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasPaper)
            {
                Debug.Log("Door opened!");
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("You need the paper first!");
            }
        }
    }
}
