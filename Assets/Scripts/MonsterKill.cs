using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareTimer : MonoBehaviour
{
    public float delay = 3f;  
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