using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    public Slider brightnessSlider;
    public PostProcessVolume postProcessVolume;

    private float defaultBrightness = 1.0f;
    private ColorGrading colorGrading;

    private void Start()
    {
        // Ensure slider value matches default brightness
        brightnessSlider.value = defaultBrightness;
        // Set initial brightness
        SetBrightness(defaultBrightness);
        // Add listener to the slider
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);

        // Get the ColorGrading effect from the Post Process Volume
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    private void OnBrightnessChanged(float value)
    {
        SetBrightness(value);
    }

    private void SetBrightness(float brightnessValue)
    {
        // Ensure brightness value is within the valid range
        brightnessValue = Mathf.Clamp(brightnessValue, 0.0f, 1.0f);

        // Modify the post processing effect to adjust brightness
        if (colorGrading != null)
        {
            colorGrading.postExposure.value = brightnessValue * 2.0f; // Adjust multiplier as needed
        }
    }
}
