using UnityEngine;

public class Paper : MonoBehaviour, IInteractable
{
    public static int papersLeft = 10;

    [Header("Monster Reference")]
    public MonsterAI monster;   

    public void Interact()
    {
        papersLeft = Mathf.Max(papersLeft - 1, 0);
        Debug.Log($"Paper collected! {papersLeft} left");

        if (monster != null)
        {
            monster.TriggerHunt();
        }

        Destroy(gameObject);
    }

    public string GetInteractText() => "Press E to collect paper";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press E to collect");
        }
    }
}
