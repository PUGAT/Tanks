using UnityEngine;

namespace Complete
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Player'ın referans'ı
        TankHealth playerHealth;      // Player'ın mevcut canı
        TankHealth enemyHealth;        // Kendi canım
        UnityEngine.AI.NavMeshAgent nav;               // Nav mesh agent ın referansı


        void Awake ()
        {
            // Referansalaarı ayarlama //manuel olarakta yapılabilir
            player = GameObject.FindGameObjectWithTag ("Player").transform;
            playerHealth = player.GetComponent<TankHealth>();
            enemyHealth = GetComponent <TankHealth> ();
            nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
            nav.speed = Random.Range(0.5f,2f);
        }


        void Update ()
        {

            // player ve düşaman ölmamişse
            if(enemyHealth.m_CurrentHealth > 0 && playerHealth.m_CurrentHealth > 0 && nav.enabled)
            {
                // nav mesh'i playera ayarlam
                nav.SetDestination (player.position);
            }
            // ikisinden bii ölmüşse 
            else
            {
                // nav mesh'i kapatma
                nav.enabled = false;
            }
        }

       public void StartStop(bool val)
        {
            nav.enabled = val;
        }
    }
}