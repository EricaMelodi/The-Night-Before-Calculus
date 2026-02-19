using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject HowToPlayPanel;

    public void OpenHowToPlay()
    {
        HowToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        HowToPlayPanel.SetActive(false);
    }
}
