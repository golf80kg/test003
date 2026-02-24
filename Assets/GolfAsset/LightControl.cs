using UnityEngine;

public class LightControl : MonoBehaviour
{
    public Light myLight;
    public AudioSource clickSound; 

    void Start()
    {
        // สั่งให้ปิดไฟทันทีที่เริ่มเกม
        if (myLight != null)
        {
            myLight.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // สลับการเปิด-ปิดไฟ
            if (myLight != null)
            {
                myLight.enabled = !myLight.enabled;
            }

            // เล่นเสียงคลิกทุกครั้งที่กด F
            if (clickSound != null)
            {
                clickSound.Play();
            }
        }
    }
}