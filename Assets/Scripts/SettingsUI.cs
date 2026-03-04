using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider brightnessSlider;

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event to refresh sliders when returning to menu
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        SetupSliders();
    }

    // This triggers automatically when any scene finishes loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupSliders();
    }

    private void SetupSliders()
    {
        // Find the global VolumeSettings instance if we don't have it
        VolumeSettings settings = VolumeSettings.Instance;

        if (volumeSlider == null || brightnessSlider == null || settings == null)
        {
            Debug.LogWarning("SettingsUI: Missing Sliders or VolumeSettings Instance!");
            return;
        }

        // Set slider positions based on saved data
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 0.3f);

        // Clean up old listeners to prevent "Double Calling" or errors
        volumeSlider.onValueChanged.RemoveAllListeners();
        brightnessSlider.onValueChanged.RemoveAllListeners();

        // Link sliders to the VolumeSettings methods
        volumeSlider.onValueChanged.AddListener(settings.SetVolume);
        brightnessSlider.onValueChanged.AddListener(settings.SetBrightness);
    }
}