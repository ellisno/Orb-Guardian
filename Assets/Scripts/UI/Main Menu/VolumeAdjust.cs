using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the UI slider for volume control

    private void Start()
    {
        // Ensure that the volume slider value matches the initial volume
        volumeSlider.value = AudioListener.volume;
        // Add a listener to the volume slider
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // Method to handle volume change when the slider value changes
    private void OnVolumeChanged(float value)
    {
        // Update the volume of the AudioListener
        AudioListener.volume = value;
    }
}
