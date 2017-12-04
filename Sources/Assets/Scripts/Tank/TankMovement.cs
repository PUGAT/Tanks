using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public float m_Speed = 12f;                 // Tank' ileri geri/geri hızı
        public float m_TurnSpeed = 180f;            // Tank'ın dönme hızı
        private AudioSource m_MovementAudio;         // Motor Seslerini çalmak için referans(Çıkış kaynağı)
        public AudioClip m_EngineIdling;            // Tnak sabit iken çalan ses
        public AudioClip m_EngineDriving;           // Tank hareket ederken çıkan ses

        private Rigidbody m_Rigidbody;              // Tnak'ın referans (katı cisim)
        private float m_MovementInputValue;         // Hareket için girş değeri
        private float m_TurnInputValue;             // Dönme için giriş değeri
        private float m_OriginalPitch;              // Sahne başında ses kaynağının perdesi(gürültüsü)
        private ParticleSystem[] m_particleSystems; // Partikül sistemlerin referansları

        //uyanınca çalışır
        private void Awake ()
        {
            //Debug.Log("Awake");                             //
            m_Rigidbody = GetComponent<Rigidbody>();        //rigidbody bileşenine erişmek
            m_MovementAudio = GetComponent<AudioSource>();  //audioSource bileşenine erişmek
        }

        //Script kullanılabilir olduğunda çalışşır
        private void OnEnable ()
        {
            //Debug.Log("OnEnable");
            
            m_Rigidbody.isKinematic = false;                //hareket edebilir hale getirmek(kinematic = sabit)

            m_MovementInputValue = 0f;                      //giriş değerlerini sıfırlama
            m_TurnInputValue = 0f;                          //giriş değerlerini sıfırlama


            //tank ta bulunan partikül sistemlerine erişme
            //bu sayede dah sonra partiküller üzerinde işlke yapabilriz
            //(bu işlem manuel olark el ilede yapılabilir)
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }

        //script kullanılamz olunca çalışır
        private void OnDisable ()
        {

            m_Rigidbody.isKinematic = true;                     //Tnak kapandığı zaman hareketini engellemk için

            // Tnak durduğu zaman partikülleri durdurmak
            for(int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }


        private void Start ()
        {
            //Debug.Log("Start");
            
            m_OriginalPitch = m_MovementAudio.pitch;        //ses kaynağının oarjinal adım aralığını ayarlamk
        }

        //Her frame de çalışır
        private void Update ()
        {
            // Girş değerlerini alma
            m_MovementInputValue = Input.GetAxis ("Vertical");
            m_TurnInputValue = Input.GetAxis ("Horizontal");

            EngineAudio ();
        }


        private void EngineAudio ()
        {
            float m_PitchRange = 0.2f; //motor adım aralığı
            
            //eğer giriş değeri yoksa(tank duruyo)
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {
                // ama hareket halinde olduğu ses çalıyorsa
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // audioClip'i değşitrme
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();
                }
            }
            else
            {
                //Tank hareket ediyor ama Idle(rölente) de ise
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // audioClip ideğiştirme
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }


        // belli aralıklarla sürekli çalışır(0.2 ms)
        private void FixedUpdate ()
        {
            // fizik işlemleri burda yapılır

            // yönü ve konum ayarlamk için
            Move ();
            Turn ();
        }

        // hareket fonksiyonu
        private void Move ()
        {
            // hareket vektörünü oluşturma (iki frame arasında)
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // hareket vektörünücisim üzerine uygulama
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }

        // dönme metodu(fonk)
        private void Turn ()
        {
            // Dönme açısını giriş değerine göre ayarlama
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // y de dönme aşısı
            Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);  //dördey ( karmaşık sayılar cisminin değişmesiz genişletmesidir.) (qua kapasar karmaşık) // euler dönme açısı

            // dönem açısını uygulama
            m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
        }
    }
}