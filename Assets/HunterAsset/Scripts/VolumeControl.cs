using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;

    void Awake()
    {
        // 1. ดึงค่าที่เคยเซฟไว้
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        // 2. ปิดการส่ง Event ชั่วคราว เพื่อไม่ให้มันไปเซฟทับตอนเราโหลดค่า
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners(); 
            volumeSlider.value = savedVolume;
            // เปิดการฟัง Event กลับคืนมาหลังจากตั้งค่าเสร็จ
            volumeSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        if (muteToggle != null)
        {
            muteToggle.onValueChanged.RemoveAllListeners();
            muteToggle.isOn = isMuted;
            muteToggle.onValueChanged.AddListener(OnToggleMuted);
        }
    }

    void Start()
    {
        RefreshAudio();
    }

    public void RefreshAudio()
    {
        float currentVol = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        bool currentMute = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        StartCoroutine(ApplySavedSettings(currentVol, currentMute));
    }

    private IEnumerator ApplySavedSettings(float volume, bool isMuted)
    {
        yield return new WaitUntil(() => AudioManager.instance != null);
        ApplyVolume(volume, isMuted);
    }

    public void OnSliderChanged(float value)
    {
        ApplyVolume(value, muteToggle.isOn);
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save(); 
    }

    public void OnToggleMuted(bool isMuted)
    {
        ApplyVolume(volumeSlider.value, isMuted);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float volume, bool isMuted)
    {
        if (AudioManager.instance != null && AudioManager.instance.mainMixer != null)
        {
            float targetDB = isMuted ? -80f : Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20;
            AudioManager.instance.mainMixer.SetFloat("MasterVol", targetDB);
        }
    }
}