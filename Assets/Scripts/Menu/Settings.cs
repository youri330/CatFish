using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; //Работа с аудио
using UnityEngine.UI; //Работа с интерфейсами


public class Settings : MonoBehaviour {
    public bool isFullScreen;
    public void FullScreenToggle() {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
    public AudioMixer am;
    public void AudioVolume(float sliderValue) {
        am.SetFloat("masterVolume", sliderValue);
    }
    public void Quality(int q) {
        QualitySettings.SetQualityLevel(q);
    }
    Resolution[] rsl;
    List<string> resolutions;
    public Dropdown dropdown;
    public void Awake() {
        resolutions = new List<string>();
        rsl = Screen.resolutions;
        foreach (var i in rsl) {
            resolutions.Add(i.width + "x" + i.height);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(resolutions);
    }
    public void Resolution(int r) {
        Screen.SetResolution(rsl[r].width, rsl[r].height, isFullScreen);
    }
}

