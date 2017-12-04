using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankHealth : MonoBehaviour
    {
        public float m_StartingHealth = 100f;               // Tank'ın başlangıçtaki canı
        public Slider m_Slider;                             // Mevcut sağlığı gösteren slider
        public Image m_FillImage;                           // Slider'ın görüntü bileşeni (slider = background + fill)
        public Color m_FullHealthColor = Color.green;       // Sağlık cubuğunun tam iken hali
        public Color m_ZeroHealthColor = Color.red;         // sağlık sıfır iken rengi
        public GameObject m_ExplosionPrefab;                // Tnak öldüğü zaman sapawn edilecek prefab
        
        
        private AudioSource m_ExplosionAudio;               // Tnak patlayınca çalacak olan ses
        private ParticleSystem m_ExplosionParticles;        // Tank patlayınca çıkacak partikül
        public float m_CurrentHealth;                      // Mevcut sağlık durumu
        private bool m_Dead;                                // ölp ölmediğinin kontrolü



        private void OnEnable()
        {
            // başlangıç değerlierini sıfırla
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;

            // Slider'ı güncelle
            SetHealthUI();
        }


        //hasar alamfanksiyonu
        public void TakeDamage (float amount)
        {
            // gelen hasara göre nevcut sağlık durumnu azaltma
            m_CurrentHealth -= amount;

            // UI'ı güncelle
            SetHealthUI ();

            // Ölümü kontrol et
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath ();
            }
        }

        //UI'ı güncelle (UI = user Interface)
        private void SetHealthUI ()
        {
            // Slider'ın değerini ayarlama
            m_Slider.value = m_CurrentHealth;

            // seçilen renkler arasında sağlık durumuna göre slkiderın rengin iayarlama
            m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }


        //ölünce çalışcak
        private void OnDeath ()
        {
            // öldü (bi daha ölmesine gerke yok)
            m_Dead = true;

            // Partikül'ü sapwn etme ve tankın pozisyonuna taşıma
            m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
            m_ExplosionParticles.transform.position = transform.position;

            //m_ExplosionParticles = Instantiate(m_ExplosionPrefab,transform.position,Quaternion.identity).GetComponent<ParticleSystem>();  //bu şekilde de yapılabilir :)

            // prafb taki ses kaynağına erişip oynatma
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
            m_ExplosionParticles.Play ();

            // Tankı pasif hale getirme
            gameObject.SetActive (false);
        }
    }
}