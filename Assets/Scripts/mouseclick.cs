using UnityEngine;

public class GameOverSetup : MonoBehaviour
{
    void Start()
    {
        // Visa musen
        Cursor.visible = true;

        // Lås upp musen så den kan röra sig fritt
        Cursor.lockState = CursorLockMode.None;
    }

}