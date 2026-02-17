using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game"); 
    }
}
