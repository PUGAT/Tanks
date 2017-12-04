using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public Rigidbody m_Shell;                   // Mermi'nin prefab
        public Transform m_FireTransform;           // Merminin instaiate olacağı nokta
        public Slider m_AimSlider;                  // Fırlatma kuvvetini gösteren slider
        public AudioSource m_ShootingAudio;         // Çekim sesi(mermi atmak için basınca)
        public AudioClip m_ChargingClip;            // şarj olma sesi
        public AudioClip m_FireClip;                // ateş edilince çalaack ses
        public float m_MinLaunchForce = 15f;        // minimum atış kuvveti
        public float m_MaxLaunchForce = 30f;        // Maksimum atış kuvveti
        public float m_MaxChargeTime = 0.75f;       // şarj zamanı

        private float m_CurrentLaunchForce;         // ateş edilince mermiye verilecek kuvvet
        private float m_ChargeSpeed;                // ne kadar hızlı şarj olduğu değeri
        private bool m_Fired;                       // ateş edilip edilmediği


        private void OnEnable()
        {
            // fırlatma kuvvetini ve arayüzü
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
        }


        private void Start ()
        {

            // şarj oranın hesaplanması
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }


        private void Update ()
        {
            // sliderın ilk değeri
            m_AimSlider.value = m_MinLaunchForce;

            // ateş edilmediyyse ve maksimum mu geçtiye
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                // maksimuma ayarlam ve ateş etme
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire ();
            }
            // ateş etme tuşuna basılmışsa
            else if (Input.GetKeyDown (KeyCode.Space))
            {
                // kuvveti ve ateşetti değerini sıfırlama
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                // şarj oluyo sesini çalma
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play ();
            }
            // tuşa basılı tutluyorsa
            else if (Input.GetKey (KeyCode.Space) && !m_Fired)
            {
                //  fırlatma kuvvetini arttırma
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            // tuştan kaldırdı ve ateş edilmediyse
            else if (Input.GetKeyUp (KeyCode.Space) && !m_Fired)
            {
                // dan dan dan :D
                Fire ();
            }
        }

        //ateş etme metodu
        public void Fire ()
        {
            // ateş edildi
            m_Fired = true;

            // merminin bir örenğini sahnede oluşturma
            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // merminin hızını ve yönünü ayarlama
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;


            // ateş etme sesinin oynatılması
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();

            // fırlatma kuvvetini sıfrılama
            m_CurrentLaunchForce = m_MinLaunchForce;
        }
    }
}