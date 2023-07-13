using CASP.SoundManager;
using System;
using UnityEngine;

namespace Code.Scripts.Managers
{
    public class GameManager : SingletoneBase<GameManager>
    {
        public GameState gameState;
        public event Action OnStartGame;
        public event Action OnPauseGame;
        public event Action OnResumeGame;
        public event Action OnWinGame;
        public event Action OnLooseGame;
        public event Action OnRestartGame;

        public void StartGame()
        {
            OnStartGame?.Invoke();
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            OnPauseGame?.Invoke();
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            OnResumeGame?.Invoke();
        }

        public void WinGame()
        {
            OnWinGame?.Invoke();
        }

        public void LooseGame()
        {
            OnLooseGame?.Invoke();
        }

        public void RestartGame()
        {
            OnRestartGame?.Invoke();
        }
    }
}