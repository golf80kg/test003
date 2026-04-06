using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;

    void Awake() //โหลดค่า UI (ไม่ยิง Event)
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

    void Start()//รอ AudioManager
    {
        RefreshAudio();
    }

    public void RefreshAudio()
    {   
        //ดึงค่า Volume ที่เคยบันทึกไว้
        float currentVol = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        //ดึงค่า Mute ที่เคยบันทึกไว้และแปลงกลับเป็น bool 1 จะได้ true, ถ้าเป็น 0 จะได้ false
        bool currentMute = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        StartCoroutine(ApplySavedSettings(currentVol, currentMute));
    }

    private IEnumerator ApplySavedSettings(float volume, bool isMuted)
    {
        //หยุดรอตรงนี้ก่อน โดยเช็คทุก frame ว่า AudioManager
        yield return new WaitUntil(() => AudioManager.instance != null);
        ApplyVolume(volume, isMuted);
    }

    public void OnSliderChanged(float value)// เรียกฟังก์ชันนี้อัตโนมัติทุกครั้งที่ผู้เล่นขยับ Slider
    {
        ApplyVolume(value, muteToggle.isOn);//ส่งค่า Volume ใหม่ไปปรับเสียงทันทีถ้ากรณี Mute ถูกใช้งาน
        PlayerPrefs.SetFloat("MasterVolume", value);//บันทึกค่า Volume ใหม่ลง PlayerPrefs
        PlayerPrefs.Save(); //เขียนข้อมูลลงดิสก์จริงๆ ถ้าไม่เรียกบรรทัดนี้ ค่าอาจหายถ้าเกม crash
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
        //เช็คก่อนว่า AudioManager และ Mixer มีอยู่จริงถ้าไม่เช็ค แล้ว AudioManager ยังไม่พร้อม → เกิด NullReferenceException
        
        {
            float targetDB = isMuted ? -80f : Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20;
            //แปลงค่า Volume (0-1) เป็น Decibel ที่ Mixer รับได้ 
            // ป้องกัน volume = 0 เพราะ Log10(0) = ติดลบอนันต์ → Mixer พังจึงกำหนดให้ต่ำสุดได้แค่ 0.0001f
            
            AudioManager.instance.mainMixer.SetFloat("MasterVol", targetDB);
            //ส่งค่า dB ที่คำนวณได้ไปให้ Audio Mixer ปรับเสียงจริงๆ
            //MasterVol คือชื่อ ExposeParameter ที่ตั้งไว้ใน Unity Audio Mixer
        
        }
    }
}