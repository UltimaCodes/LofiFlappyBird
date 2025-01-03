using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle rainToggle;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Button resetButton;
    public GameObject rainObject;

    private void Start()
    {
        // Load saved settings (if any) from PlayerPrefs
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        rainToggle.isOn = PlayerPrefs.GetInt("Rain", 1) == 1;
        fullscreenToggle.isOn = Screen.fullScreen;

        // Set the resolution dropdown options
        Resolution[] resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var resolutionOptions = new System.Collections.Generic.List<string>();

        foreach (var resolution in resolutions)
        {
            resolutionOptions.Add(resolution.width + "x" + resolution.height);
        }

        resolutionDropdown.AddOptions(resolutionOptions);

        // Set the currently selected resolution
        int currentResolutionIndex = System.Array.FindIndex(resolutions, r => r.width == Screen.width && r.height == Screen.height);
        resolutionDropdown.value = currentResolutionIndex >= 0 ? currentResolutionIndex : 0;

        // Add listeners for changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        rainToggle.onValueChanged.AddListener(OnRainToggleChanged);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
        resetButton.onClick.AddListener(OnResetSettings);

        // Apply the settings at the start
        ApplySettings();
    }

    private void OnVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        AudioListener.volume = value;
    }

    private void OnRainToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("Rain", isOn ? 1 : 0);
        rainObject.SetActive(isOn);
    }

    private void OnResolutionChanged(int index)
    {
        Resolution[] resolutions = Screen.resolutions;
        Resolution selectedResolution = resolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    private void OnFullscreenToggleChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void OnResetSettings()
    {
        // Reset to default settings
        volumeSlider.value = 1f;
        rainToggle.isOn = true;
        resolutionDropdown.value = 0; // Reset to the first resolution
        fullscreenToggle.isOn = true;

        // Apply the reset settings
        ApplySettings();
    }

    private void ApplySettings()
    {
        // Apply volume
        AudioListener.volume = volumeSlider.value;

        // Apply rain toggle
        rainObject.SetActive(rainToggle.isOn);

        // Apply fullscreen and resolution
        Resolution[] resolutions = Screen.resolutions;
        Resolution selectedResolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);
    }
}
