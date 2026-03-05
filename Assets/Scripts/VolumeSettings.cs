using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public Image brightnessOverlay;
    public static VolumeSettings Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.3f);

        AudioListener.volume = savedVolume;
        ApplyBrightness(savedBrightness);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void SetBrightness(float value)
    {
        ApplyBrightness(value);
        PlayerPrefs.SetFloat("Brightness", value);
    }

    private void ApplyBrightness(float value)
    {
        if (brightnessOverlay == null) return;

        float invertedValue = 0.6f - value;
        Color c = brightnessOverlay.color;
        c.a = Mathf.Clamp(invertedValue, 0f, 0.6f);
        brightnessOverlay.color = c;
    }
}