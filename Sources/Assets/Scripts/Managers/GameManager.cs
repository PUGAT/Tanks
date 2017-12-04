using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public TankHealth tankHealt;

        public Transform endPanel;

        private void Update()
        {
            if (tankHealt.m_CurrentHealth <= 0)
            {
                EndGame();
            }
        }

        void EndGame()
        {
            Time.timeScale = 0f;
            endPanel.gameObject.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(1);
        }

    }
}