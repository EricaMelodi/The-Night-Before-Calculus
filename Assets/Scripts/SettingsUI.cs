using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider brightnessSlider;

    void Start()
    {
        SetupSliders();
    }

    public void SetupSliders()
    {
        VolumeSettings settings = VolumeSettings.Instance;

        if (volumeSlider == null || brightnessSlider == null || settings == null)
            return;

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 0.3f);

        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(settings.SetVolume);

        brightnessSlider.onValueChanged.RemoveAllListeners();
        brightnessSlider.onValueChanged.AddListener(settings.SetBrightness);
    }
}