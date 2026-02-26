using UnityEngine;
using System.Collections;

public class JumpscareZoom : MonoBehaviour
{
    public Camera cam;
    public float targetFOV = 60f;
    public float zoomSpeed = 50f;

    void StartZoom()
    {
        StartCoroutine(ZoomIn());
    }

    IEnumerator ZoomIn()
    {
        while (cam.fieldOfView > targetFOV)
        {
            cam.fieldOfView -= zoomSpeed * Time.deltaTime;
            yield return null;
        }
    }
}