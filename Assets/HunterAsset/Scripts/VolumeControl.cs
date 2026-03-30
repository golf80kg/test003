using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;

    // ใช้ Awake เพื่อตั้งค่าหน้าตา UI ก่อนที่ Start จะเริ่มทำงาน
    void Awake()
    {
        // 1. ดึงค่าที่เคยเซฟไว้ทันที (ถ้าไม่มีใช้ 0.75f)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        // 2. บังคับให้ UI แสดงค่าตามที่เซฟไว้
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        if (muteToggle != null)
        {
            muteToggle.isOn = isMuted;
        }
    }

    void Start()
    {
        // เริ่มต้นส่งค่าไปยัง Mixer
        RefreshAudio();
    }

    // ฟังก์ชันช่วยโหลดค่าและส่งไปที่ Mixer อีกครั้ง (กันเหนียว)
    public void RefreshAudio()
    {
        float currentVol = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        bool currentMute = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        
        StartCoroutine(ApplySavedSettings(currentVol, currentMute));
    }

    private IEnumerator ApplySavedSettings(float volume, bool isMuted)
    {
        // รอจนกว่า AudioManager.instance จะพร้อม
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