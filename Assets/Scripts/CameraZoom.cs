using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class JumpscareZoom : MonoBehaviour
{
    public Camera cam;
    public float targetFOV = 60f;
    public float zoomSpeed = 50f;

    public AudioSource scareSound;

    public string previousScene;

    void StartZoom()
    {
        StartCoroutine(ZoomIn());
        scareSound.Play();
    }

    IEnumerator ZoomIn()
    {
        while (cam.fieldOfView > targetFOV)
        {
            cam.fieldOfView -= zoomSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void ReturnToGame()
    {
        SceneManager.LoadScene(previousScene);
    }
}