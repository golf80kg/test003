using UnityEngine;
using UnityEngine.Audio; // สำคัญมาก ต้องมีเพื่อคุม Mixer

public class AudioManager : MonoBehaviour
{
    // ทำให้ Script อื่นๆ เรียกใช้ได้ง่ายๆ ผ่าน AudioManager.instance
    public static AudioManager instance;

    [Header("Settings")]
    public AudioMixer mainMixer; // ลากไฟล์ Audio Mixer ของคุณมาใส่ในช่องนี้ที่ Inspector

    void Awake()
    {
        // ระบบ Singleton: ตรวจสอบว่ามี AudioManager อยู่ในเกมหรือยัง
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // สั่งให้ Object นี้ไม่ถูกทำลายเมื่อเปลี่ยน Scene
        }
        else
        {
            // ถ้ามี AudioManager ตัวเก่าอยู่แล้ว ให้ทำลายตัวที่เพิ่งสร้างใหม่ทิ้งซะ
            Destroy(gameObject);
        }
    }

    // ฟังก์ชันสำหรับปรับความดัง (จะถูกเรียกจาก Script VolumeControl)
    public void SetVolume(string name, float value)
    {
        // สูตรคำนวณ Linear To Logarithmic (เพื่อให้ Slider เลื่อนแล้วเสียงดังเบาดูเป็นธรรมชาติ)
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20f;
        mainMixer.SetFloat(name, dB);
    }
}