using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Paper : MonoBehaviour, IInteractable

{
    public static bool isLoadingScene = false;

    public static int papersLeft = 10;

    public static PapersUI papersUI; // referens till UI

    [Header("Monster Reference")]
    public MonsterAI monster;

    [Header("Monster Spawn")]
    public Transform monsterSpawnPoint;

    void Awake()
    {
        papersLeft = 10;
    }

    void Start()
    {
        // hitta UI automatiskt första gången
        if (papersUI == null)
        {
            papersUI = FindObjectOfType<PapersUI>();
            papersUI.UpdateText();
        }
    }


    public void Interact()
    {
        papersLeft = Mathf.Max(papersLeft - 1, 0);
        papersUI.UpdateText();

        Debug.Log($"Paper collected! {papersLeft} left");

        if (papersLeft == 8 && !isLoadingScene)
        {
            isLoadingScene = true;
            SceneManager.LoadScene("PianoScene", LoadSceneMode.Additive);
        }

        if (papersLeft == 8 && monster != null && !monster.gameObject.activeInHierarchy)
        {
            monster.ActivateMonster(monsterSpawnPoint.position);
        }

        if (papersLeft <= 5 && papersLeft > 0 && monster != null)
        {
            monster.TriggerHunt();
        }

        if (papersLeft == 0 && monster != null)
        {
            monster.TriggerHunt();
        }

        Destroy(gameObject);
    }

    public string GetInteractText() => "Press E to collect paper";
}