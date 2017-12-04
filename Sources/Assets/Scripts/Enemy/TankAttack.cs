using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Complete
{
    public class TankAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // iki atak arası süre

        GameObject player;                          // player objesini referansı
        EnemyMovement enemyMovement;                // 
        float timer;                                // saldırı için sayıcı


        public Rigidbody m_Shell;                   // Mermi'nin prefab
        public Transform m_FireTransform;           // Merminin instaiate olacağı nokta
        public Slider m_AimSlider;                  // Fırlatma kuvvetini gösteren slider
        public AudioSource m_ShootingAudio;         // Çekim sesi(mermi atmak için basınca)
        public AudioClip m_ChargingClip;            // şarj olma sesi
        public AudioClip m_FireClip;                // ateş edilince çalaack ses
        public float m_MinLaunchForce = 15f;        // minimum atış kuvveti
        public float m_MaxLaunchForce = 30f;        // Maksimum atış kuvveti


        void Awake ()
        {
            player = GameObject.FindGameObjectWithTag("Player").gameObject;
            enemyMovement = GetComponent<EnemyMovement>();
        }





        void Update ()
        {
            timer += Time.deltaTime;

            // 
            //Debug.Log("distance: " + Vector3.Distance(player.transform.position, transform.position));

            if (timer >= timeBetweenAttacks)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < Random.Range(m_MinLaunchForce, m_MaxLaunchForce))
                {
                    enemyMovement.StartStop(false);
                    Attack();
                }
                else
                {
                    enemyMovement.StartStop(true);
                }
            }
        }


        void Attack ()
        {

            timer = 0f;
            // ateş edildi

            // merminin bir örenğini sahnede oluşturma
            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // merminin hızını ve yönünü ayarlama
            shellInstance.velocity = Random.Range(m_MinLaunchForce, m_MaxLaunchForce) * m_FireTransform.forward;

            // ateş etme sesinin oynatılması
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

            // fırlatma kuvvetini sıfrılama

        }
    }
}