using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadWinningScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("WinningScene");
        }
    }
}