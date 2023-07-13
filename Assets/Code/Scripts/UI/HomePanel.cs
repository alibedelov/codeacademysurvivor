using Code.Scripts.Managers;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class HomePanel : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 0;
        }

        public void StartGame()
        {
            LevelManager.Instance.LoadLevel(LevelManager.Instance.CurrentLevel);
        }
    }
}