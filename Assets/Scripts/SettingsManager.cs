using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool isOpen;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    Resolution[] resolutions;

    private void Awake() {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Start() {
        // Cache possible resolutions based on hardware
        resolutions = Screen.resolutions;

        // Clear any options
        resolutionsDropdown.ClearOptions();

        // Format resolutions
        List<string> options = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width) {
                currentResIndex = i;
            }
        }

        // Update possible resolutions
        resolutionsDropdown.AddOptions(options);
        // Mark your current resolution
        resolutionsDropdown.value = currentResIndex;
        // Update the options
        resolutionsDropdown.RefreshShownValue();
    }

    private void Update() {
        // If escape or right click is pressed
        if (isOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1) )) {
            Close();
        }
    }

    public void Open() {
        isOpen = true;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close() {
        isOpen = false;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    /// ~~~~~ Functionality Here ~~~~~

    public void SetVolume(float volume) {
        // Set mixer volume 
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
        // Debug
        Debug.Log("Quality changed to:" + qualityIndex);
    }

    public void SetResolution(int resolutionIndex) {
        // Get resolution from our dropdown
        Resolution resolution = resolutions[resolutionIndex];
        // Update resolution
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        // Debug
        Debug.Log("Resolution changed to:" + resolution.width + " x " + resolution.height);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        // Debug
        Debug.Log("Fullscreen changed to:" + isFullscreen.ToString());
    }
}
