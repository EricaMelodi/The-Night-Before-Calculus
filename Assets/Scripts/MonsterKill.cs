using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterKill : MonoBehaviour
{
    public string jumpscareSceneName = "JumpscareScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        Debug.Log("Player caught!");

        SceneManager.LoadScene(jumpscareSceneName);
    }
}
