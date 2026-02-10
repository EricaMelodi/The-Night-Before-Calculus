using UnityEngine;
using TMPro;

public class Paper : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float collectDistance = 6f;            // Max avstånd för att kunna se/picka upp papper
    public TextMeshProUGUI collectPrompt;         // Dra in TextMeshPro-objektet i inspector
    public string collectMessage = "Paper collected!"; // Meddelande som visas när detta papper plockas upp

    [Header("Counter Settings")]
    public static int papersLeft = 10;            // Totalt antal papper i scenen

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No MainCamera found in scene!");
        }

        if (collectPrompt != null)
            collectPrompt.gameObject.SetActive(false); // Dölj prompt initialt
    }

    void Update()
    {
        if (cam == null) return;

        CheckLookAtPaper();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCollectPaper();
        }
    }

    // Kollar om spelaren ser papperet
    void CheckLookAtPaper()
    {
        if (collectPrompt == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, collectDistance) && hit.collider.gameObject == gameObject)
        {
            collectPrompt.gameObject.SetActive(true);
        }
        else
        {
            collectPrompt.gameObject.SetActive(false);
        }
    }

    // Försök plocka upp papperet
    void TryCollectPaper()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, collectDistance) && hit.collider.gameObject == gameObject)
        {
            CollectPaper();
        }
    }

    void CollectPaper()
    {
        papersLeft = Mathf.Max(papersLeft - 1, 0); // Räkna ner papper utan att gå under 0
        Debug.Log($"{collectMessage} {papersLeft} left");

        if (collectPrompt != null)
            collectPrompt.gameObject.SetActive(false);

        Destroy(gameObject);
    }
}
