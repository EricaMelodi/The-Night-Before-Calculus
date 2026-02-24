using UnityEngine;

public class Paper : MonoBehaviour, IInteractable
{
    public static int papersLeft = 10;

    [Header("Monster Reference")]
    public MonsterAI monster;

    [Header("Monster Spawn")]
    public Transform monsterSpawnPoint;

    void Awake()
    {
        // Reset papersLeft whenever scene starts
        papersLeft = 10;
    }

    public void Interact()
    {
        // Decrement papersLeft safely
        papersLeft = Mathf.Max(papersLeft - 1, 0);
        Debug.Log($"Paper collected! {papersLeft} left");

        // Spawn monster on 2nd paper
        if (papersLeft == 8 && monster != null && !monster.gameObject.activeInHierarchy)
        {
            monster.ActivateMonster(monsterSpawnPoint.position);
        }

        // Hunt phase: papers 5–9
        if (papersLeft <= 5 && papersLeft > 0 && monster != null)
        {
            monster.TriggerHunt();
        }

        // Last paper (0 left)
        if (papersLeft == 0 && monster != null)
        { 
            monster.TriggerHunt();
            // något mer måste hända här, vet ej än vad
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