using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareEnd : MonoBehaviour
{
    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}