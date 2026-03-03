using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Image brightnessOverlay;

    void Start()
    {
        volumeSlider.value = 0.5f;
        AudioListener.volume = 0.5f;

        volumeSlider.onValueChanged.AddListener(SetVolume);

        brightnessSlider.value = 0.3f;
        brightnessSlider.onValueChanged.AddListener(SetBrightness);

        SetBrightness(0.3f);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetBrightness(float value)
    {
        float invertedValue = 0.6f - value;
        Color c = brightnessOverlay.color;
        c.a = invertedValue;
        brightnessOverlay.color = c;
    }
}