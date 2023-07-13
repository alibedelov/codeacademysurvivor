using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Managers
{
    public class TimeManager : SingletoneBase<TimeManager>
    {
        //Enemy kill per 10 Second
        [SerializeField] private int _enemyKilled;
        [SerializeField] private float _chanceToSpawnCollectable; // 50% chance to drop crystal exp
        [SerializeField] private CinemachineVirtualCamera _cinemachineCamera;

        [SerializeField] private int _totalEnemyKilled;

        //Events
        public event Action OnFirstMinuteEvent;
        public event Action OnTwoMinuteEvent;
        public event Action OnThreeMinuteEvent;
        public event Action OnFourMinuteEvent;
        public event Action OnFiveMinuteEvent;
        public event Action<int> OnTenSecondEvent; // New event

        //Time Field
        private float _gameTime;
        private float _previousGameTime;
        private float _firstMinuteEventTime = 60;
        private float _twoMinuteEventTime = 2 * 60;
        private float _threeMinuteEventTime = 3 * 60;
        private float _fourMinuteEventTime = 4 * 60;
        private float _fiveMinuteEventTime = 5 * 60;
        private float _tenSecondEventTime = 15; // New event time


        public int TotalEnemyKilled => _totalEnemyKilled;
        public int EnemyKilled => _enemyKilled;
        public float ChanceToSpawnCollectable => _chanceToSpawnCollectable;

        private void Awake()
        {
         
            _enemyKilled = 0;
            _cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        public float GameTime => _gameTime;

        private void Update()
        {
            _previousGameTime = GameTime;
            _gameTime += Time.deltaTime;

            CheckTimeEvents(_previousGameTime, GameTime);
        }

        private void CheckTimeEvents(float previousTime, float currentTime)
        {

            if (previousTime < _firstMinuteEventTime && currentTime >= _firstMinuteEventTime)
            {
                OnFirstMinuteEvent?.Invoke();
            }
            if (previousTime < _twoMinuteEventTime && currentTime >= _twoMinuteEventTime)
            {
                UIManager.Instance.GamePanel.ShowAlert("Zombies Coming !!!");
                DOTween.To(() => _cinemachineCamera.m_Lens.OrthographicSize,
                    x => _cinemachineCamera.m_Lens.OrthographicSize = x, 14.5f, 2.5f);
                OnTwoMinuteEvent?.Invoke();
            }

            if (previousTime < _threeMinuteEventTime && currentTime >= _threeMinuteEventTime)
            {
                OnThreeMinuteEvent?.Invoke();
            }

            if (previousTime < _fourMinuteEventTime && currentTime >= _fourMinuteEventTime)
            {
                UIManager.Instance.GamePanel.ShowAlert("Zombies Coming !!!");
                OnFourMinuteEvent?.Invoke();
            }

            if (previousTime < _fiveMinuteEventTime && currentTime >= _fiveMinuteEventTime)
            {
                UIManager.Instance.GamePanel.ShowAlert("Boss is Coming !!!");
                OnFiveMinuteEvent?.Invoke();
            }

            if (previousTime < _tenSecondEventTime && currentTime >= _tenSecondEventTime)
            {
                OnTenSecondEvent?.Invoke(_enemyKilled); // Fire new event
                _enemyKilled = 0; // Reset the enemy killed count
                _tenSecondEventTime += 10; // Move to the next 10 second interval
            }
        }


        public void IncreaseEnemyKillCount()
        {
            _enemyKilled++; // Increase the enemy killed count whenever an enemy is killed
            _totalEnemyKilled++;
        }
    }
}