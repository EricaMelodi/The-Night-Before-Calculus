using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareManager : MonoBehaviour
{
    public float delay = 2.8f;               // Hur lång din jumpscare-animation är i sekunder
    public string gameOverSceneName = "GameOverScen";

    void Start()
    {
        Invoke(nameof(LoadGameOver), delay);
    }

    void LoadGameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}