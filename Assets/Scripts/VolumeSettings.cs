using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    // The Image used to darken the screen
    public Image brightnessOverlay;

    // Static instance allows other scripts to find this one easily
    public static VolumeSettings Instance { get; private set; }

    void Awake()
    {
        // Singleton Pattern: Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Exit early so Start() doesn't run on the duplicate
        }
    }

    void Start()
    {
        // Load saved settings on startup
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
        c.a = Mathf.Clamp(invertedValue, 0, 1);
        brightnessOverlay.color = c;
    }
}