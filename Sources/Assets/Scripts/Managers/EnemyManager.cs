using UnityEngine;
namespace Complete
{
    public class EnemyManager : MonoBehaviour
    {
        public TankHealth playerHealth;
        public GameObject enemy;
        public float spawnTime = 3f;
        public Transform[] spawnPoints;


        void Start()
        {
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }


        void Spawn()
        {
            if (playerHealth.m_CurrentHealth <= 0f)
            {
                return;
            }

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
    }
}