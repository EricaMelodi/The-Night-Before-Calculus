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

    void Start()
    {
        SceneManager.SetActiveScene(gameObject.scene);

        RenderSettings.ambientIntensity = 0.02f;                    
        RenderSettings.ambientLight = new Color(0.05f, 0.05f, 0f); 
    }

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
        SceneManager.UnloadSceneAsync(gameObject.scene.name);
        Paper.isLoadingScene = false;
    }
}